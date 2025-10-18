using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        class NPC
        {
            //TODO change NPC stats to come from to Stats.cs
            private string _name;
            private string _description;
            private int _health;
            private int _maxHealth;
            private int _mana;
            private int _maxMana;
            private Stats _stats;
            private int _initiative;
            private Move[] _moveSet;
            private LootTableItem[] _lootTable;
            private int _goldDrop;
            private int _level;
            private int _expDrop;
            private PrimaryStatType _primaryStat;
            private Resistances _resistances = new Resistances();
            private List<DOTEffect> _activeDoTs = new List<DOTEffect>();

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public int Health { get => _health; set => _health = value; }
            public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
            public int Mana { get => _mana; set => _mana = value; }
            public int MaxMana { get => _maxMana; set => _maxMana = value; }
            public Stats Stats { get => _stats; [MemberNotNull(nameof(_stats))] set => _stats = value; }
            public int Initiative { get => _initiative; set => _initiative = value; }
            public Move[] MoveSet { get => _moveSet; [MemberNotNull(nameof(_moveSet))] set => _moveSet = value; }
            public LootTableItem[] LootTable { get => _lootTable; [MemberNotNull(nameof(_lootTable))] set => _lootTable = value; }
            public int GoldDrop { get => _goldDrop; set => _goldDrop = value; }
            public int Level { get => _level; set => _level = value; }
            public int ExpDrop { get => _expDrop; set => _expDrop = value; }
            public Resistances Resistances { get => _resistances; [MemberNotNull(nameof(_resistances))] set => _resistances = value; }
            public List<DOTEffect> ActiveDoTs { get => _activeDoTs; [MemberNotNull(nameof(_activeDoTs))] set => _activeDoTs = value; }
            public int PrimaryStat
            {
                get =>
                _primaryStat switch
                {
                    PrimaryStatType.Strength => this.Stats.Strength,
                    PrimaryStatType.Dexterity => this.Stats.Dexterity,
                    PrimaryStatType.Intelligence => this.Stats.Intelligence,
                    _ => 0
                };
            }
            public int SetInitiative()
            {
                Initiative = (Stats.Dexterity - 10) / 2;
                return Initiative;
            }
            public NPC Clone()
            {
                return new NPC(Name, Description, _primaryStat, Stats, Level, MoveSet, LootTable, GoldDrop);
            }
            public NPC(string Name, string Description, PrimaryStatType PrimaryStat, Stats Stats, int Level, Move[] MoveSet, LootTableItem[] LootTable, int GoldDrop = 0)
            {
                this.Name = Name;
                this.Description = Description;
                this._primaryStat = PrimaryStat;
                this.MoveSet = MoveSet;
                foreach (Move move in this.MoveSet)
                {
                    move.Description = move.Description.Replace("__", this.Name);
                }
                this.Health = Stats.Constitution * 10;
                this.Mana = Stats.Intelligence * 10;
                this.MaxHealth = this.Health;
                this.MaxMana = this.Mana;
                this.LootTable = LootTable;
                this.GoldDrop = GoldDrop;
                this.Level = Level;
                //NPC Scaling
                float scaleFactor = 1.0f + ((Level - 1) * 0.1f); // +10% stats per level
                this._stats = new Stats(
                    (int)(Stats.Strength * scaleFactor),
                    (int)(Stats.Dexterity * scaleFactor),
                    (int)(Stats.Intelligence * scaleFactor),
                    (int)(Stats.Constitution * scaleFactor),
                    (int)(Stats.Wisdom * scaleFactor)
                );
                ;
                this.ExpDrop = (int)(100 * Math.Pow(1.07, this.Level)); // EXP DROP FORMULA
            }
        }

        static Move Bite = new Move("Bite", "The __ sinks its teeth into you", 12, 0);
        static Move Club = new Move("Club", "The __ swings a wooden club", 15, 0);
        static Move Tackle = new Move("Tackle", "The __ tackles you", 10, 0);

        //Goblin
        static Move[] GoblinMoves = { Club };
        static LootTableItem[] GoblinLoot = { new LootTableItem(ItemName.HealthPotion, 100), new LootTableItem(ItemName.ManaPotion, 100), new LootTableItem(ItemName.RustySword, 100) };
        static Stats GoblinStats = new Stats(14/*STR*/, 8/*DEX*/, 8/*INT*/, 12/*CON*/, 8/*WIS*/);
        static NPC Goblin = new NPC("Goblin", "A small green creature", PrimaryStatType.Strength, GoblinStats, 1/*LVL*/, GoblinMoves, GoblinLoot, 5/*Gold*/);

        static Move[] WolfMoves = { Bite };
        static LootTableItem[] WolfLoot = { new LootTableItem(ItemName.HealthPotion, 100), new LootTableItem(ItemName.ManaPotion, 100), new LootTableItem(ItemName.RustySword, 100) };
        static Stats WolfStats = new Stats(14/*STR*/, 8/*DEX*/, 8/*INT*/, 12/*CON*/, 8/*WIS*/);
        static NPC Wolf = new NPC("Wolf", "A hungry forest predator", PrimaryStatType.Dexterity, WolfStats, 1/*LVL*/, WolfMoves, WolfLoot, 0/*Gold*/);
    }
}


/* Handle Enemy EXP
  Option 1: Based on Enemy Level (most common)
  // Enemy drops EXP roughly equal to what player needs at that level
  expDrop = (int)(100 * Math.Pow(1.07, enemyLevel))

  // Or a fraction if you want multiple kills per level
  expDrop = (int)(50 * Math.Pow(1.07, enemyLevel))  // ~2 kills per level
  expDrop = (int)(33 * Math.Pow(1.07, enemyLevel))  // ~3 kills per level

  Option 2: Fixed percentage of player's current requirement
  // Enemy gives 20-30% of what player needs, based on player level
  expDrop = (int)(100 * Math.Pow(1.07, playerLevel) * 0.25)

  Option 3: Differential scaling (risk/reward for fighting higher level enemies)
  int levelDiff = enemyLevel - playerLevel;
  float multiplier = 1.0f + (levelDiff * 0.1f); // +10% per level difference
  expDrop = (int)(100 * Math.Pow(1.07, enemyLevel) * multiplier);
*/