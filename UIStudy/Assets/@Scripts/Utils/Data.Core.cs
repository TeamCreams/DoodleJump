using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    #region About IAP 
    /*
     * ProductID,ProductName,Price,Currency,ItemType,Amount,IsConsumable
     */

    public enum ECurrencyType
    {
        KRW,
        USD
    }

    public enum EItemType
    {
        Cash,
        Subscription
    }

    [Serializable]
    public class IapProductData
    {
        // 제품 Id
        public string ProductId;
        // 제품 이름
        public string ProductName;
        // 가격
        public float Price;
        // 통화 (달러, 원)
        public ECurrencyType Currency;
        // 아이템 타입 (캐시, 구독)
        public EItemType ItemType;
        // 제공할 양
        public int Amount;
        // 소모성 상품인경우
        public bool IsConsumable;
    }

    [Serializable]
    public class IapProductDataLoader : ILoader<string, IapProductData>
    {
        public List<IapProductData> iaps = new List<IapProductData>();

        public Dictionary<string, IapProductData> MakeDict()
        {
            Dictionary<string, IapProductData> dict = new Dictionary<string, IapProductData>();
            foreach (IapProductData data in iaps)
                dict.Add(data.ProductId, data);

            return dict;
        }
    }

    #endregion
}