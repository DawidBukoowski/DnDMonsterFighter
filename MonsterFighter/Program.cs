using MonsterFighter;

Monster bandit = new("Bandit", 12, 11, 12, [
    new("Scimitar", 3, "1d6+1", DamageType.Slashing),
    new("Light Crossbow", 3, "1d8-1", DamageType.Piercing)
]);
Monster bloodHawk = new("Blood Hawk", 12, 7, 14, [
    new("Beak", 4, "1d4+2", DamageType.Piercing)
]);
Monster btharpy = new("Blood-Toll Harpy", 11, 9, 13, [
    new("Multiattack", 0, "", DamageType.Piercing, AttackType.Multi,[
        new("Bite", 3, "1d4+1", DamageType.Piercing),
        new("Claws", 3, "1d4+1", DamageType.Slashing)
    ])
]);

FightSimulator.SimulateFight(bandit, btharpy);

