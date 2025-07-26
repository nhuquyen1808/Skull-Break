using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private List<Transform> lstEnemy;
    [SerializeField] private Transform posA;
    [SerializeField] private Transform posB;

    void Start()
    {
        Application.targetFrameRate = 120;
        Load();
    }
    private async UniTask Load()
    {
       
        var lstTask = new List<UniTask>();
        foreach (var enemy in lstEnemy)
        {
            enemy.position = posA.position;
            lstTask.Add(enemy.DOMove(posB.position, 3f).SetEase(Ease.Linear).ToUniTask());
            await UniTask.Delay(400); // Delay to stagger the movement of enemies
        }

        await UniTask.WhenAll(lstTask); // Wait for all enemies to finish moving
        await UniTask.Delay(1000); // Wait for 2 seconds
        SceneManager.LoadScene("Gameplay"); // Load the main menu scene
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
