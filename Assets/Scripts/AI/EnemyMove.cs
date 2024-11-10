using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;


public class EnemyMove : MonoBehaviour
{
    public Transform player;
    public float move_sp;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform ; 
    }

    private void FixedUpdate()
    {
     
        transform.LookAt(player);
    transform.position = Vector3.MoveTowards(transform.position, player.position,move_sp * Time.deltaTime);

    }



    private void AttackPlayer()
    {

        

        if (!alreadyAttacked)
        {
            ///Attack code here
        




            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

}
