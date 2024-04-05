using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerModals : MonoBehaviour
{
    public int playerIndex = 0;

    public TextMeshProUGUI textMeshProUGUI;

    public Color currentColour;
    public GameObject lWeapon;
    public GameObject rWeapon;

    public List<Image> sprites = new();
    public List<Sprite> wepSprites = new();

    public PlayerChoices playerChoices;

    public int colourIndex = 0;
    public int lWepIdx = 0;
    public int rWepIdx = 0;

    [SerializeField] GameObject button;

    private void OnEnable()
    {
        playerChoices = FindObjectOfType<PlayerChoices>();
        sprites[0].color = playerChoices.colors[colourIndex];
        PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.playerColour = playerChoices.colors[colourIndex];

        sprites[1].sprite = wepSprites[lWepIdx];

        sprites[2].sprite = wepSprites[rWepIdx];

        PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.es.SetSelectedGameObject(button);

    }

    private void Start()
    {
        if (PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.healthBar == null)
        {
            PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.healthBar = playerChoices.healthUI[playerIndex];
        }
    }

    public void ChangeColourSelection(int changeValue)
    {
        colourIndex += changeValue;

        if (PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.healthBar == null)
        {
            PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.healthBar = playerChoices.healthUI[playerIndex];
        }


        if (colourIndex < 0)
        {
            colourIndex = playerChoices.colors.Count - 1;
        }
        else if (colourIndex >= playerChoices.colors.Count)
        {
            colourIndex = 0;
        }
        currentColour = playerChoices.colors[colourIndex];
        sprites[0].color = playerChoices.colors[colourIndex];
        PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.playerColour = currentColour;

        PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.healthBar.color = currentColour;
        AudioController.instance.PlayAudio(AudioType.SFX_01);

    }

    public void ChangeLWeaponSelection(int changeValue)
    {
        lWepIdx += changeValue;

        if (lWepIdx < 0)
        {
            lWepIdx = playerChoices.weapons.Count - 1;
        }
        else if (lWepIdx >= playerChoices.weapons.Count)
        {
            lWepIdx = 0;
        }

        sprites[1].sprite = wepSprites[lWepIdx];
        PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.lWeapon = playerChoices.weapons[lWepIdx];
        AudioController.instance.PlayAudio(AudioType.SFX_01);

    }
    public void ChangeRWeaponSelection(int changeValue)
    {
        rWepIdx += changeValue;

        if (rWepIdx < 0)
        {
            rWepIdx = playerChoices.weapons.Count - 1;
        }
        else if (rWepIdx >= playerChoices.weapons.Count)
        {
            rWepIdx = 0;
        }

        sprites[2].sprite = wepSprites[rWepIdx];
        PlayerConfigurationManager.instance.playerConfigs[playerIndex].GetComponent<PlayerInformation>().playerInfo.playerControls.rWeapon = playerChoices.weapons[rWepIdx];
        AudioController.instance.PlayAudio(AudioType.SFX_01);

    }
}
