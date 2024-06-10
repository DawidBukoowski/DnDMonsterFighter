namespace MonsterFighter
{
    public class Attack(string name, int bonusToHit, List<DamageComponent> damageComponents, AttackType type = AttackType.Single, List<Attack>? subAttacks = null, StatusEffect? statusEffect = null)
    {
        public string Name { get; set; } = name;
        public int BonusToHit { get; set; } = bonusToHit;
        public AttackType Type { get; set; } = type;
        public List<Attack> SubAttacks { get; set; } = subAttacks ?? [];
        public List<DamageComponent> DamageComponents { get; set; } = damageComponents;
        public StatusEffect? StatusEffect {get; set;} = statusEffect;

    }
}