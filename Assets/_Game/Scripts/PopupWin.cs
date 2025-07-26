using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : PopupBase
{
    public override async UniTask SetupBeforeShow()
    {
        await base.SetupBeforeShow();
        SoundController.Instance.PlaySound(SoundName.LevelComplete);
    }
    public override async UniTask Show()
    {
        await base.Show();
  
    }
    public override async UniTask Hide()
    {
        await base.Hide();
    }
    public void OnClickNext()
    {
        GameplayController.Instance.OnClickRestart();
    }
}
