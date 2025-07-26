using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IngameData
{
    public static GameMode gameMode = GameMode.Level;
    public static bool ShowLevelMap = false;

}
public enum GameMode
{
    QuickPlay,
    Level
}

