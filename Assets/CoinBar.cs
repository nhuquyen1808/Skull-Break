using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    public Text coinText;

    public static CoinBar ins;

    private void Awake()
    {
        ins = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCoinText();
    }

    // Update is called once per frame
    public void UpdateCoinText()
    {
        var coin = DatabaseController.Instance.Coin; // Get the current coin value from the database
        coinText.text = $"{coin}";
    }
}
