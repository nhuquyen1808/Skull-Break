using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupShop : MonoBehaviour
{

    public Image shadow;
    public GameObject nPopup;
    public Button closeButton;
    public static PopupShop instance;
    
    private void Awake()
    {
        instance = this;

        /*if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }*/
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    public void Show()
    {
        Debug.Log(shadow.gameObject);
        shadow.gameObject.SetActive(true); 
        nPopup.SetActive(true);
    }

    private void OnClickCloseButton()
    {
       nPopup.SetActive(false);
       shadow.gameObject.SetActive(false); 

    }
}
