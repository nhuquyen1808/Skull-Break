using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace  IAP_Dev
{
    public class ItemCoin : MonoBehaviour
    {
        [SerializeField] private string key;
        [SerializeField] private int coinReceive;
        [SerializeField] private Text txtPrice;
        [SerializeField] private Text txtCoin;
        //UnityAction<string> actionOnClick;

        public string Key => key;
        public int CoinReceive => coinReceive;

        private void OnEnable()
        {
            SetPriceText();
        }

        public void Init(string txtPrice, UnityAction<string> actionOnClick)
        {
            this.txtPrice.text = txtPrice;
            txtCoin.text = coinReceive.ToString();
            //this.actionOnClick = actionOnClick;
        }

        void SetPriceText()
        {
            txtCoin.text = coinReceive.ToString();
            txtPrice.text = IAPController.Instance.GetPriceValue(key);
        }
        public void OnClickItem()
        {
            Debug.Log($"OnClickItem: name:{gameObject.name} key:{key}");
            //actionOnClick?.Invoke(key);
            IAPController.Instance.BuyProduct(key, (success) =>
            {
                if (success)
                {
                    Debug.Log($"OnSuccess: name:{gameObject.name} key:{key}");
                    //UIInteract.ins.UpdateCoin(coinReceive);
                    OnSuccess();
                }
                else
                {
                    Debug.Log($"Buy Iap False: name:{gameObject.name} key:{key}");
                }
            });
        }
        public void OnSuccess()
        {
            Debug.Log($"OnSuccess: name:{gameObject.name} key:{key}");
            var coin =  PlayerPrefs.GetFloat("coin");
            coin += coinReceive;
            PlayerPrefs.SetFloat("coin", coin);
            CoinBar.instance.UpdateCoinText();
            // AudioManager.instance.PlaySound("Cash");
        }
    }

}
