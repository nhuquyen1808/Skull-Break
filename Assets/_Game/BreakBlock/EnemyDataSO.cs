using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnymyData> enymyDataList; // Danh sách dữ liệu của các kẻ thù

    public EnymyData GetEnemyData(BlockType blockType)
    {
        foreach (var enemyData in enymyDataList)
        {
            if (enemyData.name == blockType)
            {
                return enemyData;
            }
        }
        return null; // Trả về null nếu không tìm thấy dữ liệu cho loại kẻ thù
    }
}

[System.Serializable]
public class EnymyData
{
    public BlockType name;
    public int health;
    public float speed;
    public Sprite sprite;
}
