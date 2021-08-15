using Newtonsoft.Json;

public class ArmorFactory
{
    public enum ArmorType
    {
        Helmet,
        Upper,
        LeftHand,
        RightHand,
        Lower,
        Boots
    }

    public static ArmorItem CreateArmor(int type, string json)
    {
        ArmorType armorType = (ArmorType)type;

        switch (armorType)
        {
            case ArmorType.Helmet:
                return JsonConvert.DeserializeObject<Helmet>(json);

            case ArmorType.Upper:
                return JsonConvert.DeserializeObject<Upper>(json);

            case ArmorType.RightHand:
                return JsonConvert.DeserializeObject<LeftHand>(json);

            case ArmorType.Lower:
                return JsonConvert.DeserializeObject<Lower>(json);

            case ArmorType.Boots:
                return JsonConvert.DeserializeObject<Boots>(json);

            default:
                return null;
        }
    }
}
