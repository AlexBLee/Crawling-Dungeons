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
        Boots,
        HealthPot,
        ManaPot
    }

    public static Item CreateItem(JToken itemToken)
    {
        ItemType armorType = (ItemType)(int)itemToken[ItemTypeKey];
        string itemJson = itemToken.ToString();
        Item item = null;

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

            // TODO: Find a way to deserialize more types of pots without bloating the code.
            case ItemType.HealthPot:
                item = JsonConvert.DeserializeObject<HealthPot>(itemJson);
                break;
            
            case ItemType.ManaPot:
                item = JsonConvert.DeserializeObject<ManaPot>(itemJson);
                break;

            default:
                return null;
        }

        item.SetImage();
        return item;
    }
}
