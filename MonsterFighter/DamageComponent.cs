namespace MonsterFighter
{
    public class DamageComponent
    {
        public string? Damage { get; set; }
        public DamageType DamageType { get; set; }
        public int? FixedDamage { get; set; }
        public bool IsFixedDamage { get; set; }
        public int? SavingThrowDC { get; set; }
        public Attribute? SavingThrowAttribute { get; set; }

        public DamageComponent(string damage, DamageType damageType, int? savingThrowDC = null, Attribute? savingThrowAttribute = null)
        {
            Damage = damage;
            DamageType = damageType;
            IsFixedDamage = false;
            SavingThrowDC = savingThrowDC;
            SavingThrowAttribute = savingThrowAttribute;
        }
        public DamageComponent(int damage, DamageType damageType, int? savingThrowDC = null, Attribute? savingThrowAttribute = null)
        {
            FixedDamage = damage;
            DamageType = damageType;
            IsFixedDamage = true;
            SavingThrowDC = savingThrowDC;
            SavingThrowAttribute = savingThrowAttribute;
        }

        public int RollDamage(Random rand)
        {
            if (IsFixedDamage && FixedDamage.HasValue) return FixedDamage.Value;

            if (Damage != null)
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
            throw new InvalidOperationException("DamageComponent is not properly initialized.");
        }
    }
}