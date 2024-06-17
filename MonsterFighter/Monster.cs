namespace MonsterFighter
{
    public class Monster(string name, int armorClass, int hitPoints, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, List<Attack> attacks, List<DamageType>? immunities = null, List<DamageType>? resistances = null, List<DamageType>? vulnerabilities = null)
    {
        public string Name { get; set; } = name;
        public int ArmorClass { get; set; } = armorClass;
        public int MaxHitPoints { get; set; } = hitPoints;
        public int CurrentHitPoints { get; set; } = hitPoints;
        public Dictionary<Attribute, int> Attributes { get; set; } = new Dictionary<Attribute, int> {
            {Attribute.Strength, strength},
            {Attribute.Dexterity, dexterity},
            {Attribute.Constitution, constitution},
            {Attribute.Intelligence, intelligence},
            {Attribute.Wisdom, wisdom},
            {Attribute.Charisma, charisma}
        };
        public List<Attack> Attacks { get; set; } = attacks;
        public List<DamageType> Immunities { get; set; } = immunities ?? [];
        public List<DamageType> Resistances { get; set; } = resistances ?? [];
        public List<DamageType> Vulnerabilities { get; set; } = vulnerabilities ?? [];
        public List<Condition> Conditions { get; set; } = [];
        public List<RecurringEffect> RecurringEffects { get; set; } = [];
        public void Heal()
        {
            CurrentHitPoints = MaxHitPoints;
        }
        public int RollInitiative(Random rand)
        {
            int roll = rand.Next(1, 21);
            int total = roll + GetModifier(Attribute.Dexterity);
            Console.WriteLine($"{Name} rolled {total} ({roll}+{GetModifier(Attribute.Dexterity)}) for initiative.");
            return total;
        }
        public int RollSave(Attribute attribute, int dc, Random rand)
        {
            int roll = rand.Next(1, 21);
            int modifier = GetModifier(attribute);
            int total = roll + modifier;
            string sign = modifier >= 0 ? "+" : "";
            Console.WriteLine($"{Name} rolled {total} ({roll}{sign}{modifier}) on a DC {dc} {attribute} saving throw.");
            return total;
        }
        public string GetCurrentHP()
        {
            return $"{CurrentHitPoints}/{MaxHitPoints}";
        }
        public int GetModifier(Attribute attribute)
        {
            if (Attributes.TryGetValue(attribute, out int value)) return (value - 10) / 2;
            throw new ArgumentException($"Attribute {attribute} not found.");
        }
        public void ApplyCondition(Condition condition)
        {
            if (!HasCondition(condition))
            {
                Conditions.Add(condition);
                Console.WriteLine($"{Name} is now {condition}.");
            }
        }
        public bool HasCondition(Condition condition)
        {
            return Conditions.Contains(condition);
        }
        public void RemoveCondition(Condition condition)
        {
            if (HasCondition(condition))
            {
                Conditions.Remove(condition);
                Console.WriteLine($"{Name} is no longer {condition}.");
            }
            Conditions.Remove(condition);
        }

        public void ApplyRecurringEffect(RecurringEffect effect)
        {
            RecurringEffects.Add(effect);
            Console.WriteLine($"{Name} is now affected by {effect.Name}.");
        }

        public void ResolveRecurringEffects(Random rand)
        {
            List<RecurringEffect> effectsToRemove = [];

            foreach (var effect in RecurringEffects)
            {
                int roll = rand.Next(1, 21);
                int total = roll + GetModifier(effect.SavingThrowAttribute);

                if (total >= effect.SavingThrowDC)
                {
                    Console.WriteLine($"{Name} succeeds the saving throw and ends the {effect.Name} effect!");
                    effectsToRemove.Add(effect);
                }
                else
                {
                    int damage = effect.DamageComponent.RollDamage(rand);
                    CurrentHitPoints -= damage;
                    Console.WriteLine($"{Name} fails the saving throw and takes {damage} {effect.DamageComponent.DamageType} damage!");

                    if (CurrentHitPoints <= 0)
                    {
                        break;
                    }
                }
            }

            foreach (var effect in effectsToRemove)
            {
                RecurringEffects.Remove(effect);
                Console.WriteLine($"{Name} is no longer affected by {effect.Name}.");
            }
        }
    }
}
