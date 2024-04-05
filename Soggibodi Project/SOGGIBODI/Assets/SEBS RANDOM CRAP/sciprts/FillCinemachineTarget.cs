using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class FillCinemachineTarget : MonoBehaviour
{
    CinemachineTargetGroup ctg;
    List<PlayerControls> playersInScene = new List<PlayerControls>();

    void Awake()
    {
        ctg = GetComponent<CinemachineTargetGroup>();
    }

    // Start is called before the first frame update
    public void FillMyPants()
    {
        playersInScene = FindObjectsOfType<PlayerControls>().ToList<PlayerControls>();
        ctg.m_Targets = new CinemachineTargetGroup.Target[playersInScene.Count];

        for(int i = 0; i < playersInScene.Count; ++i){
            ctg.m_Targets[i].target = playersInScene[i].GetHipsRB().gameObject.transform;
            ctg.m_Targets[i].weight = 1;
            ctg.m_Targets[i].radius = 10;
        }
    }
}
