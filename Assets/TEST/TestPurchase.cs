using System.Collections;
using System.Collections.Generic;
using Mayotech.Store;
using Mayotech.UGSEconomy.Currency;
using Sirenix.OdinInspector;
using UnityEngine;

public class TestPurchase : MonoBehaviour
{
    [SerializeField] private ScriptableCurrency currency;
    [SerializeField, AutoConnect] private CurrencyManager currencyManager;
    [SerializeField, AutoConnect] private PurchasesData purchasesData;
    [SerializeField, AutoConnect] private StoreManager storeManager;

    [Button("Add Gold", ButtonSizes.Large, ButtonStyle.Box, Expanded = true)]
    public void AddGold(int amount)
    {
        currencyManager.IncrementSingleCurrencyBalance(currency.CurrencyId, amount, () =>
        {
            Debug.Log("Gold incremented");
        }, exception =>
        {
            Debug.LogException(exception);
        } );
    }
    
    [Button("Remove Gold", ButtonSizes.Large, ButtonStyle.Box, Expanded = true)]
    public void RemoveGold(int amount)
    {
        currencyManager.DecrementSingleCurrencyBalance(currency.CurrencyId, amount, () =>
        {
            Debug.Log("Gold incremented");
        }, exception =>
        {
            Debug.LogException(exception);
        } );
    }
    
    [Button("Make Purchase", ButtonSizes.Large)]
    public void MakePurchase()
    {
        var purchase = purchasesData.GetVirtualPuchaseDefinition("TEST_PURCHASE");
        storeManager.MakeVirtualPurchase(purchase.Id, () =>
        {
            Debug.Log("Purchase Success");
        }, exception =>
        {
            Debug.LogException(exception);
        });
    }
}
