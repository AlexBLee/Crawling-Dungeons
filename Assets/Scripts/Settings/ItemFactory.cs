using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ItemFactory
{
    private const string ItemTypeKey = "itemType";

    public enum ItemType
    {
        Helmet,
        Upper,
        RightHand,
        LeftHand,
        Lower,
        Boots
    }

    public static EquippableItem CreateItem(JToken itemToken)
    {
        ItemType armorType = (ItemType)(int)itemToken[ItemTypeKey];
        string itemJson = itemToken.ToString();
        EquippableItem item = null;

        switch (armorType)
        {
            case ItemType.Helmet:
                item = JsonConvert.DeserializeObject<Helmet>(itemJson);
                break;

            case ItemType.Upper:
                item = JsonConvert.DeserializeObject<Upper>(itemJson);
                break;

            case ItemType.RightHand:
                item = JsonConvert.DeserializeObject<RightHand>(itemJson);
                break;

            case ItemType.LeftHand:
                item = JsonConvert.DeserializeObject<LeftHand>(itemJson);
                break;

            case ItemType.Lower:
                item = JsonConvert.DeserializeObject<Lower>(itemJson);
                break;

            case ItemType.Boots:
                item = JsonConvert.DeserializeObject<Boots>(itemJson);
                break;

            default:
                return null;
        }

        item.SetImage();
        return item;
    }
}
