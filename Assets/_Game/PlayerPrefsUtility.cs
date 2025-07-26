using System.Collections.Generic;
using UnityEditor;

public static class PlayerPrefsUtility
{
    /// <summary>
    /// L?y t?t c? c�c key trong PlayerPrefs (ch? ho?t ??ng trong Unity Editor).
    /// </summary>
    /// <returns>Danh s�ch c�c key.</returns>
    public static List<string> GetAllKeys()
    {
        List<string> keys = new List<string>();

#if UNITY_EDITOR
        // L?y danh s�ch c�c key t? Unity EditorPrefs
        var prefs = EditorPrefs.GetString("UnityEditor.PlayerSettings.PlayerPrefs");
        if (!string.IsNullOrEmpty(prefs))
        {
            var lines = prefs.Split('\n');
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    var key = line.Split('=')[0].Trim();
                    keys.Add(key);
                }
            }
        }
#endif

        return keys;
    }
}
