using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class MovePlayerOnTrigger : MonoBehaviour
{
    [SerializeField] private float force = 10f; // Total launch force
    [SerializeField] private Vector3 moveDirection = new Vector3(1, 1, 0); // Launch direction
    [SerializeField] private float moveEnableDelay = 1.5f; // Time before player
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        if(!other.gameObject.TryGetComponent(out PlayerControler pc))
        {
            Debug.LogWarning("PlayerControler component not found on the player object.");
            return;
        }
        
        if (pc.rigidbody != null)
        {
            LaunchRigidbody(pc.rigidbody);
            pc.canMove = false;
            StartCoroutine(EnablePlayerMovementAfterDelay(pc));
        }
    }




    private void LaunchRigidbody(Rigidbody rb)
    {
        rb.velocity = Vector3.zero; // Reset velocity for consistency
        Vector3 launchForce = moveDirection.normalized * force;
        rb.AddForce(launchForce, ForceMode.Impulse); // Instant force application
    }
    
    private IEnumerator EnablePlayerMovementAfterDelay(PlayerControler playerpc)
    {
        yield return new WaitForSeconds(moveEnableDelay);
        playerpc.canMove = true;
    }
}