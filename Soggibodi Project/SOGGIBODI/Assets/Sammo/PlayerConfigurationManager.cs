using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerConfigurationManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> playerConfigs = new List<GameObject>();
    [SerializeField] public int currentPlayerIndex = 0;
    [SerializeField] private InputActionAsset inputActionAsset;
    [SerializeField] private GameObject playerPrefab;
    public static PlayerConfigurationManager instance { get; private set; }

    private void OnEnable()
    {

        if (!instance)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HandlePlayerJoined(PlayerInput input)
    {
        PlayerInformation player = input.GetComponent<PlayerInformation>();
        PlayerInfo playerInfo = player.playerInfo;

        PageController.instance.TurnPageOff(PageType.PressScreen);
        PageController.instance.TurnPageOn(PageType.Menu);
        input.transform.SetParent(transform);
        playerConfigs.Add(input.gameObject);
        input.gameObject.name = $"Player: {input.playerIndex + 1}";

    }

    public void AssignPlayerName(TMP_InputField input)
    {
        switch (input.gameObject.tag)
        {
            case "Player1":
                playerConfigs[0].GetComponent<PlayerInformation>().playerInfo.playerName = input.text;
                break;
            case "Player2":
                playerConfigs[1].GetComponent<PlayerInformation>().playerInfo.playerName = input.text;
                break;
        }
    }
    public void ChangePlayerEnabled()
    {
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            playerConfigs[i].GetComponent<PlayerInformation>().EnablePlayer();
        }
    }

    public void EnableSinglarPlayer(int index)
    {
        playerConfigs[index].GetComponent<PlayerInput>().ActivateInput();
    }

    public void DisableSinglarPlayer(int index)
    {
        playerConfigs[index].GetComponent<PlayerInput>().DeactivateInput();
    }

    public void ChangeActionMap(int index, string actionMap)
    {
        playerConfigs[index].GetComponent<PlayerInput>().SwitchCurrentActionMap(actionMap);
    }

    void UpdatePlayerColour(GameObject player, Color newColour)
    {
        player.GetComponent<PlayerInformation>().playerInfo.playerColour = newColour;
    }

}

