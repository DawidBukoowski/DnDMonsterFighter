namespace MonsterFighter
{
    public class RecurringEffect(string name, int savingThrowDC, Attribute savingThrowAttribute, DamageComponent damageComponent)
    {
        public string Name { get; set; } = name;
        public int SavingThrowDC { get; set; } = savingThrowDC;
        public Attribute SavingThrowAttribute { get; set; } = savingThrowAttribute;
        public DamageComponent DamageComponent { get; set; } = damageComponent;
    }
}

