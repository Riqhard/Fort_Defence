using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    Transform cam;

    public void Awake()
    {
        cam = Camera.main.transform;
    }
    public void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
