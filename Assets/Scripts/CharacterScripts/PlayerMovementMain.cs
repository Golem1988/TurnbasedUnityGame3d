using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementMain : MonoBehaviour
{
    public GameObject player;

    Vector3 curPos, lastPos;

    public long steps;
    public float stepSize = 1f;
    public float distanceTraveled = 0f;
    public float moveSpeed = 5f;
    private Vector2 movement;
    private Vector2 lastMove;
    private Vector3 target;
    private Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        transform.position = GameManager.instance.nextHeroPosition;
        target = transform.position;
    }

    void Update()
    {
        // Mouse input move
        /*
        if (Input.GetMouseButtonDown(1))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = transform.position.z;
        }

        transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

        */

        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Move();
    }

    void FixedUpdate()
    {

        curPos = transform.position;
        if (curPos == lastPos)
        {
            GameManager.instance.isWalking = false;
        }
        else
        {
            GameManager.instance.isWalking = true;
        }
        lastPos = curPos;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnterTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }

        if (other.tag == "LeaveTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }

        if (other.tag == "Region1")
        {
            GameManager.instance.curRegions = 0;
        }

        if (other.tag == "Region2")
        {
            GameManager.instance.curRegions = 1;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canGetEncounter = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canGetEncounter = false;
        }
    }

    public void Move()
    {
        anim.SetFloat("MoveX", movement.x);
        anim.SetFloat("MoveY", movement.y);
        if (movement != Vector2.zero)
        {
            anim.SetBool("isMoving", true);
            lastMove = movement;
            anim.SetFloat("LastMoveX", lastMove.x);
            anim.SetFloat("LastMoveY", lastMove.y);

            movement.Normalize();
            transform.position += (Vector3)movement * moveSpeed * Time.deltaTime;
            distanceTraveled += movement.magnitude * moveSpeed * Time.deltaTime;
            if (distanceTraveled >= stepSize)
            {
                steps++;
                distanceTraveled -= stepSize;
            }
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }
}
