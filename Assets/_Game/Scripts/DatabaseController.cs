using UnityEditor;
using UnityEngine;

public class DatabaseController : Singleton<DatabaseController>
{
    private void Awake()
    {
        CheckDependence();
    }
    private int coin;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            PlayerPrefs.SetInt(DBKeyThisGame.COIN, coin);
            PlayerPrefs.Save();
        }
    }    private int ball;
    public int Ball
    {
        get => ball;
        set
        {
            ball = value;
            PlayerPrefs.SetInt(DBKeyThisGame.BALL, ball);
            PlayerPrefs.Save();
        }
    }
    private int level;

    public int Level
    {
        get => level;
        set
        {
            level = value;
            Debug.Log($"Level: {level}");
            PlayerPrefs.SetInt(DBKeyThisGame.LEVEL, level);
            PlayerPrefs.Save();
        }
    }
    private int bestTime;

    public int BEST_TIME
    {
        get => bestTime;
        set
        {
            bestTime = value;
            PlayerPrefs.SetInt(DBKeyThisGame.BEST_TIME, bestTime);
            PlayerPrefs.Save();
        }
    }  

    public void CheckDependence()
    {
        if (!PlayerPrefs.HasKey(DBKeyThisGame.COIN))
        {
            PlayerPrefs.SetInt(DBKeyThisGame.COIN, 500);
            PlayerPrefs.Save();
        } 
        if (!PlayerPrefs.HasKey(DBKeyThisGame.BALL))
        {
            PlayerPrefs.SetInt(DBKeyThisGame.BALL, 5);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey(DBKeyThisGame.LEVEL))
        {
            PlayerPrefs.SetInt(DBKeyThisGame.LEVEL, 1); // Default level is 1
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey(DBKeyThisGame.BEST_TIME))
        {
            PlayerPrefs.SetInt(DBKeyThisGame.BEST_TIME, 1000); // Default level is 1
            PlayerPrefs.Save();
        }

        Load();
    }
    // Load data from PlayerPrefs
    public void Load()
    {
        coin = Load<int>(DBKeyThisGame.COIN); // Default coin is 500
        ball = Load<int>(DBKeyThisGame.BALL); // Default coin is 500
        level = Load<int>(DBKeyThisGame.LEVEL); // Default level is 1
        bestTime = Load<int>(DBKeyThisGame.BEST_TIME); // Default level is 1
    }
    private T Load<T>(string key)
    {
        if (!PlayerPrefs.HasKey(key))
            return default;

        var type = typeof(T);

        if (type == typeof(int))
            return (T)(object)PlayerPrefs.GetInt(key);
        else if (type == typeof(float))
            return (T)(object)PlayerPrefs.GetFloat(key);
        else if (type == typeof(string))
            return (T)(object)PlayerPrefs.GetString(key);
        else if (type == typeof(bool))
            return (T)(object)(PlayerPrefs.GetInt(key) == 1);

        return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
    }
}

public static class DBKeyThisGame
{
    public const string COIN = "COIN";
    public const string BALL = "BALL";
    public const string LEVEL = "LEVEL";
    public const string SOUND = "SOUND";
    public const string VIBRATE = "VIBRATE";

    public const string BEST_TIME = "BEST_TIME";
 
}