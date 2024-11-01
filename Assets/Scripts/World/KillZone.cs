using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] GameObject CentrePoint;
    [SerializeField] float timeBeforeComingOut;
    [SerializeField] float timeBeforeReenableMovement;
    bool shootup;
    public int forceUp;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player" && shootup == false)
        {
            shootup = true;
            StartCoroutine(ShootBackToDeck(col.gameObject));

        }
    }

    IEnumerator ShootBackToDeck(GameObject player)
    {
        yield return new WaitForSeconds(1);
        player.GetComponent<PlayerControler>().DisableMovment();
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(timeBeforeComingOut);
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(0, forceUp,0);
        yield return new WaitForSeconds(3);
        shootup = false;
        rb.MovePosition(new Vector3(CentrePoint.transform.position.x, player.transform.position.y, CentrePoint.transform.position.z));
        player.transform.position = new Vector3(CentrePoint.transform.position.x, player.transform.position.y ,CentrePoint.transform.position.z);
        yield return new WaitForSeconds(timeBeforeReenableMovement);
        player.GetComponent<PlayerControler>().EnableMovement();
    }
}
