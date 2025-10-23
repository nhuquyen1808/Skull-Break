#if UNITY_EDITOR
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;

public class captureSceen : MonoBehaviour
{
    private bool isCreateFolder = false;
    public bool isCapturePortrait = false;
    public static captureSceen instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnValidate()
    {
        string folderPath = "Assets/InfoGame/"; // the path of your project folder
        if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
        {
            AssetDatabase.CreateFolder("Assets", "InfoGame");
            AssetDatabase.Refresh();
        }   
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Time.timeScale = 0;
            CaptureScreenshots();
            StartCoroutine(ToggleEditorPause()) ;

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Time.timeScale = 1;
        }
        /*if (Input.GetKeyDown(KeyCode.Space))
        { // capture screen shot on space key down
            CaptureScreenshots();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                CaptureIcon();
            }
        }*/
    }
    
    [ContextMenu("CaptureIcon")]
    void CaptureIcon()
    {
        string folderPath = "Assets/InfoGame/"; // the path of your project folder

        if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
            System.IO.Directory.CreateDirectory(folderPath);  // it will get created
        CaptureScreenshot(folderPath, "Icon_512", 512, 512);
    }

    [ContextMenu("CaptureScreenshots")]
    void CaptureScreenshots()
    {
        string folderPath = "Assets/InfoGame/"; // the path of your project folder

        if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
            System.IO.Directory.CreateDirectory(folderPath);  // it will get created

        if (isCapturePortrait)
        {
            //CaptureScreenshot(folderPath, "Portrait_1242x2688", 1242, 2688);
            //CaptureScreenshot(folderPath, "Portrait_1242x2208", 1242, 2208);

            //Huy_Update
         //   CaptureScreenshot(folderPath, "Image_Portrait_1080x1920", 1080, 1920);
           // CaptureScreenshot(folderPath, "store_kit_", 1080, 1920);
            StartCoroutine(Capture(folderPath, "store_kit_"));
        }
        else
        {
            CaptureScreenshot(folderPath, "Portrait_2688x1242", 2688, 1242);
            CaptureScreenshot(folderPath, "Portrait_2208x1242", 2208, 1242);
        }
        // else
        // {
        //     var screenshotName =
        //                         "Screenshot_" +
        //                         System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + // puts the current time right into the screenshot name
        //                         ".png"; // put your favorite data format here
        //
        //     ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName), 1); // takes the screenshot
        //     AssetDatabase.Refresh();
        //     Debug.Log("Capture Screenshot Name " + screenshotName);
        // }
    }
  
    void CaptureScreenshot(string folderPath, string resolutionName, int width, int height)
    {
        var renderTexture = new RenderTexture(width, height, 24);
        var screenshotTexture = new Texture2D(width, height, TextureFormat.RGB24, false);

        Camera.main.targetTexture = renderTexture;
        Camera.main.Render();

        RenderTexture.active = renderTexture;
        screenshotTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshotTexture.Apply();

        Camera.main.targetTexture = null;
        RenderTexture.active = null;

        Destroy(renderTexture);

        var screenshotName =
                            resolutionName + "_" +
                            System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                            ".png";

        var screenshotPath = System.IO.Path.Combine(folderPath, screenshotName);
        System.IO.File.WriteAllBytes(screenshotPath, screenshotTexture.EncodeToPNG());
        AssetDatabase.Refresh();
        Debug.Log("Captured Screenshot: " + screenshotPath);
    }
    
    private IEnumerator Capture(string folderPath, string resolutionName)
    {
        // Chờ đến cuối frame, khi cả UI + world đã render xong
        yield return new WaitForEndOfFrame();

        int width = Screen.width;
        int height = Screen.height;

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        string screenshotName = resolutionName + "_" +
                                System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                                ".png";

        string screenshotPath = Path.Combine(folderPath, screenshotName);
        File.WriteAllBytes(screenshotPath, tex.EncodeToPNG());

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        Debug.Log("Captured Screenshot: " + screenshotPath);

        Object.Destroy(tex);
    }
    
    IEnumerator ToggleEditorPause()
    {
#if UNITY_EDITOR
        // Đảo trạng thái Pause của Editor
        yield return new WaitForEndOfFrame();
        EditorApplication.isPaused = !EditorApplication.isPaused;
#else
        // Trong build thực tế thì chỉ log ra
        Debug.Log("Pause chỉ hoạt động trong Unity Editor!");
#endif
    }
}
#endif