using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    //public PageController pageController;
    private bool isFaded = false;

    [SerializeField] private float duration = 2f;

    public AudioSource soundtrack;


    private void Start()
    {
        AudioController.instance.PlayAudio(AudioType.ST_01);
    }
    private void Update()
    {

    }

    private void LateUpdate()
    {
    }
    //public void PlayMenuScroll()
    //{
    //    AudioController.instance.PlayAudio(Audio.AudioType.SFX_MenuScrollClick);
    //}

    public void LoadSingleGame()
    {
        SceneController.instance.Load(SceneType.Dungeon, (_scene) =>
        {
            //TurnPageOnPageType(PageType.DraftAndPlacementMaster); battle ui
            TurnPageOffPageType(PageType.Loading);
            TurnPageOffPageType(PageType.Credits);
        }, false, PageType.Loading);
        //AudioController.instance.PlayAudio(Audio.AudioType.SFX_01);
        for (int i = 0; i < PlayerConfigurationManager.instance.playerConfigs.Count; i++)
        {
            PlayerConfigurationManager.instance.playerConfigs[i].GetComponent<PlayerInput>().SwitchCurrentActionMap("Knight");
            Debug.Log(PlayerConfigurationManager.instance.playerConfigs[i].GetComponent<PlayerInput>().currentActionMap);
        }
            TurnPageOnPageType(PageType.Health);
    }

    //public void LoadMainMenu()
    //{
    //    SceneController.instance.Load(SceneType.MainMenu, (_scene) =>
    //    {
    //        TurnPageOnPageType(PageType.MainMenuMaster);
    //        TurnPageOnPageType(PageType.Menu);
    //        TurnOffList(new List<PageType> { PageType.EndScreen, PageType.BattleMaster });
    //    }, false, PageType.Loading);
    //    AudioController.instance.PlayAudio(Audio.AudioType.SFX_01);
    //}
    public void LoadURL(string URL)
    {
        Application.OpenURL(URL);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Fader(CanvasGroup group)
    {
        isFaded = !isFaded;

        if (isFaded)
        {
            group.DOFade(0f, duration);
            group.interactable = false;
            group.blocksRaycasts = false;
        }
        else
        {
            group.DOFade(1f, duration);
            group.interactable = true;
            group.blocksRaycasts = true;

        }
    }

    public void TurnPageOn(Page _page)
    {
        PageController.instance.TurnPageOn(_page.type);
        AudioController.instance.PlayAudio(AudioType.SFX_01);
    }
    public void TurnPageOff(Page _page)
    {
        PageController.instance.TurnPageOff(_page.type);
    }
    public void TurnPageOffPageType(PageType _pagetype)
    {
        PageController.instance.TurnPageOff(_pagetype);
    }
    public void TurnPageOnPageType(PageType _pagetype)
    {
        PageController.instance.TurnPageOn(_pagetype);
    }
}