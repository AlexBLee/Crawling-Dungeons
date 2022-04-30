using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SpellFactory
{
    public static SpellInfo GetSpell(JToken token)
    {
        const string NameKey = "name";
        string spellName = token[NameKey].Value<string>();
        string spellJson = token.ToString();

        switch (spellName)
        {
            case GameConstants.IceSpellName:
                return JsonConvert.DeserializeObject<IceBolt>(spellJson);

            case GameConstants.FireSpellName:
                return JsonConvert.DeserializeObject<FireBolt>(spellJson);

            case GameConstants.EnergyBallSpellName:
                return JsonConvert.DeserializeObject<EnergyBall>(spellJson);

            case GameConstants.ExplosionSpellName:
                return JsonConvert.DeserializeObject<Explosion>(spellJson);

            case GameConstants.XplosionSpellName:
                return JsonConvert.DeserializeObject<Xplosion>(spellJson);

            case GameConstants.GodsMightSpellName:
                return JsonConvert.DeserializeObject<GodsMight>(spellJson);

            default:
                return null;
        }
    }
}
