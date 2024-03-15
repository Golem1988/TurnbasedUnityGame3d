using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using Cinemachine;
using System.Linq;

public class ControllableCharacter : MonoBehaviour
{
    public Transform player;
    public Animator animator;
    public float speed;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        var Cinemachine = GameManager.instance.PlayerFollowCamera.GetComponent<CinemachineVirtualCamera>();
        Cinemachine.m_LookAt = player;
        Cinemachine.m_Follow = player;
    }

    private void Start()
    {
        //string heroID = Extensions.FindMainCharacterID();
        string heroID = HeroDataManager.instance.CharacterInfo.FirstOrDefault(name => name.isMainCharacter)?.BaseID;
        GameObject heroModel = Extensions.FindModelPrefab(heroID, true);
        var go = Instantiate(heroModel, player.position, Quaternion.identity, player);
        animator = go.GetComponent<Animator>();
    }

    private void Update()
    {
        speed = agent.velocity.magnitude -1.2f;
        if (speed > -1)
            animator.SetFloat("Speed", speed);
        if (speed < 1.3)
            animator.SetFloat("Speed", 0);
    }

}
