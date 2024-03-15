using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNPC : MonoBehaviour
{
    Animator animator;
    public GameObject patrolArea;
    [Range(1, 10)] public int chaseRange;
    [Range(1, 10)] public int patrolSpeed;
    [Range(1, 10)] public int chaseSpeed;
    [Range(1, 10)] public int encounterRange;
    //[SerializeField] ScriptableObject battleEnemies;
    //[SerializeField] dialogue;
    public int regionId = 1;
    [SerializeField] GameObject sphere;

    private void OnValidate()
    {
        sphere.transform.localScale = new Vector3(chaseRange * 2, chaseRange * 2, chaseRange * 2);
    }

    private void Awake()
    {
        sphere.transform.localScale = new Vector3(chaseRange * 2, chaseRange * 2, chaseRange * 2);
        animator = transform.GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator.SetBool("isDungeonNPC", true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().curRegions = regionId;
        GameObject.Find("GameManager").GetComponent<GameManager>().gameState = GameStates.IDLE;
        GameObject.Find("GameManager").GetComponent<GameManager>().StartBattle();
    }


    private void OnDisable()
    {
        animator.SetBool("isDungeonNPC", false);
    }
}
