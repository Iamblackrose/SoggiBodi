using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PageController : MonoBehaviour
{

    public static PageController instance;
    public bool debug;
    //Page that can load up at the beginning of the game
    public PageType entryPage;
    //UI Pages that we create can be logged here in the manager for context
    public Page[] pages;

    private Hashtable m_Pages;
    #region Unity Functions
    private void Awake()
    {
        //singleton setting
        if (!instance)
        {
            instance = this;
            m_Pages = new Hashtable();
            RegisterAllPages();

            if (entryPage != PageType.None)
            {
                TurnPageOn(entryPage);
                //if(entryPage == PageType.MainScreenLoader)
                //{
                //    StartCoroutine("TurnEntryPageOff");
                //}
            }

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #endregion

    #region Public Functions
    public void TurnPageOn(PageType _type)
    {
        //Check the page type is not of none
        if (_type == PageType.None)
        {
            return;
        }
        //check for any errors
        if (!PageExists(_type))
        {
            LogWarning($"You are trying to turn the page on {_type} that has not been registered");
        }
        Page _page = GetPage(_type);
        _page.gameObject.SetActive(true);

        _page.Animate(true);
    }

    public void TurnPageOff(PageType _off, PageType _on = PageType.None, bool _waitForExit = false)
    {
        if (_off == PageType.None)
        {
            return;
        }
        if (!PageExists(_off))
        {
            LogWarning($"You are trying to turn the page off {_off} that has not been registered");
        }

        Page _offPage = GetPage(_off);
        if (_offPage.gameObject.activeSelf)
        {
            _offPage.Animate(false);
        }

        if (_on != PageType.None)
        {
            Page _onPage = GetPage(_on);
            if (_waitForExit)
            {
                //StopCoroutine("WaitForPageExit"); Can Cause some issues with functionality taken out but left as legacy
                StartCoroutine(WaitForPageExit(_onPage, _offPage));
            }
            else
            {
                TurnPageOn(_on);
            }
        }
    }

    public bool PageIsOn(PageType _type)
    {
        if (!PageExists(_type))
        {
            LogWarning($"You are tring to detect if a page is on {_type}, but it has not been registered");
            return false;
        }

        return GetPage(_type).isOn;
    }

    #endregion

    #region Private Functions
    private IEnumerator WaitForPageExit(Page _on, Page _off)
    {
        while (_off.targetState != Page.FLAG_NONE)
        {
            yield return null;
        }

        TurnPageOn(_on.type);
    }
    //Register all pages in the current context
    private void RegisterAllPages()
    {
        foreach (Page _page in pages)
        {
            RegisterPage(_page);
        }
    }
    //Single register Pages
    private void RegisterPage(Page _page)
    {
        if (PageExists(_page.type))
        {
            LogWarning($"Page type {_page.type} has already been registered: {_page.gameObject.name}");
        }

        m_Pages.Add(_page.type, _page);
        Log($"Registered new page {_page.type}");
    }

    private Page GetPage(PageType _type)
    {
        if (!PageExists(_type))
        {
            LogWarning($"You are trying to get a page {_type} that has not been registered");
            return null;
        }

        return (Page)m_Pages[_type];
    }

    private bool PageExists(PageType _type)
    {
        return m_Pages.ContainsKey(_type);
    }

    private void Log(string _msg)
    {
        if (!debug)
        {
            return;
        }
        Debug.Log($"[{this.name}]: {_msg}");
    }

    private void LogWarning(string _msg)
    {
        if (!debug)
        {
            return;
        }
        Debug.LogWarning($"[{this.name}]: {_msg}");
    }

    #endregion
}