using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class HintCntroller : Singleton<HintCntroller>
{
    [SerializeField] private Transform tfmA;
    [SerializeField] private Transform tfmB;
    [SerializeField] private Transform tfmHand;
    [SerializeField] private GameObject gobjDetail;
    [SerializeField] private GameObject gobjBooster1;
    [SerializeField] private GameObject gobjBooster2;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private bool startHint = false;

    private void Start()
    {
        if (DatabaseController.Instance.Level == 1)
        {
            StartHint();
        }
    }
    public async UniTask CoroutineHint()
    {
        startHint = true;
        tfmHand.position = tfmA.position;
        tfmHand.gameObject.SetActive(true);
        gobjDetail.gameObject.SetActive(true);
        gobjBooster1.gameObject.SetActive(false);
        gobjBooster2.gameObject.SetActive(false);

        while (startHint)
        {
            await tfmHand.DOLocalMove(tfmB.localPosition, duration).ToUniTask();
            await tfmHand.DOLocalMove(tfmA.localPosition, duration).ToUniTask();
        }
    }
    public void StartHint()
    {
        CoroutineHint();
    }
    public void StopHint()
    {
        tfmHand.gameObject.SetActive(false);
        gobjDetail.gameObject.SetActive(false);
        gobjBooster1.gameObject.SetActive(true);
        gobjBooster2.gameObject.SetActive(true);
        tfmHand.DOKill();
        startHint = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopHint();
        }
    }
}
