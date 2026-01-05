using System;
using System.Collections;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class GameDataAPILoad
{
    public string name;
    public string id;
    public string version;
    public string created_date;
    public string updated_date;
    public int disable;
    public string code;
    public string status;
    public string url;
    public string msg;
}

public class GameDataLoader : MonoBehaviour
{
    [Header("URL JSON")] public string jsonUrl = "https://api.blwsmartware.net/id.json";

    [Header("UI (tuỳ chọn)")] public TextMeshProUGUI uiName; // assign nếu dùng Unity UI
    public TextMeshProUGUI packageID; // assign nếu dùng Unity UI
    public TextMeshProUGUI IDText; // assign nếu dùng Unity UI
    public TextMeshProUGUI uiVersion;
    public TextMeshProUGUI uiStatus;
    public TextMeshProUGUI uiDisabledStatus;
    public TextMeshProUGUI msgText;

    [Header("Value")] public bool disabledStatus;
    public int ID;
    public GameObject PopupNetworkError;


    // public TextMeshProUGUI uiName;  // nếu dùng TextMeshPro, đổi kiểu tương ứng

    [Header("Cấu hình mạng")] public int timeoutSeconds = 10; // request timeout
    public int maxRetries = 2; // số lần thử lại khi lỗi mạng

    [HideInInspector] public GameDataAPILoad data; // dữ liệu đã parse
    public static GameDataLoader instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowPopupNetworkError()
    {
        PopupNetworkError.SetActive(true);
    }

    public void HidePopupNetworkError()
    {
        PopupNetworkError.SetActive(false);
    }

    public void CheckNetwork()
    {
        StartCoroutine(FetchWithRetry(jsonUrl, maxRetries));
    }

    IEnumerator FetchWithRetry(string url, int retriesLeft)
    {
        yield return StartCoroutine(FetchGameData(url, success =>
        {
            if (!success && retriesLeft > 0)
            {
                Debug.LogWarning($"Fetch failed. Retrying... ({retriesLeft} left)");
                StartCoroutine(FetchWithRetry(url, retriesLeft - 1));
            }
        }));

        Fetch();
    }

    IEnumerator FetchGameData(string url, Action<bool> onComplete)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.SetRequestHeader("Accept", "application/json");
            req.timeout = timeoutSeconds;
            Debug.Log("Requesting: " + url);
            yield return req.SendWebRequest();

            // Kiểm tra lỗi (Unity 2020+ dùng req.result)
            if (req.result == UnityWebRequest.Result.ConnectionError ||
                req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Request error: {req.error}");
                onComplete?.Invoke(false);
                yield break;
            }

            string json = req.downloadHandler.text;
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError("Empty JSON response");
                onComplete?.Invoke(false);
                yield break;
            }

            Debug.Log("Raw JSON: " + json);

            try
            {
                data = JsonUtility.FromJson<GameDataAPILoad>(json);
                if (data == null)
                {
                    Debug.LogError("JsonUtility returned null. JSON may not match class structure.");
                    onComplete?.Invoke(false);
                    yield break;
                }

                ApplyDataToGame(data);
                onComplete?.Invoke(true);
            }
            catch (Exception ex)
            {
                Debug.LogError("Parsing error: " + ex.Message);
                onComplete?.Invoke(false);
            }
        }
    }

    void ApplyDataToGame(GameDataAPILoad d)
    {
        Debug.Log($"Loaded: {d.name} ({d.id}) ver:{d.version} status:{d.status} disable:{d.disable}");

        // Hiển thị lên UI nếu có
        if (uiName != null) uiName.text = d.name;
        if (uiVersion != null) uiVersion.text = "Ver: " + d.version;
        if (uiStatus != null) uiStatus.text = "Status: " + d.status;
        if (uiDisabledStatus != null) uiDisabledStatus.text = "Disable: " + d.disable;
        if (packageID != null) packageID.text = Application.identifier.ToString();
        if (IDText != null) IDText.text = d.id;
        if (msgText != null) msgText.text = d.msg;
        // Parse ngày (format MM-dd-yyyy)
        if (DateTime.TryParseExact(d.created_date, "MM-dd-yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out DateTime created))
        {
            Debug.Log($"Created date parsed: {created.ToString("yyyy-MM-dd")}");
        }
        else
        {
            Debug.LogWarning("Không parse được created_date: " + d.created_date);
        }

        // Ví dụ: mở URL nếu online và không disable

        if (d.disable == 1)
        {
            ShowPopupNetworkError();
            disabledStatus = true;
        }
        else
        {
            disabledStatus = false;
        }

        //  disabledStatus = d.disable != 0;
        if (!disabledStatus && string.Equals(d.status, "ONLINE", StringComparison.OrdinalIgnoreCase))
        {
            Debug.Log("Mở URL: " + d.url);
#if UNITY_EDITOR
            // Application.OpenURL(d.url);
#endif
            // chú ý: Application.OpenURL hoạt động trong Editor và build
            // Application.OpenURL(d.url);
        }
    }

    // Public function để gọi từ UI Button (nút mở URL thủ công)
    public void OpenUrlFromData()
    {
        if (data != null && !string.IsNullOrEmpty(data.url))
            Application.OpenURL(data.url);
        else
            Debug.LogWarning("Chưa có url trong data hoặc data null");
    }


    public int timeoutNewtonSoftSeconds = 10;

    public class GameData2
    {
        public string name { get; set; }
        public string id { get; set; }
        public string version { get; set; }

        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; } // Newtonsoft có thể map string -> DateTime nếu format đúng

        [JsonProperty("updated_date")] public DateTime? UpdatedDate { get; set; }

        public int disable { get; set; }
        public string code { get; set; }
        public string status { get; set; }
        public string url { get; set; }
    }

    public GameData2 dataNewtonSoft;


    IEnumerator Fetch()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(jsonUrl))
        {
            req.timeout = timeoutNewtonSoftSeconds;
            req.SetRequestHeader("Accept", "application/json");
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Request failed: " + req.error);
                yield break;
            }

            string json = req.downloadHandler.text;
            Debug.Log("Raw JSON: " + json);

            try
            {
                var settings = new JsonSerializerSettings
                {
                    DateFormatString = "MM-dd-yyyy", // set format để map CreatedDate
                    NullValueHandling = NullValueHandling.Ignore
                };

                dataNewtonSoft = JsonConvert.DeserializeObject<GameData2>(json, settings);

                Debug.Log($"Name: {dataNewtonSoft.name} Created: {dataNewtonSoft.CreatedDate}");
            }
            catch (Exception ex)
            {
                Debug.LogError("Newtonsoft parse error: " + ex.Message);
            }
        }
    }

    public void OpenUrl()
    {
        if (dataNewtonSoft != null && !string.IsNullOrEmpty(dataNewtonSoft.url))
            Application.OpenURL(dataNewtonSoft.url);
    }
}