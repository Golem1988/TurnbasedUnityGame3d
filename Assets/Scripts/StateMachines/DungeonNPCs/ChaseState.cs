using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;
    int encounterRange;
    DungeonNPC DungeonNPC;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DungeonNPC = animator.GetComponentInParent<DungeonNPC>();
        agent = animator.GetComponentInParent<NavMeshAgent>();
        encounterRange = DungeonNPC.encounterRange;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = DungeonNPC.chaseSpeed;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if ((int)distance < encounterRange)
        {
            agent.SetDestination(animator.transform.position);
            GameManager.instance.Chat.AddToChatOutput("You got attacked");
            animator.SetBool("isChasing", false);
            animator.SetBool("standStill", true);
            //DungeonNPC.StartBattle();
            DungeonNPC.gameObject.SetActive(false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
