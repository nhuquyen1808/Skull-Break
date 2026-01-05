using System;
using TMPro;
using UnityEngine;

public class DailyReward : IDComponent
{
    public GameObject glow;
    public GameObject claimedObject;
    public TextMeshProUGUI amountCoinText;
    public float amountCoin;

  
    private void Start()
    {
        amountCoinText.text = amountCoin.ToString();
    }

    public void SetClaimed()
    {
        glow.SetActive(false);
        claimedObject.gameObject.SetActive(true);
    }

    public void SetLock()
    {
        glow.SetActive(false);

    }
    public void SetUnLock()
    {
        glow.SetActive(true);
        claimedObject.gameObject.SetActive(false);
    }
}
