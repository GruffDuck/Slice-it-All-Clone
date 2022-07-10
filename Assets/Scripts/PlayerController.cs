using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    private Vector3 knifeDir = ((Vector3.forward * 4) + (Vector3.down * 2)).normalized;
    bool canStuck = true;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        instance = this;
        rb.maxAngularVelocity = 10f;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rb.isKinematic = false;
            Jump();
            SpinForward();
        }
        //Býçaðýn yavaþlama noktasý
        float knifeAngle = Vector3.Angle(knifeDir, transform.forward);
        if (canStuck && knifeAngle < 35)
        {
            rb.maxAngularVelocity = 2f;
        }
        else rb.maxAngularVelocity = 10f;
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
            canStuck = false;
            rb.angularVelocity = Vector3.zero;
            rb.AddTorque(new Vector3(spinX, 0, 0), ForceMode.Acceleration);
            Invoke("Stuck", 0.4f);
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
            rb.AddTorque(new Vector3(-spinX, 0, 0), ForceMode.Acceleration);
        }
    }
    private void Stuck()
    {
        canStuck = true;
    }
}
