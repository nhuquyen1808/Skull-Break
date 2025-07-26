using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class PlayerPrefsViewerWindow : EditorWindow
{
    private Vector2 scrollPos;

    [MenuItem("Tools/PlayerPrefs Viewer")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsViewerWindow>("PlayerPrefs Viewer");
    }

    void OnGUI()
    {
        GUILayout.Label("PlayerPrefs Viewer", EditorStyles.boldLabel);

        if (GUILayout.Button("Refresh"))
        {
            Repaint();
        }

        if (GUILayout.Button("Delete All PlayerPrefs"))
        {
            if (EditorUtility.DisplayDialog("Warning", "Are you sure you want to delete all PlayerPrefs?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();
            }
        }

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        DrawAllPlayerPrefs();
        EditorGUILayout.EndScrollView();
    }

    void DrawAllPlayerPrefs()
    {
#if UNITY_EDITOR_WIN
        string regKey = $"Software\\Unity\\UnityEditor\\{Application.companyName}\\{Application.productName}";
        Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regKey);

        if (key != null)
        {
            foreach (string valueName in key.GetValueNames())
            {
                object val = key.GetValue(valueName);
                string raw = val.ToString();

                GUILayout.BeginVertical("box");
                GUILayout.BeginHorizontal();
                GUILayout.Label($"Key: {valueName}", EditorStyles.boldLabel);

                if (GUILayout.Button("Delete", GUILayout.Width(60)))
                {
                    PlayerPrefs.DeleteKey(valueName);
                    PlayerPrefs.Save();
                    Repaint();
                    break;
                }
                GUILayout.EndHorizontal();

                string prettyValue = TryFormatJson(raw);
                GUIStyle style = new GUIStyle(EditorStyles.textArea) { wordWrap = true };
                EditorGUILayout.TextArea(prettyValue, style, GUILayout.ExpandHeight(false));
                GUILayout.EndVertical();
            }
            key.Close();
        }
        else
        {
            GUILayout.Label("No PlayerPrefs found.");
        }
#else
    GUILayout.Label("Listing PlayerPrefs is only supported in Windows Editor.");
#endif
    }
    string TryFormatJson(string input)
    {
        input = input.Trim();
        if ((input.StartsWith("{") && input.EndsWith("}")) || (input.StartsWith("[") && input.EndsWith("]")))
        {
            try
            {
                var parsed = UnityEngine.JsonUtility.FromJson<GenericWrapper>(input);
                return JsonUtility.ToJson(parsed, true);
            }
            catch
            {
                return input; // không phải JSON đúng
            }
        }

        return input; // không phải JSON
    }

    [System.Serializable]
    private class GenericWrapper
    {
        // wrapper rỗng để JsonUtility không báo lỗi
    }
}
