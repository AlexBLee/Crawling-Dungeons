using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SpellFactory
{
    public static Spell GetSpell(JToken token)
    {
        const string NameKey = "name";
        string spellName = token[NameKey].Value<string>();
        string spellJson = token.ToString();
        Spell spell = new Spell();

        switch (spellName)
        {
            case GameConstants.IceSpellName:
                spell = JsonConvert.DeserializeObject<IceBolt>(spellJson);
                break;

            case GameConstants.FireSpellName:
                spell = JsonConvert.DeserializeObject<FireBolt>(spellJson);
                break;

            case GameConstants.EnergyBallSpellName:
                spell = JsonConvert.DeserializeObject<EnergyBall>(spellJson);
                break;

            case GameConstants.ExplosionSpellName:
                spell = JsonConvert.DeserializeObject<Explosion>(spellJson);
                break;

            case GameConstants.XplosionSpellName:
                spell = JsonConvert.DeserializeObject<Xplosion>(spellJson);
                break;

            case GameConstants.GodsMightSpellName:
                spell = JsonConvert.DeserializeObject<GodsMight>(spellJson);
                break;

            default:
                return null;
        }

        spell.SetPrefab(GameDatabase.instance.GetSpellPrefab(spell.name));
        return spell;
    }
}
