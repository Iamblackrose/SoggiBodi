using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cinemachine;
using UnityEngine;

// This dogshit ass script, can be moved to a global gamemanager script that gets game scene info from a seperate stored one in map.
public class GameManager : MonoBehaviour
{
    public float TimeLeft { get; private set; }

    public List<PlayerControls> listOfPlayer = new List<PlayerControls>(); 

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Scene Camera Reference")]
    [SerializeField] private CinemachineTargetGroup cameraTargetGroup;

    [SerializeField] FillCinemachineTarget fct;

    public void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        listOfPlayer = FindObjectsByType<PlayerControls>(FindObjectsSortMode.None).ToList();
        if (spawnPoints.Count < PlayerConfigurationManager.instance.playerConfigs.Count)
        {
            Debug.Log("Too many players for assigned spawnpoints, having to repeat.");
        }

        for (int i = 0; i < PlayerConfigurationManager.instance.playerConfigs.Count; i++)
        {
            // Probs Easier way to do it, but actual player object is 100% at index 0.
            GameObject player = PlayerConfigurationManager.instance.playerConfigs[i].transform.GetChild(0).gameObject;
            Debug.Log(PlayerConfigurationManager.instance.playerConfigs[i].transform.GetChild(0).gameObject.name);


            // If you want to add more players / spawn points will reuse them is case.
            player.transform.position = spawnPoints[i % spawnPoints.Count].position;

            cameraTargetGroup?.m_Targets.Append(new CinemachineTargetGroup.Target { target = player.transform, weight = 1f, radius = 0.25f });

            player.SetActive(true);
        }

        fct.FillMyPants();
    }
}