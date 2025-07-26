using Cysharp.Threading.Tasks;
using DG.Tweening;
using MainMenuBar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : Singleton<MainMenuController>
{
    [SerializeField] private GameObject gobjMap;
    [SerializeField] private GameObject gobjMenu;
    private void Start()
    {
        if (IngameData.ShowLevelMap)
            OnClickAdvanture();
    }
    public void OnClickQuickPlay()
    {
        SoundController.Instance.PlaySound(SoundName.Click);
        IngameData.ShowLevelMap = false;
        IngameData.gameMode = GameMode.QuickPlay;
        SceneManager.LoadScene("Gameplay"); // Load the main menu scene

    }
    public void OnClickAdvanture()
    {
        SoundController.Instance.PlaySound(SoundName.Click);
    /*    ToastMessage.Instance.ShowToast("Coming Soon");*/
        gobjMap.SetActive(true);
        gobjMenu.SetActive(false);
        IngameData.gameMode = GameMode.Level;
    }
    public void OnClickBackToMap()
    {
        SoundController.Instance.PlaySound(SoundName.Click);

        gobjMap.SetActive(false);
        gobjMenu.SetActive(true);
    }    
    public void OnCLickPlay()
    {
        SceneManager.LoadScene("Gameplay"); // Load the main menu scene

    }
}
