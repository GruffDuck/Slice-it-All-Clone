using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharpCheck : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerController.instance.isGround == false)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                PlayerController.instance.isGround = true;
                transform.parent.GetComponentInParent<Rigidbody>().isKinematic = true;

            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            PlayerController.instance.isGround = true;

        }
    }
    private void OnTriggerExit(Collider other)
    {

        StartCoroutine(groundFalse());
    }
    private IEnumerator groundFalse()
    {
        yield return new WaitForSeconds(0.3f);
        PlayerController.instance.isGround = false;
    }
}
