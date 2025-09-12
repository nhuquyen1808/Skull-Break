using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    public Button CoinButton;
    public Text coinText;
    public static CoinBar instance;
  //  public PopupShop popupShop;

    private void Awake()
    {
        instance = this;
        CoinButton.onClick.RemoveAllListeners();
        CoinButton.onClick.AddListener(OnClickCoinButton);
        UpdateUI();
    }

    private void OnDestroy()
    {
        CoinButton.onClick.RemoveAllListeners();
    }

    private void OnClickCoinButton()
    {
        Debug.Log(PopupShop.instance);
        ShopController.Instance.Show();
    }


    public void UpdateUI()
    {
        int coin = PlayerPrefs.GetInt(DBKey.COIN);
        coinText.text = coin.ToString();
    }
}
