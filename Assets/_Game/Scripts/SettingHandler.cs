using Cysharp.Threading.Tasks;
using DG.Tweening;
//using Storage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingHandler : MonoBehaviour
{
    [SerializeField] private RectTransform rtfmSettingPanel;
    [SerializeField] private bool showSetting = false;
    [SerializeField] private bool isAnimationShow = false;
    [SerializeField] private float hidePosY;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float fade = 0.9f;
    [SerializeField] private Ease ease = Ease.OutQuad;

    [SerializeField] private ButtonSettings btnSound;
    [SerializeField] private ButtonSettings btnVibrate;
    [SerializeField] private Image imgFade; // Thêm Image imgFade

    public void OnClickSetting()
    {
        // AudioController.Instance.PlaySound(SoundName.Click);
        SoundController.Instance.PlaySound(SoundName.Click);

        if (showSetting)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    private void Start()
    {
      /*  btnSound.SetState(Db.storage.SETTING_DATAS.music);
        btnVibrate.SetState(Db.storage.SETTING_DATAS.vibra);*/
    }

    private async UniTask Show()
    {
        showSetting = true;
        isAnimationShow = true;
        rtfmSettingPanel.DOKill();
        imgFade.DOKill();

        // Hiệu ứng fade in
        imgFade.gameObject.SetActive(true);
        imgFade.DOFade(fade, duration).SetEase(ease);

        await rtfmSettingPanel.DOAnchorPosY(0, duration).SetEase(ease);
        isAnimationShow = false;
    }

    private async UniTask Hide()
    {
        showSetting = false;
        isAnimationShow = true;
        rtfmSettingPanel.DOKill();
        imgFade.DOKill();

        // Hiệu ứng fade out
        imgFade.DOFade(0, duration).SetEase(ease).OnComplete(() => imgFade.gameObject.SetActive(false));

        await rtfmSettingPanel.DOAnchorPosY(hidePosY, duration).SetEase(ease);
        isAnimationShow = false;
    }

    public void OnClickSound()
    {
        SoundController.Instance.PlaySound(SoundName.Click);
        /* AudioController.Instance.PlaySound(SoundName.Click);

         if (isAnimationShow)
             return;
         var setting = Db.storage.SETTING_DATAS;
         setting.music = !setting.music;
         Db.storage.SETTING_DATAS = setting;
         AudioController.Instance.SetSound(setting.music);*/
        PlayerPrefs.SetInt("Sound", PlayerPrefs.GetInt("Sound",1) == 1 ? 0 : 1);
        bool isSound = PlayerPrefs.GetInt("Sound", 1) == 1;
         btnSound.SetState(isSound);
        if (isSound)
        {
            SoundController.Instance.SoundUpdate(false);
        }
        else
        {
            SoundController.Instance.SoundUpdate(true);
        }
    }

    public void OnClickVibrate()
    {
        SoundController.Instance.PlaySound(SoundName.Click);
        SceneManager.LoadScene("MainMenu");
        Hide();
       

        /*  AudioController.Instance.PlaySound(SoundName.Click);

          if (isAnimationShow)
              return;
          var setting = Db.storage.SETTING_DATAS;
          setting.vibra = !setting.vibra;
          Db.storage.SETTING_DATAS = setting;
          btnVibrate.SetState(setting.vibra);*/
    }

    public void OnClickRestart()
    {
        SoundController.Instance.PlaySound(SoundName.Click);
        GameplayController.Instance.OnRestartButtonClicked();
        Hide();

        /* AudioController.Instance.PlaySound(SoundName.Click);

         if (isAnimationShow)
             return;
         GameManager.Instance.GamePlayController.OnClickReloadScene();*/
    }
}
