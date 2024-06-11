using System.Runtime;

namespace MonsterFighter
{
    public class StatusEffect(string name, int duration, Attribute savingThrowAttribute, int savingThrowDC = 0, DamageComponent? recurringDamage = null, Condition? condition = null)
    {
        public string Name { get; set; } = name;
        public int Duration { get; set; } = duration;
        public int SavingThrowDC { get; set; } = savingThrowDC;
        public Attribute SavingThrowAttribute { get; set; } = savingThrowAttribute;
        public DamageComponent? RecurringDamage { get; set; } = recurringDamage;
        public Condition? Condition { get; set; } = condition;
        public bool HasSavingThrow { get; set; } = savingThrowDC > 0;

        public bool ResolveEffect(Monster target, Random rand)
        {
            if (HasSavingThrow)
            {
                int roll = rand.Next(1, 21);
                int attributeMod = target.GetModifier(SavingThrowAttribute);
                int total = roll + attributeMod;

                if (total >= SavingThrowDC)
                {
                    Console.WriteLine($"{target.Name} succeeds the saving throw against {Name} effect!");
                    return true;
                }
                else if (RecurringDamage != null)
                {
                    int damage = RecurringDamage.RollDamage(rand);
                    target.CurrentHitPoints -= damage;
                    Console.WriteLine($"{target.Name} fails the saving throw and takes {damage} {RecurringDamage.DamageType} damage!");
                }
            }
            return false;
        }

        public StatusEffect Clone()
        {
            return new StatusEffect(Name, Duration, SavingThrowAttribute, SavingThrowDC, RecurringDamage);
        }
    }
}