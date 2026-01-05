using System.Collections.Generic;
using UnityEngine;

namespace IAP_Dev
{
    public class ShopController : Singleton<ShopController>
    {
        [SerializeField] private GameObject shopUI;
        bool isOpen = false;
        public List<ItemCoin> listItemCoin = new List<ItemCoin>();
        public bool IsOpen { get => isOpen;}
        public void Show()
        {
            if(GameDataLoader.instance.disabledStatus) return;
            shopUI.gameObject.SetActive(true);
            isOpen = true;
        
        }

        public void Hide()
        {
            shopUI.gameObject.SetActive(false);
            isOpen = false;
        }
    }

}
