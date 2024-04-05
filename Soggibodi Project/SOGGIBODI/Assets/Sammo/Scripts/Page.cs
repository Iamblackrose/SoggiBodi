using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class Page : MonoBehaviour
{
    public static readonly string FLAG_ON = "On";
    public static readonly string FLAG_OFF = "Off";
    public static readonly string FLAG_NONE = "None";

    public PageType type;
    public bool debug;
    //define if the page uses an animation
    public bool useAnimation;
    public string targetState { get; private set; }
    //List of player eventSystems for button selection
    public Button startingButton;
    private Animator m_Animator;
    private bool m_IsOn;

    public bool isOn
    {
        get
        {
            return m_IsOn;
        }
        private set
        {
            m_IsOn = value;
        }
    }

    #region Unity Functions
    private void OnEnable()
    {
        foreach(EventSystem es in FindObjectsByType<EventSystem>(FindObjectsSortMode.None))
        {
            SetActiveButtonOnEnable(startingButton, es);
        }
    }

    private void Start()
    {
    }
    #endregion

    #region Public Functions
    //Animate if turning on maybe use similar function for DG.tween
    public void Animate(bool _on)
    {
        if (useAnimation)
        {
            m_Animator.SetBool("on", _on);

            StopCoroutine("AwaitAnimation");
            StartCoroutine("AwaitAnimation", _on);
        }
        else
        {
            if (!_on)
            {
                gameObject.SetActive(false);
                isOn = false;
            }
            else
            {
                isOn = true;
            }
        }
    }

    public void SetActiveButtonOnEnable(Button button, EventSystem es)
    {
        if (startingButton != null)
        {
            es.SetSelectedGameObject(button.gameObject);
        }
    }

    #endregion

    #region Private Functions

    private IEnumerator AwaitAnimation(bool _on)
    {
        targetState = _on ? FLAG_ON : FLAG_OFF;

        //Wait for the animator to reach the target state
        while (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName(targetState))  //Gets the current state on layer 0 of the animator and checks the target state
        {
            yield return null;
        }

        //Wait for the animator to finish animating
        while (m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)    //normalized time at 1 means end of the clip
        {
            yield return null;
        }

        targetState = FLAG_NONE;

        Log($"Page [{type}] finished transitioning to " + (_on ? "on" : "off"));

        if (!_on)
        {
            isOn = false;
            gameObject.SetActive(false);
        }
        else
        {
            isOn = true;
        }
    }

    private void CheckAnimatorIntegrity()
    {
        if (useAnimation)
        {
            m_Animator = GetComponent<Animator>();
            if (!m_Animator)
            {
                LogWarning($"You wanted to animate a page {type} but no Animator exists on the object");
            }
        }
    }

    private void Log(string _msg)
    {
        if (!debug)
        {
            return;
        }
        Debug.Log($"[Page]: {_msg}");
    }

    private void LogWarning(string _msg)
    {
        if (!debug)
        {
            return;
        }
        Debug.LogWarning($"[Page]: {_msg}");
    }

    #endregion
}