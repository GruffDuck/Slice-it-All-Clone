using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeBackCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            PlayerController.instance.BackFlip();
        }
    }
}
