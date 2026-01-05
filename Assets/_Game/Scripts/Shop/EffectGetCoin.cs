using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DevDuck
{
    public class EffectGetCoin : MonoBehaviour
    {
        [SerializeField] GameObject coinParent;
        public Vector3[] initialPos;
        public Quaternion[] initialRotation;
        public GameObject startPosition, destination;
        int coinNum = 10;
        public Vector3 des, originPos;
        public Text coinText;

        void Start()
        {
            SetStartPosition();
        }
        private void SetStartPosition()
        {
            initialPos = new Vector3[coinNum];
            initialRotation = new Quaternion[coinNum];
            originPos = startPosition.transform.position;
            des = destination.transform.position;
            coinParent.transform.position = originPos;
            for (int i = 0; i < coinParent.transform.childCount; i++)
            {
                initialPos[i] = coinParent.transform.GetChild(i).position;
                initialRotation[i] = coinParent.transform.GetChild(i).rotation;
            }
        }

        private void Reset()
        {
            for (int i = 0; i < coinParent.transform.childCount; i++)
            {
                coinParent.transform.GetChild(i).position = initialPos[i];
                coinParent.transform.GetChild(i).rotation = initialRotation[i];
            }
        }

        public void GetCoin(float coinGet, int amountImageShow ,Action action)
        {
            Reset();
            var delay = 0f;
          //  coinParent.SetActive(true);
            des = destination.transform.position;
            int count = 0;
            int testCoin = 0;

            for (int i = 0; i < coinParent.transform.childCount; i++)
            {
                if (amountImageShow >= 10)
                {
                    var a = i;
                    coinParent.transform.GetChild(a).DOScale(1, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                    coinParent.transform.GetChild(a).GetComponent<RectTransform>().DOMove(des, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.InBack).OnComplete((
                        () =>
                        {
                            coinParent.transform.GetChild(a).DOScale(0, 0.1f).SetEase(Ease.InBack).OnComplete((() =>
                            {
                                count++;
                                ShowCoinText(coinGet, amountImageShow);
                                if (count ==coinNum)
                                {
                                    action?.Invoke();
                                }
                            }));
                        }));
                    delay += 0.1f;
                }
                else
                {
                    // use for coin get below 10 
                    if (i < amountImageShow)
                    {
                        var a = i;
                        coinParent.transform.GetChild(a).DOScale(1, 0.3f).SetDelay(delay).SetEase(Ease.OutBack);
                        coinParent.transform.GetChild(a).GetComponent<RectTransform>().DOMove(des, 0.5f).SetDelay(delay + 0.5f).SetEase(Ease.InBack).OnComplete((
                            () =>
                            {
                                coinParent.transform.GetChild(a).DOScale(0, 0.1f).SetEase(Ease.InBack).OnComplete((() =>
                                {
                                    count++;
                                    ShowCoinText(coinGet, amountImageShow);
                                    if (count == amountImageShow)
                                    {
                                        action?.Invoke();
                                    }
                                }));
                            }));
                        delay += 0.1f;
                    }
                }
            }
        }
        private void ShowCoinText(float coinGet, int amountImageShow)
        {
            PlayerPrefs.SetFloat("coin", PlayerPrefs.GetFloat("coin") + coinGet/amountImageShow);
            coinText.text = Mathf.Round(PlayerPrefs.GetFloat("coin")).ToString();
        }
    }
}
