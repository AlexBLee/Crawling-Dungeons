using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ItemFactory
{
    private const string ITEM_TYPE_KEY = "itemType";

    public enum ItemType
    {
        Helmet,
        Upper,
        RightHand,
        LeftHand,
        Lower,
        Boots
    }

    public static EquippableItem CreateItem(JToken item)
    {
        ItemType armorType = (ItemType)(int)item[ITEM_TYPE_KEY];

        switch (armorType)
        {
            case ItemType.Helmet:
                return JsonConvert.DeserializeObject<Helmet>(item.ToString());

            case ItemType.Upper:
                return JsonConvert.DeserializeObject<Upper>(item.ToString());

            case ItemType.RightHand:
                return JsonConvert.DeserializeObject<RightHand>(item.ToString());

            case ItemType.LeftHand:
                return JsonConvert.DeserializeObject<LeftHand>(item.ToString());

            case ItemType.Lower:
                return JsonConvert.DeserializeObject<Lower>(item.ToString());

            case ItemType.Boots:
                return JsonConvert.DeserializeObject<Boots>(item.ToString());

            default:
                return null;
        }
    }
}
