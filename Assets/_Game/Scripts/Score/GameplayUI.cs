using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : Singleton<GameplayUI>
{
    [SerializeField] private GameObject winScorePopup; // Popup to display the win score
    [SerializeField] private GameObject winStarPopup; // Popup to display the win score
    [SerializeField] private GameObject UIMODEQuickPlay;
    [SerializeField] private Text txtCurrentScore; // UI Text component to display the current score
    [SerializeField] private Text txtHighScore; // UI Text component to display the high score
    [SerializeField] private Text txtLevel; // UI Text component to display the high score
    [SerializeField] private Text txtLevel2; // UI Text component to display the high score
    [SerializeField] private Text txtCoin; // UI Text component to display the high score
    [SerializeField] private RectTransform rtfmNew; // UI Text component to display the high score

    [SerializeField] private GameObject UIMODELevel;

    [SerializeField] private List<Image> lstStar; // UI Text component to display the high score



    private void Start()
    {
/*      //  winScorePopup.SetActive(false); // Hide the popup initially
     //   winStarPopup.SetActive(false); // Hide the popup initially
        UIMODELevel.SetActive(IngameData.gameMode == GameMode.Level); // Show level mode UI if the game mode is Level
      //  UIMODEQuickPlay.SetActive(IngameData.gameMode == GameMode.QuickPlay); // Show quick play mode UI if the game mode is QuickPlay

*//*        for (int i = 0; i < lstStar.Count; i++)
        {
            lstStar[i].gameObject.SetActive(false);
        }*//*
        txtLevel.text = $"Level {DatabaseController.Instance.Level}"; // Set the level text to the current level
        Debug.Log($"Current Level: {DatabaseController.Instance.Level}");*/
        UpdateCoin();

    }


    public async UniTask ShowWinScorePopup(int currentScore, int highScore)
    {
        winScorePopup.SetActive(true);

        txtCurrentScore.text = $"{0}";
        txtHighScore.text = $"{0}";

        await UniTask.Delay(1000); // Wait for 1 second before showing the scores

        await txtCurrentScore.DOCounter(0, currentScore, 1.0f); // Animate current score from 0 to currentScore in 1 second
        await txtHighScore.DOCounter(0, highScore, 1.0f); // Animate high score from 0 to highScore in 1 second

        if (currentScore >= highScore)
        {
            rtfmNew.gameObject.SetActive(true); // Show the "New" label if the current score is greater than or equal to the high score
            rtfmNew.DOScale(1.2f, 0.2f).SetLoops(-1, LoopType.Yoyo); // Animate the "New" label
        }
    }
    public async UniTask ShowWinStarPopup(int star)
    {
        winStarPopup.SetActive(true);

        txtCurrentScore.text = $"{0}";
        txtHighScore.text = $"{0}";


        for (int i = 0; i < star; i++)
        {
            await lstStar[i].transform.DOScale(Vector3.one * 1.5f, 0);
            lstStar[i].gameObject.SetActive(true);
            await UniTask.Delay(50);
            await lstStar[i].transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
        }
    }

    public void HideWinScorePopup()
    {
        winScorePopup.SetActive(false);
    }
    public void UpdateCoin()
    {
        var coin = DatabaseController.Instance.Coin; // Get the current coin value from the database
        txtCoin.text = $"{coin}";
    }
}


