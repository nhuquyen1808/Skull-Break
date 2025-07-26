using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class EffectFlare : MonoBehaviour
{
    // Start is called before the first frame update
    private Image image;
    private RectTransform rectTransform;
    [SerializeReference] private float timeDuration = 1f;
    [SerializeReference] private float endValueAlpha = 0.5f;
    [SerializeReference] private float scaleTo = 1.5f;
    [SerializeReference] private Vector3 originScale;

    [SerializeReference] private float euler;

    void Start()
    {
        image = GetComponent<Image>();
        originScale = this.transform.localScale;
        DoFade();
        rectTransform = GetComponent<RectTransform>();
        OnScale();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(new Vector3(0, 0, euler * Time.deltaTime));
    }
/*    void DoFade()
    {
        image.DOFade(endValueAlpha, timeDuration).OnComplete(() =>
        {
            image.DOFade(1, timeDuration).OnComplete(() =>
            {
                DoFade();
            });
        });
    }*/
    async UniTask DoFade(float time = 0.5f)
    {
        while (true)
        {
            float offset = 5f;
            await image.DOFade(endValueAlpha, timeDuration);
            await image.DOFade(1, timeDuration);
        }
    }

    void OnScale()
    {
        rectTransform.DOScale(originScale * scaleTo, timeDuration).OnComplete(() =>
         {
             GetComponent<RectTransform>().DOScale(originScale, timeDuration).OnComplete(() =>
             {
                 OnScale();
             });
         });
    }
}
