using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : Singleton<PopupController>
{
    [SerializeField] private List<PopupBase> popups;
    public void ShowPopup(PopupType popupType)
    {
        popups.Find(p => p.PopupType == popupType).Show();
    }
}
