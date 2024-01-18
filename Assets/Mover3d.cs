using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover3d : MonoBehaviour
{
    public Transform target;
    
    public float moveForce = 1f;
    public float rotForce = 1f;
    public float rotThreshold = 0.25f;
    private void FixedUpdate()
    {
        var v = target.position - transform.position;
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(v * moveForce, ForceMode.VelocityChange);
        
        
        var r = Vector3.Angle(target.up, transform.up) * Mathf.Sign(Vector3.Dot(transform.right, -target.up));
        if (Mathf.Abs(r) < rotThreshold) return;
        rb.AddTorque(new Vector3(0, 0, r * rotForce), ForceMode.VelocityChange);

    }
}
