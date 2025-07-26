using System.Collections.Generic;
using UnityEngine;

public class LevelController :Singleton<LevelController>
{
    [SerializeField] private List<TextAsset> levelDataList;
    [SerializeField] private LevelData currentLevelData;

    public LevelData GetLevel(int level)
    {
        level -= 1;
        if (level < 0 || level >= levelDataList.Count)
        {
           level = Random.Range(9, levelDataList.Count);
        }
        currentLevelData = JsonUtility.FromJson<LevelData>(levelDataList[level].text); ;
        return JsonUtility.FromJson<LevelData>(levelDataList[level].text);
    }
}
