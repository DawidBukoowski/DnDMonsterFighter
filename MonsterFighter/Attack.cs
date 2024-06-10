namespace MonsterFighter
{
    public class Attack(string name, int bonusToHit, List<DamageComponent> damageComponents, AttackType type = AttackType.Single, List<Attack>? subAttacks = null)
    {
        public string Name { get; set; } = name;
        public int BonusToHit { get; set; } = bonusToHit;
        public AttackType Type { get; set; } = type;
        public List<Attack> SubAttacks { get; set; } = subAttacks ?? [];
        public List<DamageComponent> DamageComponents { get; set; } = damageComponents;

    }

    public class DamageComponent(string damage, DamageType damageType)
    {
        public string Damage { get; set; } = damage;
        public DamageType DamageType { get; set; } = damageType;

        public int RollDamage(Random rand)
        {
            var parts = Damage.Split('d', '+', '-');
            int n = int.Parse(parts[0]);
            int k = int.Parse(parts[1]);
            int b = parts.Length > 2 ? int.Parse(parts[2]) : 0;

            int totalDamage = 0;
            for (int i = 0; i < n; i++)
            {
                totalDamage += rand.Next(1, k + 1);
            }
            return totalDamage += b;
        }
    }
    public enum DamageType
    {
        Slashing,
        Bludgeoning,
        Piercing,
        Fire,
        Cold,
        Lightning,
        Thunder,
        Poison,
        Acid,
        Necrotic,
        Radiant,
        Force,
        Psychic
    }
    public enum AttackType
    {
        Single,
        Multi
    }
}