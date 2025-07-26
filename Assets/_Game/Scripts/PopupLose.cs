using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLose : PopupBase
{
    public override UniTask Show()
    {
        SoundController.Instance.PlaySound(SoundName.Fail);
        return base.Show();
    }
    public void OnClickRetry()
    {
        Hide().Forget();
        GameplayController.Instance.OnClickRestart();
    }    
}
