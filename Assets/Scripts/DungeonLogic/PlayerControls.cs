using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MapTileGridCreator.Core;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Vector3Int location;
    [SerializeField]
    private Grid3D _grid;
    private Rigidbody rBody;
    [SerializeField]
    private Image leftButton;
    [SerializeField]
    private Image rightButton;
    [SerializeField]
    private GameObject leftLight;
    [SerializeField]
    private GameObject rightLight;
    [SerializeField]
    public Transform modelHolder;
    [SerializeField]
    private Animator animator;
    private Cell leftCell;
    private Cell rightCell;

    private Vector3 dest;

    private List<Cell> NeighbourCells = new();
    public List<Cell> FilteredOut = new();

    private bool canLeft = false;
    private bool canRight = false;

    private bool is_moving;

    public float animSpeed = 5f;

    public enum PlayerStates
    {
        IDLE,
        WALKING,
        FIGHT
    }
    public PlayerStates PlayerState;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        Vector3 dest = _grid.GetPositionCell(location) + new Vector3 (0, 0.5f, 0);
        rBody.MovePosition(dest);
        is_moving = false;
        //Moving();
        animator = modelHolder.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (PlayerState)
        {
            case (PlayerStates.IDLE):
                //animator.Play("idleWar");
                break;

            case (PlayerStates.WALKING):
                //wait for player to move
                StartCoroutine(MoveThePlayer(dest));
                break;

            case (PlayerStates.FIGHT):
                //wait for player input, countdown etc
                break;
        }
    }

    private IEnumerator MoveThePlayer(Vector3 dest)
    {
        dest = dest + new Vector3(0, 0.5f, 0);
        if (is_moving)
        {
            yield break;
        }

        is_moving = true;
        animator.Play("walk");
        modelHolder.LookAt(dest);

        while (MoveToPosition(dest))
        {
            yield return null;
        }
        animator.Play("idleWar");
        modelHolder.rotation = Quaternion.Euler(0f, 0f, 0f);
        PlayerState = PlayerStates.IDLE;
        is_moving = false;
    }

    private bool MoveToPosition(Vector3 target)
    {
        return target != (transform.position = Vector3.MoveTowards(transform.position, target, animSpeed * Time.deltaTime));
    }

    public void ControlButtons()
    {
        rightButton.gameObject.SetActive(canRight);
        leftButton.gameObject.SetActive(canLeft);
        rightLight.SetActive(canRight);
        leftLight.SetActive(canLeft);
    }

    public void MoveLeft()
    {
        dest = leftCell.transform.position;
        FilteredOut.Clear();
        NeighbourCells.Clear();
        canLeft = false;
        canRight = false;
        ControlButtons();
        PlayerState = PlayerStates.WALKING;
    }

    public void MoveRight()
    {
        dest = rightCell.transform.position;
        FilteredOut.Clear();
        NeighbourCells.Clear();
        canLeft = false;
        canRight = false;
        ControlButtons();
        PlayerState = PlayerStates.WALKING;
    }

    void OnTriggerEnter(Collider other)
    {
        canLeft = false;
        canRight = false;
        //we want to get information from the cell we just entered, basically it's index
        location = other.gameObject.GetComponent<Cell>().GetIndex();

        //then we want to trigger the event this cell provides, encounter, chest or something else.
        var scenario = other.gameObject.GetComponent<DungeonCell>();

        if(scenario.Model)
            Destroy(scenario.Model);
        scenario.isCleared = true;
        if(scenario.Scenario)
            scenario.Scenario.StepOn();
        //trigger saving of this data:


        //and we want to set the position

        //and we want gamemanager to remember this position so after let's say battle we will be spawned back where we need to be

        //detect neighbours to the left and to the right
        NeighbourCells = new(_grid.GetNeighboursCell(ref location)); //checks for all neighbour cells
        FilteredOut = new();

        //filter out the ones in front
        foreach (Cell cell in NeighbourCells)
        {
            var test = cell.GetComponent<Cell>().GetIndex();
            if (test.z > location.z)
            {
                FilteredOut.Add(cell);
            }
        }

        //enable buttons
        foreach (Cell cell in FilteredOut)
        {
            var test = cell.GetComponent<Cell>().GetIndex();
            if (test.x < location.x)
            {
                canLeft = true;
                leftCell = cell;
                continue;
            }

            if (test.z > location.z && test.x == location.x)
            {
                canRight = true;
                rightCell = cell;
                continue;
            }

        }

        ControlButtons();

        if (other.CompareTag("EnterTown"))
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FilteredOut.Clear();
        NeighbourCells.Clear();
        canLeft = false;
        canRight = false;
        ControlButtons();
    }

    private void OnDisable()
    {
        FilteredOut.Clear();
        NeighbourCells.Clear();
        canLeft = false;
        canRight = false;
    }
}
