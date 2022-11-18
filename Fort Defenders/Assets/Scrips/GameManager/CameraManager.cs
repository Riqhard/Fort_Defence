using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineFreeLook freelook;
    bool holdingMouseDown = false;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            if (!holdingMouseDown)
            {
                freelook.m_XAxis.m_InputAxisName = "Mouse X";
                freelook.m_YAxis.m_InputAxisName = "Mouse Y";
                holdingMouseDown = true;
            }
        }

        if (Input.GetButtonUp("Fire2"))
        {
            if (holdingMouseDown)
            {
                freelook.m_XAxis.m_InputAxisName = "";
                freelook.m_YAxis.m_InputAxisName = "";
                freelook.m_XAxis.m_InputAxisValue = 0f;
                freelook.m_YAxis.m_InputAxisValue = 0f;
                holdingMouseDown = false;
            }
        }

    }
}
