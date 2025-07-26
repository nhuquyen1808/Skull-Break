using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BlockState
{
    Close,
    Open,
    Final
}
public class Block : MonoBehaviour
{
    [SerializeField] private int column;
    [SerializeField] private int row;
    [SerializeField] private BlockState blockState;
    [SerializeField] private BlockType blockType;
    [SerializeField] private SpriteRenderer sprEnemy;
    [SerializeField] private Transform tfmBack;
    [SerializeField] private int value;
    [SerializeField] private Image imgHP;
    [SerializeField] private Text txtHP;
    [SerializeField] private ParticleSystem parDie;

    public int Value { get => value;}
    public BlockState BlockState { get => blockState;}
    public BlockType BlockType { get => blockType; }
    public int Row { get => row; set => row = value; }
    public void MoveDown()
    {
        row--;
    }

    private void Start()
    {
        //   lstNeighbor = new List<Block>();

    }
    public void Initialize(int col, int row, BlockData blockData)
    {
        blockType = blockData.blockType;
        InitUI();

        blockState = BlockState.Close;
        column = col;
        this.row = row;
        gameObject.name = $"Block {blockType} at ({col}, {row})";
    }
    private void InitUI()
    {
        var enemyData = EnemyDataController.Instance.GetBlockImageByType(blockType);
        if (enemyData == null)
        {
            Debug.LogWarning($"No data found for block type: {blockType}");
            return; // Trả về nếu không tìm thấy dữ liệu
        }
        sprEnemy.sprite = enemyData.sprite;
        value = enemyData.health;
        tfmBack.gameObject.SetActive(blockType!= BlockType.None);
    }

    public void SetValue(int value)
    {
        this.value = value;
        UpdateUI();

    }
    private async UniTask UpdateUI()
    {
        imgHP.fillAmount = (float)value / EnemyDataController.Instance.GetBlockImageByType(blockType).health;
        txtHP.text = value.ToString();
    }    
/*    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"aaaaaaa {collision.gameObject.name} at {collision.contacts[0].point}");
        var ball = collision.gameObject.GetComponent<Ball>();
        if (ball != null)
        {
         
        }
    }*/
    public void OnHit()
    {
        Debug.Log($"Block {blockType} at ({column}, {row}) hit! Current value: {value}");
        value--;
        UpdateUI();
        if (value <= 0)
        {
            OnDie();
        }
    }
    private void OnDie()
    {
        blockState = BlockState.Final;
        tfmBack.gameObject.SetActive(false);
        sprEnemy.gameObject.SetActive(false);
        parDie.Play();
        BoardController.Instance.On1EnemyDie();
    }
}
