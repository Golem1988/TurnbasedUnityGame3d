using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MapTileGridCreator.Core;
using System;
using System.Linq;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    public Vector3Int location;
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
    [SerializeField]
    GameObject buttonHolder;
    [SerializeField]
    GameObject recapWindow;
    [SerializeField]
    GameObject lootWindow;
    [SerializeField]
    GameObject healWindow;


    public Vector3 dest;

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

    private void OnEnable()
    {
        Actions.OnDungeonChestTrigger += ShowChestUI;
        Actions.OnDungeonHealTrigger += ShowHealUI;
    }

    private void ShowHealUI()
    {
        buttonHolder.SetActive(false);
        healWindow.SetActive(true);
    }

    private void ShowChestUI()
    {
        buttonHolder.SetActive(false);
        lootWindow.SetActive(true);
    }

    private void OnDisable()
    {
        FilteredOut.Clear();
        NeighbourCells.Clear();
        canLeft = false;
        canRight = false;
        Actions.OnDungeonChestTrigger -= ShowChestUI;
        Actions.OnDungeonHealTrigger -= ShowHealUI;
    }


    void Start()
    {
        var mainHeroID = HeroDataManager.instance.CharacterInfo.FirstOrDefault(hero => hero.isMainCharacter).BaseID;
        GameObject heroModel = Extensions.FindModelPrefab(mainHeroID, true);
        GameObject myAv = Instantiate(heroModel, modelHolder.position, Quaternion.Euler(0, 0, 0), modelHolder);

        rBody = GetComponent<Rigidbody>();
        Vector3 dest = _grid.GetPositionCell(location) + new Vector3 (0, 0.5f, 0);
        rBody.MovePosition(dest);
        Vector3 zero = new (0f, 0f, 0f);
        if (GameManager.instance.nextHeroPosition != zero)
        {
            dest = GameManager.instance.nextHeroPosition;
            rBody.MovePosition(dest);
        }
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

        if (scenario.isBoss && scenario.Data.CellData.isCleared)
        {
            recapWindow.SetActive(true);
        }

        if (scenario.Scenario && !scenario.Data.CellData.isCleared)
        {
            if (scenario.Model)
                Destroy(scenario.Model);
            scenario.Data.CellData.isCleared = true;
            DungeonManager.instance.UpdateData(scenario.Data.CellData);
            scenario.Scenario.StepOn();
        }

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

    }

    private void OnTriggerExit(Collider other)
    {
        FilteredOut.Clear();
        NeighbourCells.Clear();
        canLeft = false;
        canRight = false;
        ControlButtons();
    }


}
