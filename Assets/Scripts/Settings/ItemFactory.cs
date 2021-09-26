using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ItemFactory
{

    public enum ItemType
    {
        Helmet,
        Upper,
        RightHand,
        LeftHand,
        Lower,
        Boots,
        HealthPot,
        ManaPot
    }

    public static Item CreateItem(JToken itemToken)
    {
        const string ItemTypeKey = "itemType";
        ItemType itemType = (ItemType)(int)itemToken[ItemTypeKey];
        string itemJson = itemToken.ToString();

        switch (itemType)
        {
            case ItemType.Helmet:
                return JsonConvert.DeserializeObject<Helmet>(itemJson);

            case ItemType.Upper:
                return JsonConvert.DeserializeObject<Upper>(itemJson);

            case ItemType.RightHand:
                return JsonConvert.DeserializeObject<RightHand>(itemJson);

            case ItemType.LeftHand:
                return JsonConvert.DeserializeObject<LeftHand>(itemJson);

            case ItemType.Lower:
                return JsonConvert.DeserializeObject<Lower>(itemJson);

            case ItemType.Boots:
                return JsonConvert.DeserializeObject<Boots>(itemJson);

            case ItemType.HealthPot:
                return JsonConvert.DeserializeObject<HealthPot>(itemJson);
            
            case ItemType.ManaPot:
                return JsonConvert.DeserializeObject<ManaPot>(itemJson);

            default:
                return null;
        }
    }
}
