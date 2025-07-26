using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GamePlayState
{
    Ready, 
    Shotting,
    EnemyMove,
}

public class GameplayController : Singleton<GameplayController>
{
    public GameplayUI gameplayUI;
    [SerializeField] private bool isPlaying;
    [SerializeField] private ParticleSystem parWin;
    [SerializeField] private GamePlayState gamePlayState = GamePlayState.Ready;
    [SerializeField] private TextMeshPro txtLevel;

    public bool IsPlaying { get => isPlaying;  }
    public GamePlayState GamePlayState { get => gamePlayState; }

    //    public ProcessController processController;

    void Start()
    {
        isPlaying = true;
        BoardController.Instance.Initialize();
        txtLevel.text = $"Level {DatabaseController.Instance.Level}"; // Set the level text to the current level
    }
    public void OnRestartButtonClicked()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnClickRestart()
    {
    /*    int level = PlayerPrefs.GetInt("level", 1);
        PlayerPrefs.SetInt("level", level - 1);*/
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnClickNext()
    {
        SceneManager.LoadScene("Gameplay");
    }
    public async UniTask OnWin()
    {
        if (!isPlaying)
            return; // Tránh xử lý nhiều lần khi đã hoàn thành
        isPlaying = false;
        DatabaseController.Instance.Ball += 1; // Thưởng 100 coin khi hoàn thành level
        DatabaseController.Instance.Coin += 20; // Thưởng 100 coin khi hoàn thành level
        DatabaseController.Instance.Level++;
        SoundController.Instance.PlaySound(SoundName.LevelComplete);
        parWin.Play();
        await UniTask.Delay(1000);

        PopupController.Instance.ShowPopup(PopupType.LevelComplete);
    }
    public async UniTask OnGameOver()
    {
        if (!isPlaying)
            return; // 
        isPlaying = false;
          SoundController.Instance.PlaySound(SoundName.Down);
        Debug.Log("Game Over");
        await UniTask.Delay(1000);
        PopupController.Instance.ShowPopup(PopupType.Lose);
    }
    public void OnShoot()
    {
        gamePlayState = GamePlayState.Shotting;
    }
    public async UniTask OnDoneShoot()
    {
        gamePlayState = GamePlayState.EnemyMove;
      await   MovingEnemyController.Instance.OnDoneShoot();
        gamePlayState = GamePlayState.Ready;
    }
}
