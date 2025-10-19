using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinBar : MonoBehaviour
{
    public Text coinText;
    public static CoinBar instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateCoinText();
    }

    // Update is called once per frame
    public void UpdateCoinText()
    {
        coinText.text = DatabaseController.Instance.Coin.ToString();
    }
}
