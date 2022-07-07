using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    Rigidbody rb;
    [Header("Bool Var")]
    public bool isGround;
    [Header("Jump Variables")]
    public float jumpY;
    public float jumpZ;
    [Header("Spin Variables")]
    public float spinX;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        instance = this;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.isKinematic = false;
            Jump();
            SpinForward();
        }
    }
    private void FixedUpdate()
    {
        rb.inertiaTensorRotation = Quaternion.identity;
    }
    public void BackFlip()
    {
        Jump(false);
        SpinForward(false);
    }
    public void Jump(bool forward = true)
    {
        if (forward)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0, jumpY, jumpZ), ForceMode.Impulse);
        }
        else
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0, jumpY / 2, -jumpZ), ForceMode.Impulse);
        }
    }
    public void SpinForward(bool forward = true)
    {
        if (forward)
        {
            rb.angularVelocity = Vector3.zero;
            rb.AddTorque(new Vector3(spinX, 0, 0), ForceMode.Acceleration);
         
            
            
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
            rb.AddTorque(new Vector3(-spinX, 0, 0), ForceMode.Acceleration);
        }
    }
    
}
