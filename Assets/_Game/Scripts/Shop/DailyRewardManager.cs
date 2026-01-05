using System;
using System.Collections.Generic;
using DevDuck;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardManager : MonoBehaviour
{
    
    [SerializeField] List<DailyReward> listDailyRewards =new List<DailyReward>();
    public Button claimButton,closeButton;
    public GameObject nShadow;
    public EffectGetCoin effectGetCoin;
    [SerializeField] GameObject popupDailyReward;
    [SerializeField] float currrentRewardCoin;
    [SerializeField] int currentReward, previousDate,currentDate;
    private void Awake()
    {
        claimButton.onClick.AddListener(OnClickClaimButton);
        closeButton.onClick.AddListener(OnClickCloseButton);
    }

    private bool isFirstPlay;
    private void Start()
    {
        currentReward = PlayerPrefs.GetInt("currentReward");
        if (PlayerPrefs.GetInt("TheDaySaved") == 0)
        {
            PlayerPrefs.SetInt("TheDaySaved", DateTime.Now.Day);
            GetDataReward();
            Debug.Log("1111");
            isFirstPlay = true;
            popupDailyReward.SetActive(true);
        }
        currentDate = DateTime.Now.Day;
        if (currentDate != PlayerPrefs.GetInt("TheDaySaved"))
        {
            GetDataReward();
            Debug.Log("2222"); 
            popupDailyReward.SetActive(true);
        }
        else
        {
            if(isFirstPlay) return;
            Debug.Log("3333");
            claimButton.interactable = false;
            for (int i = 0; i < listDailyRewards.Count; i++)
            {
               if(listDailyRewards[i].ID <= currentReward-1)
                {
                    listDailyRewards[i].SetClaimed();
                }
                else
                {
                    listDailyRewards[i].SetLock();
                }
            }
        }
        
    }

    private void OnClickCloseButton()
    {
        nShadow.SetActive(false);
        popupDailyReward.transform.DOScale(0,0.3f).SetEase(Ease.InBack).OnComplete(() =>
        {
            popupDailyReward.SetActive(false);
        });
    }
    private void OnClickClaimButton()
    {
        effectGetCoin.GetCoin(currrentRewardCoin,10,null);
        listDailyRewards[currentReward].SetClaimed();
        claimButton.interactable = false;
        currentReward++;
        PlayerPrefs.SetInt("currentReward",currentReward);
        PlayerPrefs.SetInt("TheDaySaved", DateTime.Now.Day);
    }

    
    private void GetDataReward()
    {
        Debug.Log(currentReward);
        for (int i = 0; i < listDailyRewards.Count; i++)
        {
            if (listDailyRewards[i].ID == currentReward)
            {
                listDailyRewards[i].SetUnLock();
                currrentRewardCoin = listDailyRewards[i].amountCoin;
            }
            else if(listDailyRewards[i].ID < currentReward)
            {
                listDailyRewards[i].SetClaimed();
            }
            else
            {
                listDailyRewards[i].SetLock();
            }
        }
    }
    
}
