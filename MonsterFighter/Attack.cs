namespace MonsterFighter
{
    public class Attack(string name, int bonusToHit, List<DamageComponent> damageComponents, AttackType type = AttackType.Single, List<Attack>? subAttacks = null, Condition? condition = null, int savingThrowDC = 0, Attribute? savingThrowAttribute = null, RecurringEffect? recurringEffect = null)
    {
        public string Name { get; set; } = name;
        public int BonusToHit { get; set; } = bonusToHit;
        public AttackType Type { get; set; } = type;
        public List<Attack> SubAttacks { get; set; } = subAttacks ?? [];
        public List<DamageComponent> DamageComponents { get; set; } = damageComponents;
        public Condition? Condition { get; set; } = condition;
        public int SavingThrowDC { get; set; } = savingThrowDC;
        public Attribute? SavingThrowAttribute { get; } = savingThrowAttribute;
        public RecurringEffect? RecurringEffect { get; set; } = recurringEffect;

    }
}