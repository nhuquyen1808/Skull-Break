using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    public static CoinBar instance;
    public Text coinText;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        var coin = DatabaseController.Instance.Coin; // Get the current coin value from the database
        coinText.text = $"{coin}";
    }
}
