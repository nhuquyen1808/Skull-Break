using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace IAP_Dev
{
    public class ShopController : Singleton<ShopController>
    {
        [SerializeField] private GameObject shopUI;
        bool isOpen = false;

        public CanvasGroup popupShop;


        public List<ItemCoin> listItemCoin = new List<ItemCoin>();
        public bool IsOpen { get => isOpen;}
        public void Show()
        {
            if(GameDataLoader.instance.disabledStatus) return;
            shopUI.gameObject.SetActive(true);
            popupShop.alpha = 0;
            popupShop.DOFade(1f, 0.3f).SetEase(Ease.OutBack);
            isOpen = true;
        
        }

        public void Hide()
        {
            popupShop.alpha = 1;
            popupShop.DOFade(0f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                shopUI.gameObject.SetActive(false);
            });
            isOpen = false;
        }


        [Header("Editor")]
        public List<Sprite> lstIconCoin;
        public void UpdateItemCoin()
        {
            for (int i = 0; i < IAPController.Instance.amount; i++)
            {

                listItemCoin[i].UpdateKeyItem(i);
                listItemCoin[i].coinICON.sprite = lstIconCoin[i];
            }
        }

        public List<int> lstPriceValue = new List<int>();

        public void SetPriceValue()
        {
            for (int i = 0; i < IAPController.Instance.amount; i++)
            {
                listItemCoin[i].CoinReceive = lstPriceValue[i];
            }
        }
    }

}
