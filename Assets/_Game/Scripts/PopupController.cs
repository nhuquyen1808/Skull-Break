using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BreakBlock
{
    public class PopupControllerNew : Singleton<PopupControllerNew>
    {
        [SerializeField] private List<PopupBase> popups;
        public void ShowPopup(PopupType popupType)
        {
            popups.Find(p => p.PopupType == popupType).Show();
        }
    }

}
