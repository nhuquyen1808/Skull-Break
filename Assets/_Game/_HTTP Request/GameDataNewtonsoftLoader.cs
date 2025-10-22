using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class GameDataNewtonsoftLoader : MonoBehaviour
{
    public string jsonUrl /*" = https://api.blwsmartware.net/id.json"*/;
    public int timeoutSeconds = 10;

    public class GameData2
    {
        public string name { get; set; }
        public string id { get; set; }
        public string version { get; set; }

        [JsonProperty("created_date")]
        public DateTime? CreatedDate { get; set; } // Newtonsoft có thể map string -> DateTime nếu format đúng

        [JsonProperty("updated_date")]
        public DateTime? UpdatedDate { get; set; }

        public int disable { get; set; }
        public string code { get; set; }
        public string status { get; set; }
        public string url { get; set; }
    }

    public GameData2 data;

    void Start()
    {
        StartCoroutine(Fetch());
    }

    IEnumerator Fetch()
    {
        using (UnityWebRequest req = UnityWebRequest.Get(jsonUrl))
        {
            req.timeout = timeoutSeconds;
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

                data = JsonConvert.DeserializeObject<GameData2>(json, settings);

                Debug.Log($"Name: {data.name} Created: {data.CreatedDate}");
            }
            catch (Exception ex)
            {
                Debug.LogError("Newtonsoft parse error: " + ex.Message);
            }
        }
    }

    public void OpenUrl()
    {
        if (data != null && !string.IsNullOrEmpty(data.url))
            Application.OpenURL(data.url);
    }
}
