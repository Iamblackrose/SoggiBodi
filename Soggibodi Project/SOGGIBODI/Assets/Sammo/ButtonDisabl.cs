using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabl : MonoBehaviour
{
    public Button button;

    void OnEnable()
    {
        StartCoroutine(DisableButtonForSeconds(15f));
    }

    IEnumerator DisableButtonForSeconds(float seconds)
    {
        button.interactable = false;

        yield return new WaitForSeconds(seconds);

        button.interactable = true;
    }
}
