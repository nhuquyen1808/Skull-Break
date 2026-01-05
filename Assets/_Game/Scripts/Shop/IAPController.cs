using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;


namespace IAP_Dev
{
    [System.Serializable]
public class ItemIap
{
    public string key;
    public ProductType productType;
}

public class IAPController : Singleton<IAPController>
{
    StoreController m_StoreController;

    [SerializeField] private List<ItemIap> lstKeyCode;

    // [SerializeField] private bool isInitialized = false;
    public Action<bool> OnPurchaseSuccess;

    protected override void CustomAwake()
    {
        // InitializeIAP();
    }

    private void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No internet connection.");
            GameDataLoader.instance.ShowPopupNetworkError();
            GameDataLoader.instance.disabledStatus = true;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("Connected via Wi-Fi or LAN.");
            GameDataLoader.instance.CheckNetwork();
            if (GameDataLoader.instance.disabledStatus) return;
            StartCoroutine(InitializeIAPPack());

            // InitializeIAP();
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            Debug.Log("Connected via mobile data.");
            GameDataLoader.instance.CheckNetwork();
            StartCoroutine(InitializeIAPPack());
            // InitializeIAP();
        }
    }

    IEnumerator InitializeIAPPack()
    {
        yield return new WaitForSeconds(1);
        if (GameDataLoader.instance.disabledStatus == false)
            InitializeIAP();
    }

    async void InitializeIAP()
    {
        m_StoreController = UnityIAPServices.StoreController();
        m_StoreController.OnPurchasePending += OnPurchasePending;
        m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
        m_StoreController.OnPurchaseFailed += OnPurchaseFailed;

        m_StoreController.OnStoreDisconnected += OnStoreDisconnected;
        Debug.Log("Connecting to store.");
        await m_StoreController.Connect();

        m_StoreController.OnProductsFetchFailed += OnProductsFetchedFailed;
        m_StoreController.OnProductsFetched += OnProductsFetched;
        FetchProducts();
    }

    void FetchProducts()
    {
        var initialProductsToFetch = new List<ProductDefinition>();
        for (int i = 0; i < lstKeyCode.Count; i++)
        {
            initialProductsToFetch.Add(new(lstKeyCode[i].key, lstKeyCode[i].productType));
        }

        m_StoreController.FetchProducts(initialProductsToFetch);
    }

    void OnPurchaseFailed(FailedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product == null)
        {
            Debug.Log("Could not find product in failed order.");
        }

        OnPurchaseSuccess?.Invoke(false);
        Debug.Log($"Purchase failed - Product: '{product?.definition.id}'," +
                  $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                  + $"Purchase Failure Details: {order.Details}");
    }

    void OnPurchasePending(PendingOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product is null)
        {
            Debug.Log("Could not find product in order.");
            OnPurchaseSuccess?.Invoke(false);
            return;
        }

        OnPurchaseSuccess?.Invoke(true);
        Debug.Log($"Purchase complete - Product: {product.definition.id}");
        //m_StoreController.ConfirmPurchase(order);
    }

    void OnPurchaseConfirmed(Order order)
    {
        switch (order)
        {
            case ConfirmedOrder confirmedOrder:
                OnPurchaseConfirmed(confirmedOrder);
                break;
            case FailedOrder failedOrder:
                OnPurchaseConfirmationFailed(failedOrder);
                break;
            default:
                //OnPurchaseSuccess?.Invoke(true);
                Debug.Log("Unknown OnPurchaseConfirmed result.");
                break;
        }
        // if (order is FailedOrder failedOrder)
        // {
        //     Debug.Log("Purchase False");
        //     OnPurchaseSuccess?.Invoke(false);
        // }
        // else
        // {
        //     Debug.Log("Purchase complete");
        //     OnPurchaseSuccess?.Invoke(true);
        // }
    }

    void OnPurchaseConfirmed(ConfirmedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product == null)
        {
            Debug.Log("Could not find product in purchase confirmation.");
            OnPurchaseSuccess?.Invoke(false);
        }
        else
        {
            OnPurchaseSuccess?.Invoke(true);
        }

        Debug.Log($"Purchase confirmed- Product: {product?.definition.id}");
    }

    void OnPurchaseConfirmationFailed(FailedOrder order)
    {
        var product = GetFirstProductInOrder(order);
        if (product == null)
        {
            Debug.Log("Could not find product in failed confirmation.");
        }

        OnPurchaseSuccess?.Invoke(false);
        Debug.Log($"Confirmation failed - Product: '{product?.definition.id}'," +
                  $"PurchaseFailureReason: {order.FailureReason.ToString()},"
                  + $"Confirmation Failure Details: {order.Details}");
    }

    Product GetFirstProductInOrder(Order order)
    {
        return order.CartOrdered.Items().First()?.Product;
    }

    // Calling StoreController.Connect without a listener on the StoreController.OnStoreDisconnected event will result in warnings.
    void OnStoreDisconnected(StoreConnectionFailureDescription description)
    {
        Debug.Log($"Store disconnected details: {description.message}");
    }

    // Calling StoreController.Connect without listeners on StoreController.OnProductsFetched and StoreController.OnProductsFetchedFailed will result in warnings.
    void OnProductsFetched(List<Product> products)
    {
        Debug.Log($"Products fetched successfully for {products.Count} products.");
    }

    void OnProductsFetchedFailed(ProductFetchFailed failure)
    {
        Debug.Log($"Products fetch failed for {failure.FailedFetchProducts.Count} products: {failure.FailureReason}");
    }

    public void BuyProduct(string productId, Action<bool> success)
    {
        OnPurchaseSuccess = success;
        m_StoreController?.PurchaseProduct(productId);
    }

    public string GetPriceValue(string productId)
    {
        var product = m_StoreController?.GetProducts().FirstOrDefault(p => p.definition.id == productId);
        if (product != null)
        {
            return product.metadata.localizedPriceString;
        }
        else
        {
            Debug.Log("Product not found");
            return "Loading...";
        }
    }
}
}
