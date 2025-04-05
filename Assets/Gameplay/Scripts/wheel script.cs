using UnityEngine;
using System.Collections;

public class MovePlayerOnTrigger : MonoBehaviour
{
    public float Force = 10f; // Total launch force
    public Vector3 moveDirection = new Vector3(1, 1, 0); // Launch direction
    public bool resetVelocityOnLaunch = true; // Ensures a consistent launch each time
    public float moveEnableDelay = 1.5f; // Time before player
    private void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Player"))
        {
           Rigidbody playerRb = other.GetComponent<Rigidbody>();
           PlayerControler playerpc = other.GetComponent<PlayerControler>();

            if (playerRb != null)
            {
                LaunchPlayer(playerRb);
                playerpc.canMove = false;
                StartCoroutine(EnablePlayerMovementAfterDelay(playerpc));
            }
        }
    }




    private void LaunchPlayer(Rigidbody playerRb)
    {
        if (resetVelocityOnLaunch)
            playerRb.velocity = Vector3.zero; // Reset velocity for consistency

        Vector3 launchForce = moveDirection.normalized * Force;
        playerRb.AddForce(launchForce, ForceMode.Impulse); // Instant force application
    }
    
    private IEnumerator EnablePlayerMovementAfterDelay(PlayerControler playerpc)
    {
        yield return new WaitForSeconds(moveEnableDelay);
        playerpc.canMove = true;
    }
}