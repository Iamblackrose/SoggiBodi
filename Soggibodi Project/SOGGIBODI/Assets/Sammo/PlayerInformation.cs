using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;
using Unity.VisualScripting;

[System.Serializable]
public class PlayerInfo
{
    public string playerName = "Player";
    public Color playerColour = Color.white;
    public GameObject playerObject;
    public PlayerControls playerControls;
    public EventSystem es;

}

public class PlayerInformation : MonoBehaviour
{
    public PlayerInfo playerInfo = new PlayerInfo();

    private void OnEnable()
    {
        playerInfo.playerName = gameObject.name;
        if (gameObject.GetComponent<PlayerInput>().playerIndex >= 1)
        {
            //playerInfo.playerColour = random;
            Debug.Log(playerInfo.playerName);
            //PlayerConfigurationManager.instance.DisableSinglarPlayer(gameObject.GetComponent<PlayerInput>().playerIndex);
        }
        else
        {
            //SetActiveButtonOnEnable(GetComponentInChildren<EventSystem>());
            //PageController.instance.TurnPageOff(PageType.PressScreen);
            //PageController.instance.TurnPageOn(PageType.Menu);
        }
    }

    public void EnablePlayer()
    {
        switch (gameObject.GetComponent<PlayerInput>().inputIsActive)
        {
            case true:
                //Disable Input
                gameObject.GetComponent<PlayerInput>().DeactivateInput();
                break;
            case false:
                //Enable Input
                gameObject.GetComponent<PlayerInput>().ActivateInput();
                break;
        }
    }

    public void SetActiveButtonOnEnable( EventSystem es)
    {
        Page p =  FindAnyObjectByType<Page>();
        if (p.startingButton != null)
        {
            es.SetSelectedGameObject(p.startingButton.gameObject);
        }
    }

}
