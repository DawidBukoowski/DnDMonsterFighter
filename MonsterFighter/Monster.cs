namespace MonsterFighter
{
    public class Monster(string name, int armorClass, int hitPoints, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, List<Attack> attacks, List<DamageType>? immunities = null, List<DamageType>? resistances = null, List<DamageType>? vulnerabilities = null)
    {
        public string Name { get; set; } = name;
        public int ArmorClass { get; set; } = armorClass;
        public int MaxHitPoints { get; set; } = hitPoints;
        public int CurrentHitPoints { get; set; } = hitPoints;
        public int Strength { get; set; } = strength;
        public int Dexterity { get; set; } = dexterity;
        public int Constitution { get; set; } = constitution;
        public int Intelligence { get; set; } = intelligence;
        public int Wisdom { get; set; } = wisdom;
        public int Charisma { get; set; } = charisma;
        public List<Attack> Attacks { get; set; } = attacks;
        public List<DamageType> Immunities { get; set; } = immunities ?? [];
        public List<DamageType> Resistances { get; set; } = resistances ?? [];
        public List<DamageType> Vulnerabilities { get; set; } = vulnerabilities ?? [];

        public void Heal()
        {
            CurrentHitPoints = MaxHitPoints;
        }
        public int RollInitiative(Random rand)
        {
            int roll = rand.Next(1, 21);
            int total = roll + GetModifier(Dexterity);
            Console.WriteLine($"{Name} rolled {total} ({roll}+{GetModifier(Dexterity)}) for initiative.");
            return total;
        }
        public string GetCurrentHP()
        {
            return $"{CurrentHitPoints}/{MaxHitPoints}";
        }
        public static int GetModifier(int attribute)
        {
            return (attribute - 10) / 2;
        }
    }
}
