
using GameApiDto.Dtos;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : IDetailedStoreListener
{
    public enum EPurchaseType
    {
        OK,
        NotManagerLoaded, // 매니저가 로드되지 않음.
    }
    private Dictionary<string, ResDtoGetCashProductListItem> _dict = new ();
    public IReadOnlyDictionary<string, ResDtoGetCashProductListItem> Dict => _dict;
    private ReactiveProperty<bool> _isLoad = new ReactiveProperty<bool>(false);
    public IReadOnlyReactiveProperty<bool> IsLoad => _isLoad;

    IStoreController m_StoreController; // The Unity Purchasing system.

    public void LateInit()
    {
        Managers.WebContents.GetCashProductList(new ReqDtoGetCashProductList(), OnLoadItems);
    }

    public void OnLoadItems(ResDtoGetCashProductList result)
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        foreach (var item in result.List)
        {
            _dict[item.ProductId] = item;

            if(item.ItemType == ResDtoGetCashProductListItem.EItemType.Subscription)
            {
                builder.AddProduct(item.ProductId, ProductType.Subscription);
            }
            else
            {
                if(item.IsConsumable)
                {
                    builder.AddProduct(item.ProductId, ProductType.Consumable);
                }
                else
                {
                    builder.AddProduct(item.ProductId, ProductType.NonConsumable);
                }
            }
        }

        Debug.Log("Developer User");
        StandardPurchasingModule.Instance().useFakeStoreUIMode = FakeStoreUIMode.Default;

        UnityPurchasing.Initialize(this, builder);



        _isLoad.Value = true;
    }

    public void Buy(string productId)
    {
        m_StoreController.InitiatePurchase(productId);
    }

    private EPurchaseType PurchaseXXX(ResDtoGetCashProductListItem item)
    {
        if (_isLoad.Value == false)
        {
            UI_ToastPopup.Show($"IS NOT LOAD {nameof(IAPManager)}::{nameof(PurchaseXXX)}({item.Id})", UI_ToastPopup.Type.Debug);
            return EPurchaseType.NotManagerLoaded;
        }

        // 실제 구매처리
        UI_ToastPopup.Show($"Complete {nameof(IAPManager)}::{nameof(PurchaseXXX)}({item.Id})", UI_ToastPopup.Type.Debug);

        return EPurchaseType.OK;
    }



    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        var message = "In-App Purchasing successfully initialized";
        Debug.Log(message);
        m_StoreController = controller;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        var errorMessage = $"Purchase failed - Product: '{product.definition.id}'," +
            $" Purchase failure reason: {failureDescription.reason}," +
            $" Purchase failure details: {failureDescription.message}";
        Debug.Log(errorMessage);
        UI_ToastPopup.Show(errorMessage, UI_ToastPopup.Type.Error);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        OnInitializeFailed(error, null);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

        if (message != null)
        {
            errorMessage += $" More details: {message}";
        }

        Debug.Log(errorMessage);
        //UI_ToastPopup.Show(errorMessage, UI_ToastPopup.Type.Error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        var errorMessage = $"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}";
        UI_ToastPopup.Show(errorMessage, UI_ToastPopup.Type.Error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        //Retrieve the purchased product
        var product = args.purchasedProduct;
        //args.purchasedProduct.definition.payout.quantity

        var status = this.PurchaseXXX(this.Dict[product.definition.id]);

        if(status != EPurchaseType.OK)
        {
            return PurchaseProcessingResult.Pending;
        }

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

}