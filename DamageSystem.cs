namespace TextRPGOne
{
    partial class Program
    {
        // Class to track damage resistances
        class Resistances
        {
            private int _slashingResist;
            private int _piercingResist;
            private int _bludgeoningResist;
            private int _fireResist;
            private int _frostResist;
            private int _poisonResist;
            private int _acidResist;
            private int _lightResist;
            private int _darkResist;

            public int SlashingResist { get => _slashingResist; set => _slashingResist = value; }
            public int PiercingResist { get => _piercingResist; set => _piercingResist = value; }
            public int BludgeoningResist { get => _bludgeoningResist; set => _bludgeoningResist = value; }
            public int FireResist { get => _fireResist; set => _fireResist = value; }
            public int FrostResist { get => _frostResist; set => _frostResist = value; }
            public int PoisonResist { get => _poisonResist; set => _poisonResist = value; }
            public int AcidResist { get => _acidResist; set => _acidResist = value; }
            public int LightResist { get => _lightResist; set => _lightResist = value; }
            public int DarkResist { get => _darkResist; set => _darkResist = value; }

            public Resistances(int slashing = 0, int piercing = 0, int bludgeoning = 0,
                             int fire = 0, int frost = 0, int poison = 0, int acid = 0,
                             int light = 0, int dark = 0)
            {
                SlashingResist = slashing;
                PiercingResist = piercing;
                BludgeoningResist = bludgeoning;
                FireResist = fire;
                FrostResist = frost;
                PoisonResist = poison;
                AcidResist = acid;
                LightResist = light;
                DarkResist = dark;
            }

            public int GetPhysicalResist(PhysicalDamageType type)
            {
                return type switch
                {
                    PhysicalDamageType.Slashing => SlashingResist,
                    PhysicalDamageType.Piercing => PiercingResist,
                    PhysicalDamageType.Bludgeoning => BludgeoningResist,
                    _ => 0
                };
            }

            public int GetElementalResist(ElementalDamageType type)
            {
                return type switch
                {
                    ElementalDamageType.Fire => FireResist,
                    ElementalDamageType.Frost => FrostResist,
                    ElementalDamageType.Poison => PoisonResist,
                    ElementalDamageType.Acid => AcidResist,
                    ElementalDamageType.Light => LightResist,
                    ElementalDamageType.Dark => DarkResist,
                    _ => 0
                };
            }
        }

        // Class to track damage over time effects
        class DOTEffect
        {
            private ElementalDamageType _type;
            private int _damagePerTurn;
            private int _turnsRemaining;
            private string _sourceName;

            public ElementalDamageType Type { get => _type; set => _type = value; }
            public int DamagePerTurn { get => _damagePerTurn; set => _damagePerTurn = value; }
            public int TurnsRemaining { get => _turnsRemaining; set => _turnsRemaining = value; }
            public string SourceName { get => _sourceName; set => _sourceName = value; }

            public DOTEffect(ElementalDamageType type, int damagePerTurn, int duration, string sourceName)
            {
                Type = type;
                DamagePerTurn = damagePerTurn;
                TurnsRemaining = duration;
                SourceName = sourceName;
            }

            public int ApplyDamage()
            {
                if (TurnsRemaining > 0)
                {
                    TurnsRemaining--;
                    return DamagePerTurn;
                }
                return 0;
            }

            public bool IsExpired()
            {
                return TurnsRemaining <= 0;
            }
        }

        // Damage calculation result
        class DamageResult
        {
            public int PhysicalDamage { get; set; }
            public int ElementalDamage { get; set; }
            public int TotalDamage { get; set; }
            public DOTEffect? AppliedDOT { get; set; }
            public List<string> DamageBreakdown { get; set; } = new List<string>();
        }

        // Central damage calculation system
        static class DamageCalculator
        {
            public static DamageResult CalculateDamage(
                int attackerPrimaryStat,
                Move move,
                Item? weapon,
                Resistances targetResistances)
            {
                var result = new DamageResult();

                // Base physical damage (existing formula)
                int baseDamage = (attackerPrimaryStat + move.Damage) / 2;
                int rawPhysicalDamage = baseDamage + Random.Shared.Next(-3, 4);

                // If weapon exists, apply weapon damage types
                if (weapon != null &&
                    (weapon.EquipmentType == EquipmentType.OneHandWeapon ||
                     weapon.EquipmentType == EquipmentType.TwoHandWeapon))
                {
                    // Apply physical resistance
                    int physicalResist = targetResistances.GetPhysicalResist(weapon.PhysicalDamageType);
                    result.PhysicalDamage = Math.Max(1, rawPhysicalDamage - physicalResist);
                    result.DamageBreakdown.Add($"Physical ({weapon.PhysicalDamageType}): {rawPhysicalDamage} - {physicalResist} resist = {result.PhysicalDamage}");

                    // Apply elemental damage if weapon has it
                    if (weapon.ElementalDamageType != null && weapon.ElementalDamage > 0)
                    {
                        ElementalDamageType elemType = weapon.ElementalDamageType.Value;

                        if (IsTrueDamage(elemType))
                        {
                            // True damage ignores resistances
                            result.ElementalDamage = weapon.ElementalDamage;
                            result.DamageBreakdown.Add($"Elemental ({elemType} - True DMG): {weapon.ElementalDamage}");
                        }
                        else if (IsDOT(elemType))
                        {
                            // DOT applies over time, small initial damage
                            int elementalResist = targetResistances.GetElementalResist(elemType);
                            int initialDamage = Math.Max(0, weapon.ElementalDamage / 4 - elementalResist);
                            result.ElementalDamage = initialDamage;
                            result.DamageBreakdown.Add($"Elemental ({elemType} - Initial): {initialDamage}");

                            // Create DOT effect (3 turns, damage per turn based on weapon damage)
                            int dotDamage = Math.Max(1, weapon.ElementalDamage / 3);
                            result.AppliedDOT = new DOTEffect(elemType, dotDamage, 3, weapon.Name);
                            result.DamageBreakdown.Add($"  Applied DOT: {dotDamage} dmg/turn for 3 turns");
                        }
                        else // Bonus damage (Fire, Frost)
                        {
                            int elementalResist = targetResistances.GetElementalResist(elemType);
                            result.ElementalDamage = Math.Max(0, weapon.ElementalDamage - elementalResist);
                            result.DamageBreakdown.Add($"Elemental ({elemType} - Bonus): {weapon.ElementalDamage} - {elementalResist} resist = {result.ElementalDamage}");
                        }
                    }
                }
                else
                {
                    // No weapon, just base damage (no resistances for unarmed)
                    result.PhysicalDamage = rawPhysicalDamage;
                    result.DamageBreakdown.Add($"Physical (Unarmed): {rawPhysicalDamage}");
                }

                result.TotalDamage = result.PhysicalDamage + result.ElementalDamage;
                return result;
            }

            // Apply DOT effects at start of turn
            public static int ApplyDOTEffects(List<DOTEffect> activeDoTs, out List<string> dotMessages)
            {
                dotMessages = new List<string>();
                int totalDotDamage = 0;

                // Apply all active DOTs
                for (int i = activeDoTs.Count - 1; i >= 0; i--)
                {
                    int damage = activeDoTs[i].ApplyDamage();
                    if (damage > 0)
                    {
                        totalDotDamage += damage;
                        dotMessages.Add($"[red]{activeDoTs[i].Type}[/] deals {damage} damage! ({activeDoTs[i].TurnsRemaining} turns remaining)");
                    }

                    // Remove expired DOTs
                    if (activeDoTs[i].IsExpired())
                    {
                        dotMessages.Add($"[dim]{activeDoTs[i].Type} effect has worn off.[/]");
                        activeDoTs.RemoveAt(i);
                    }
                }

                return totalDotDamage;
            }
        }
    }
}
