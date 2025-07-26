using UnityEngine;
using UnityEditor;
using System.IO;

public class LevelDataWindow : EditorWindow
{
    private LevelData levelData = new LevelData();
    private int levelNumber = 1;
    private const string folderPath = "Assets/_Game/_LevelData/";
    // Thêm biến để lưu asset EnemyDataSO
    private EnemyDataSO enemyDataSO;
    private BlockType currentType = BlockType.None;
    [MenuItem("Tools/Level Data Window")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataWindow>("Level Data Window");
    }
    private void ReloadDataEnemy()
    {
        Debug.Log("Reloading EnemyDataSO...");
        // Tìm asset EnemyDataSO trong project (chỉ cần filter theo type)
        string[] guids = AssetDatabase.FindAssets("t:EnemyDataSO");
        if (guids.Length > 0)
        {
            Debug.Log("Found EnemyDataSO asset.");
            string path = AssetDatabase.GUIDToAssetPath(guids[0]);
            enemyDataSO = AssetDatabase.LoadAssetAtPath<EnemyDataSO>(path);
        }
        else
        {
            Debug.LogWarning("EnemyDataSO asset not found.");
            enemyDataSO = null;
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Level Data Save/Load", EditorStyles.boldLabel);

        levelNumber = EditorGUILayout.IntField("Level", levelNumber);

        if (GUILayout.Button("Save Level Data"))
        {
            SaveLevelData();
        }

        if (GUILayout.Button("Load Level Data"))
        {
            LoadLevelData();
        }

        if (GUILayout.Button("Reset"))
        {
            levelData = new LevelData();
        }

        // Hiển thị thông tin levelData
        EditorGUILayout.LabelField("Rows", levelData.rows.ToString());
        EditorGUILayout.LabelField("Cols", levelData.cols.ToString());
        EditorGUILayout.LabelField("Blocks", levelData.blocks.Count.ToString());

        // Vẽ danh sách enemy trong EnemyDataSO
        if (GUILayout.Button("Reload Enemy",GUILayout.Width(100)))
        {
          //  Debug.Log("Reloading EnemyDataSO...");
            ReloadDataEnemy();
        }
        DrawListEnemy();

        GUILayout.Space(10);
        GUILayout.Label("Level Board", EditorStyles.boldLabel);
        DrawBoardGrid();
    }

    private void DrawListEnemy()
    {
        GUILayout.Space(10);
        GUILayout.Label("Enemy List in EnemyDataSO", EditorStyles.boldLabel);

        if (enemyDataSO != null && enemyDataSO.enymyDataList != null)
        {
            foreach (var enemy in enemyDataSO.enymyDataList)
            {
                EditorGUILayout.BeginHorizontal();
                // Hiển thị BlockType
                EditorGUILayout.LabelField(enemy.name.ToString(), GUILayout.Width(80));
                // Hiển thị sprite dưới dạng button, khi bấm sẽ chọn type
                Texture2D tex = enemy.sprite != null ? AssetPreview.GetAssetPreview(enemy.sprite) : null;
                GUIStyle style = new GUIStyle(GUI.skin.button);
                style.fixedWidth = 48;
                style.fixedHeight = 48;
                if (GUILayout.Button(tex != null ? tex : Texture2D.grayTexture, style, GUILayout.Width(48), GUILayout.Height(48)))
                {
                    currentType = enemy.name;
                }
                // Hiển thị trạng thái đã chọn
                if (currentType == enemy.name)
                {
                    GUILayout.Label("Selected", GUILayout.Width(60));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("Không tìm thấy EnemyDataSO hoặc danh sách rỗng.");
        }
    }
    private string GetFileName()
    {
        return $"level{levelNumber}.json";
    }

    private void SaveLevelData()
    {
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string json = JsonUtility.ToJson(levelData, true);
        string fullPath = Path.Combine(folderPath, GetFileName());
        File.WriteAllText(fullPath, json);
        AssetDatabase.Refresh();
        Debug.Log("Level data saved to " + fullPath);

        // Ping file vừa lưu trong Project window
        var asset = AssetDatabase.LoadAssetAtPath<TextAsset>(fullPath);
        if (asset != null)
            EditorGUIUtility.PingObject(asset);
    }

    private void LoadLevelData()
    {
        string path = Path.Combine(folderPath, GetFileName());
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            levelData = JsonUtility.FromJson<LevelData>(json);
            Debug.Log("Level data loaded from " + path);
        }
        else
        {
            Debug.LogWarning("File not found: " + path);
        }
    }
    private void DrawBoardGrid()
    {
        if (levelData == null || levelData.blocks == null) return;

        int rows = levelData.rows;
        int cols = levelData.cols;
        float cellSize = 40f;

        for (int row = rows-1; row >= 0; row--)
        {
            EditorGUILayout.BeginHorizontal();
            for (int col = 0; col < cols; col++)
            {
                BlockData block = levelData.GetBlock(col, row);
                Texture2D tex = null;
                if (enemyDataSO != null && block != null && block.blockType != BlockType.None)
                {
                    var enemy = enemyDataSO.GetEnemyData(block.blockType);
                    if (enemy != null && enemy.sprite != null)
                        tex = AssetPreview.GetAssetPreview(enemy.sprite);
                }

                GUIContent content = tex != null
                    ? new GUIContent(tex)
                    : new GUIContent(block != null ? block.blockType.ToString() : "");

                if (GUILayout.Button(content, GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                {
                    if (block != null)
                        block.blockType = currentType;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
