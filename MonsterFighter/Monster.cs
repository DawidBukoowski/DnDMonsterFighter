namespace MonsterFighter
{
    public class Monster(string name, int armorClass, int hitPoints, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, List<Attack> attacks, List<DamageType>? immunities = null, List<DamageType>? resistances = null, List<DamageType>? vulnerabilities = null)
    {
        public string Name { get; set; } = name;
        public int ArmorClass { get; set; } = armorClass;
        public int MaxHitPoints { get; set; } = hitPoints;
        public int CurrentHitPoints { get; set; } = hitPoints;
        public Dictionary<Attribute, int> Attributes {get; set;} = new Dictionary<Attribute, int> {
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
        public string GetCurrentHP()
        {
            return $"{CurrentHitPoints}/{MaxHitPoints}";
        }
        public int GetModifier(Attribute attribute)
        {
            if(Attributes.TryGetValue(attribute, out int value)) return (value - 10) / 2;
            throw new ArgumentException($"Attribute {attribute} not found.");
        }

        public bool HasCondition(Condition condition){
            return Conditions.Contains(condition);
        }

        public void RemoveCondition(Condition condition)
        {
            Conditions.Remove(condition);
        }
    }
}
