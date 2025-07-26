using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopupType
{
    None,
    Revive,
    Lose,
    Win,
    LevelComplete,
    LevelFail
}
public class PopupBase : MonoBehaviour
{
    [SerializeField] private Image imgFade;
    [SerializeField] private Image content;
    [SerializeField] private PopupType popupType;
    [SerializeField] private float showduration = 0.5f;
    [SerializeField] private float fadeValue = 0.8f;

    public PopupType PopupType { get => popupType; }

    public virtual async UniTask SetupBeforeShow()
    {
    }
    public virtual async UniTask Show()
    {
        await SetupBeforeShow();
        content.gameObject.SetActive(true);
        imgFade.gameObject.SetActive(true);
        imgFade.color = new Color(0, 0, 0, 0);
        content.color = new Color(1, 1, 1, 0);

        imgFade.DOFade(fadeValue, showduration).AsyncWaitForCompletion();
        await content.DOFade(1, showduration).AsyncWaitForCompletion();
    }
    public virtual async UniTask Hide()
    {
        imgFade.DOFade(0, showduration).AsyncWaitForCompletion();
        await content.DOFade(0, showduration).AsyncWaitForCompletion();
        content.gameObject.SetActive(false);
        imgFade.gameObject.SetActive(false);
    }
}
