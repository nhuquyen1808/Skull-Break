using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MovingEnemyController : Singleton<MovingEnemyController>
{
    [SerializeField] private int timeToMove = 4;
    [SerializeField] private int countTime = 0;
    [SerializeField] private Image imgFillTime;
    public void Init()
    {
        countTime = 0;
        imgFillTime.fillAmount = (float)countTime / timeToMove;
    }
    public async UniTask OnDoneShoot()
    {
        countTime++;
        imgFillTime.DOFillAmount((4 - (float)countTime) / timeToMove, 0.5f);

        if (countTime >= timeToMove)
        {
            imgFillTime.fillAmount = 0f;
            BoardController.Instance.MoveDownEnemy();
            return;
        }
    }
}
