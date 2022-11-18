using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody myRigidBody;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
    }

    public void Zoom(float zoomAmount) 
    {
        // min zoom = 10?
        // max zoom = 100?

    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void FixedUpdate()
    {
        myRigidBody.MovePosition(myRigidBody.position + velocity * Time.fixedDeltaTime);
    }

    public void LookAt(Vector3 lookpoint)
    {
        Vector3 heightCorrection = new Vector3(lookpoint.x, transform.position.y, lookpoint.z);
        transform.LookAt(heightCorrection);
    }
}
