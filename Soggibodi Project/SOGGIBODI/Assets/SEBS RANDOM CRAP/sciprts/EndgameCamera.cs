using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EndgameCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] float camSpeed;

    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        var dolly = vcam.GetCinemachineComponent<CinemachineTrackedDolly>();
        if(dolly){
            dolly.m_PathPosition += Time.deltaTime * camSpeed;
        }
    }
}
