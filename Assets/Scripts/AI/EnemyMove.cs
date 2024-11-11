using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;


public class EnemyMove : MonoBehaviour
{
    public Transform player;
    public float move_sp;
    public Animator ani;
    public float playerRange;
    public LayerMask IsPlayer;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public bool distancetoplayer;
   
    private void Start()
    {
        distancetoplayer = Physics.CheckSphere(transform.position, playerRange, IsPlayer);
        if (distancetoplayer) AttackPlayer();


        
        ani = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        player = GameObject.FindWithTag("Player").transform ; 
        
        
        if (distancetoplayer) AttackPlayer();


        Vector3 newtarget = player.position;
        newtarget.y = transform.position.y;
       // newtarget.x = transform.position.x;
        transform.LookAt(newtarget);


        //transform.LookAt(player);
    transform.position = Vector3.MoveTowards(transform.position, player.position,move_sp * Time.deltaTime);

    }



    private void AttackPlayer()
    {

        

        if (!alreadyAttacked)
        {
            ///Attack code here

            ani.Play("slap");



            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
     
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }

}
