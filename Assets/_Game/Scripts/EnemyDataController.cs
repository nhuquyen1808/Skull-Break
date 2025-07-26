using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class EnemyDataController : MonoBehaviour
{
    public static EnemyDataController Instance { get; private set; }
    void Awake()
    {
        Instance = this; // Đảm bảo chỉ có một instance của BoardController
    }
    [SerializeField] private EnemyDataSO data; // Mảng các hình ảnh cho loại block
    public EnymyData GetBlockImageByType(BlockType type)
    {
        EnymyData enymyData = data.GetEnemyData(type);
        if (enymyData == null)
        {
            Debug.LogWarning($"No data found for block type: {type}");
            return null; // Trả về null nếu không tìm thấy dữ liệu cho loại block
        }
        Debug.Log($"GetBlockImageByType: {type}, Health: {enymyData.health}, Speed: {enymyData.speed}");
        return enymyData; // Trả về dữ liệu của loại block
    }
}
