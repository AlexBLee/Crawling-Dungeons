using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SpellFactory
{
    public static Dictionary<string, SpellInfo> spells = new Dictionary<string, SpellInfo>();

    public static void InitializeSpellData(JToken token)
    {
        const string NameKey = "name";
        string spellName = token[NameKey].Value<string>();
        string spellJson = token.ToString();
        SpellInfo spellInfo = JsonConvert.DeserializeObject<SpellInfo>(spellJson);

        spells.Add(spellName, spellInfo);
    }

    public static Spell GetSpell(string spellName)
    {
        SpellInfo spellInfo = spells[spellName];

        switch (spellName)
        {
            case GameConstants.IceSpellName:
                return new IceBolt(spellInfo);

            case GameConstants.FireSpellName:
                return new FireBolt(spellInfo);

            case GameConstants.EnergyBallSpellName:
                return new EnergyBall(spellInfo);

            case GameConstants.ExplosionSpellName:
                return new Explosion(spellInfo);

            case GameConstants.XplosionSpellName:
                return new Xplosion(spellInfo);

            case GameConstants.GodsMightSpellName:
                return new GodsMight(spellInfo);

            default:
                return null;
        }
    }
}
