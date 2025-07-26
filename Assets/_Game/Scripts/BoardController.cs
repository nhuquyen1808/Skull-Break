using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static Unity.Burst.Intrinsics.X86.Avx;
using static Unity.Collections.AllocatorManager;
using Random = UnityEngine.Random;
public enum BoardState
{
    Ready = 0,
    Animation = 1
}
public class BoardController : MonoBehaviour
{
    public static BoardController Instance { get; private set; }
    public BoardState BoardState { get => boardState; }

    [SerializeField] private ObjectPool objectPool; // Link đến Object Pool
    [SerializeField] private SpriteRenderer boardSprite; // Hình ảnh cho board
    private Block[,] blocks;
    [SerializeField] private BoardState boardState; // Link đến Object Pool
    [SerializeField] private bool isFirstClick = true;

    [SerializeField] private Block firstBlock;
    [SerializeField] private List<int> lstValue;
    [SerializeField] private int countOpenned;
    [SerializeField] private int rows;
    [SerializeField] private int cols;
    [SerializeField] private int countEnemy;


    void Awake()
    {
        Instance = this;
    }
    public void Initialize()
    {
        countOpenned = 0;
        CreateBoard();
        boardState = BoardState.Ready;
    }
    void CreateBoard()
    {
        var levelData = LevelController.Instance.GetLevel(DatabaseController.Instance.Level);
        float scale = 0.51f;
        float scaleY = 0.5f;
        rows = levelData.rows;
        cols = levelData.cols;
        blocks = new Block[cols, rows];
        // Calculate starting position to center the board
        float startX = -(cols / 2f * scale) + scale / 2;
        float startY = -(rows / 2f * scaleY) + scaleY / 2 + 0.1f;
        // Create blocks from bottom-left (0, 0) moving right and up
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Block block = objectPool.GetBlock();
                if (block != null)
                {
                    // Shift odd columns up by half a cell
                    float yOffset = 0;
                    Vector3 position = new Vector3(
                        startX + col * scale,
                        startY + row * scaleY + yOffset,
                        0
                    );
                    block.transform.localPosition = position;
                    block.transform.parent = boardSprite.transform;
                    int index = row * cols + col;
                    Debug.Log($"Create block at {col}, {row} with index {index}");
                    block.Initialize(col, row, levelData.blocks[index]);
                    blocks[col, row] = block;
                    if (levelData.blocks[index].blockType != BlockType.None)
                    {
                        countEnemy++;
                    }
                }
            }
        }
    }
    public async UniTask MoveDownEnemy()
    {
        bool willLose = false;
        boardState = BoardState.Animation;
        var lstTask = new List<UniTask>();
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                Block block = blocks[col, row];
                if (block != null && block &&block.BlockType !=BlockType.None)
                {
                    lstTask.Add(
                        block.transform.DOMoveY(block.transform.position.y - 0.5f, 0.5f).SetEase(Ease.InOutSine).ToUniTask()
                        );
                    block.MoveDown();
                    if (block.Row <= 0 && block.BlockState!=BlockState.Final)
                    {
                        willLose = true;
                     
                    }
                }
            }
        }
        await UniTask.WhenAll(lstTask);
        if (willLose)
        {
            GameplayController.Instance.OnGameOver();
        }
        else
        {
            boardState = BoardState.Ready;
        }
    }
    public void On1EnemyDie()
    {
        SoundController.Instance.PlaySound(SoundName.EnemyDie);
        countEnemy--;
        if (countEnemy <= 0)
        {
            GameplayController.Instance.OnWin();
        }
    }
}