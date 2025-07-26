using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItem : MonoBehaviour
{
    [SerializeField] private Text txtScore;

    public void Init(int score)
    {
        txtScore.text = $"+{score}";
        // Optionally, add animation or effects here
    }
    public async UniTask PlayAnimation()
    {
        txtScore.rectTransform.anchoredPosition = new Vector2(0, 0);
       await  txtScore.rectTransform.DOAnchorPosY(+50, 0.5f).SetEase(Ease.OutBack);
        gameObject.SetActive(false);
    }
}
