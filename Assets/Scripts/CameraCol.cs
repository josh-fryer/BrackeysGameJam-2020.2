using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraCol : MonoBehaviour
{

    public CinemachineVirtualCamera vCam;

    // Start is called before the first frame update
    void Start()
    {
        vCam.enabled = false;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !other.isTrigger)
        {
            vCam.enabled = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            vCam.enabled = false;
        }
    }

}
