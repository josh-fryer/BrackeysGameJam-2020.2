using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehavior : StateMachineBehaviour
{
    private float mySpeed = 0;
    private Transform playerPos;
    GameObject player;

    Vector3 circleCentre = new Vector3(0, 0, 0);

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform;
       
    }

    //onstateupdate is called on each update frame between onstateenter and onstateexit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {
        mySpeed = animator.GetFloat("moveSpeed");
        //Debug.Log("mySpeed = "+ mySpeed);
        if (player == null)
        {
            animator.SetBool("isFollowing", false);
        }
        else
        {   
            // call orc follow method using rigidbody.     
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, playerPos.position, mySpeed * Time.deltaTime);
        }
    }

    //onstateexit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateinfo, int layerindex)
    {

    }
}
