using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Handle")|| other.gameObject.CompareTag("SharpEdge"))
        {
            other.GetComponentInParent<Rigidbody>().isKinematic = true; 
            other.transform.parent.GetComponentInParent<PlayerController>().enabled = false;
            other.transform.parent.GetComponentInParent<Rigidbody>().isKinematic = true;
            Manager.Instance.deathPanel.SetActive(true);
        }
    }
}
