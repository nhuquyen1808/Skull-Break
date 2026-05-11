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

        [Header("Editor")]
        public Image coinICON;


        //UnityAction<string> actionOnClick;
        public string Key
        {
            get => key;
            set
            {
                key = value;
                transform.name = key;
            }
        }
        public int CoinReceive
        {
            get => coinReceive;
            set
            {
                coinReceive = value;
                txtCoin.text = (coinReceive).ToString();
            }
        } 

        private void OnEnable()
        {
            SetPriceText();
        }

        public void Init(string txtPrice, UnityAction<string> actionOnClick)
        {
            this.txtPrice.text = txtPrice;
            txtCoin.text = (coinReceive).ToString();
            //this.actionOnClick = actionOnClick;
        }

        void SetPriceText()
        {
            txtCoin.text = (coinReceive).ToString();
            txtPrice.text = IAPController.Instance.GetPriceValue(key);
        }
        public void OnClickItem()
        {
          // Debug.Log($"OnClickItem: name:{gameObject.name} key:{key}");
            //actionOnClick?.Invoke(key);
            IAPController.Instance.BuyProduct(key, (success) =>
            {
                if (success)
                {
                    OnSuccess();
                }
                else
                {
                    Debug.Log($"Buy Iap False: name:{gameObject.name} key:{key}");
                }
            });
           // AudioManager.instance.PlaySound("Click");
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

        public void UpdateICONCoin(Sprite icon)
        {
            coinICON.sprite = icon;
        }

        public void UpdateKeyItem(int id)
        {
            Key = IAPController.Instance.primaryKEY+"_pack_"+(id+1);;
            key = IAPController.Instance.primaryKEY+"_pack_"+(id+1);;
        }

    }

}
