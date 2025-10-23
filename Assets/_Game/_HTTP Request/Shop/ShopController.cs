using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
public class ShopController : Singleton<ShopController>
{
    [SerializeField] private GameObject shopUI;
    [SerializeField] private List<ItemCoin> lstItemCoin;
    bool isOpen = false;

    public bool IsOpen { get => isOpen; }

    protected override void CustomAwake()
    {//GameDataLoader.instance.CheckNetwork();
    }
    private async void Start()
    {
        GameDataLoader.instance.CheckNetwork();
        await LoadIAP();
    }

    private async Task LoadIAP()
    {        
        if(GameDataLoader.instance.disabledStatus) return;
        Time.timeScale = 1f;
        await IAPController.Instance.InitializeUnityGamingServices(); 
        IAPController.Instance.InitializePurchasing();
        await UniTask.WaitUntil(() =>
            DatabaseController.Instance != null &&
            IAPController.Instance != null &&
            IAPController.Instance.IsInitialized()
        );
        Debug.Log($"Shop initialization started. Current coins: {DatabaseController.Instance.Coin}"); 
        
        InitializeIAP();
        InitializeItemCoins();

    }

    public async UniTask ShowShop()
    {
        if (isOpen)
        {
            Hide();
            return;
        }
        Show();
        await UniTask.Delay(1000);
    }
    public void Show()
    {
        if(GameDataLoader.instance.disabledStatus) return;
        SoundController.Instance.PlaySound(SoundName.Down);
       // AudioController.Instance.PlayClickDropSound();
        shopUI.gameObject.SetActive(true);
        isOpen = true;
        // Timmer.Instance.PauseTime();
    }

    public void Hide()
    {
       // AudioController.Instance.PlayClickDropSound();
       SoundController.Instance.PlaySound(SoundName.Click);
        shopUI.gameObject.SetActive(false);
        isOpen = false;
        // Timmer.Instance.ResumeTime();
    }

    public void InitializeIAP()
    {
        IAPController.Instance.OnPurchaseSuccess = (key) =>
        {
            Debug.Log($"Purchased item with key: {key}");
            for (int i = 0; i < lstItemCoin.Count; i++)
            {
                if (lstItemCoin[i].Key == key)
                {
                    lstItemCoin[i].OnSuccess();
                    // Cập nhật số coins trong GameManager để refresh UI
                    //GameManager.instance.UpdateCoin();
                 //   Debug.Log($"Added {lstItemCoin[i].CoinReceive} coins. New total: {DatabaseController.Instance.Coin}");
                }
            }
        };
        Debug.Log("IAP initialized");

    }

    public void InitializeItemCoins()
    {
        //  for (int i = 0; i < lstItemCoin.Count; i++)
        for (int i = lstItemCoin.Count - 1; i >= 0; i--)
        {
            var product = IAPController.Instance.GetProductByKey(lstItemCoin[i].Key);
            Debug.Log($"InitializeItemCoins: name:{gameObject.name} key:{lstItemCoin[i].Key} {product == null}");
            var price = product != null ? product.metadata.localizedPriceString : "N/A";
            lstItemCoin[i].Init(price, OnClickItem);
        }

        Debug.Log("Item coins initialized");
    }
    public void OnClickItem(string key)
    {
        try
        {
           // AudioController.Instance?.PlayClickDropSound();
           SoundController.Instance.PlaySound(SoundName.Click);

            Debug.Log("=== PURCHASE DEBUG INFO ===");
            Debug.Log($"1. Clicked item key: {key}");

            // Kiểm tra DatabaseController
            if (DatabaseController.Instance != null)
            {
                Debug.Log($"2. Database status: OK, Current coins: {DatabaseController.Instance.Coin}");
            }
            else
            {
                Debug.LogError("2. Database status: NULL!");
                return;
            }
            // Kiểm tra thông tin sản phẩm
          /*  var product = IAPController.Instance?.GetProductByKey(key);
            if (product != null)
            {
                Debug.Log($"7. Product info - Key: {key}, Price: {product.metadata.localizedPriceString}");
            }*/
            IAPController.Instance.BuyProduct(key, out string resultMessage);

            Debug.Log("=== END PURCHASE DEBUG ===");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in OnClickItem: {e.Message}\n{e.StackTrace}");
        }
    }
}
