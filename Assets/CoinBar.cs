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

    private void Start()
    {
        UpdateCoinText();
    }

    public void UpdateCoinText()
    {
        var coin = PlayerPrefs.GetFloat("coin");
        coinText.text = coin.ToString();
    }
}
