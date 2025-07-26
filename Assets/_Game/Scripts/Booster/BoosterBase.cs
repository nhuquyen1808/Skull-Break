using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterBase : MonoBehaviour
{
    [SerializeField] protected int coinCost;
    [SerializeField] protected Text txtCoin;
    private void Start()
    {
        txtCoin.text = coinCost.ToString();
    }
    public void OnClickUseBooster()
    {
        if (BoardController.Instance.BoardState != BoardState.Ready)
        {
            Debug.LogWarning("Board is not ready for shuffle.");
            return; // Không làm gì nếu board không sẵn sàng
        }
        Debug.Log($"OnClickUseBooster: name:{gameObject.name}");
        var coin =DatabaseController.Instance.Coin; // Get the current coin value from the database
        if (coin < coinCost)
        {
            Debug.Log("Not enough coin");
            ShopController.Instance.Show();

            return;
        }
        DatabaseController.Instance.Coin -= coinCost; // Decrease the coin count
        GameplayUI.Instance.UpdateCoin();
        OnSuccess();
    }
    protected virtual void OnSuccess() { }
}
