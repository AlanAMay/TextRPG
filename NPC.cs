using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        public enum PrimaryStatType
        {
            Strength,
            Dexterity,
            Intelligence
        }
        class NPC
        {
            private string _name;
            private string _description;
            private int _health;
            private int _maxHealth;
            private int _mana;
            private int _maxMana;
            private int _strength;
            private int _dexterity;
            private int _intelligence;
            private int _constitution;
            private int _initiative;
            private Move[] _moveSet;
            private ItemType[] _lootTable;
            private int _goldDrop;
            private PrimaryStatType _primaryStat;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public int Health { get => _health; set => _health = value; }
            public int MaxHealth { get => _maxHealth; set => _maxHealth = value; }
            public int Mana { get => _mana; set => _mana = value; }
            public int MaxMana { get => _maxMana; set => _maxMana = value; }
            public int Strength { get => _strength; set => _strength = value; }
            public int Dexterity { get => _dexterity; set => _dexterity = value; }
            public int Intelligence { get => _intelligence; set => _intelligence = value; }
            public int Constitution { get => _constitution; set => _constitution = value; }
            public int Initiative { get => _initiative; set => _initiative = value; }
            public Move[] MoveSet { get => _moveSet; [MemberNotNull(nameof(_moveSet))] set => _moveSet = value; }
            public ItemType[] LootTable { get => _lootTable; [MemberNotNull(nameof(_lootTable))] set => _lootTable = value; }
            public int GoldDrop { get => _goldDrop; set => _goldDrop = value; }
            public int PrimaryStat
            {
                get =>
                _primaryStat switch
                {
                    PrimaryStatType.Strength => this.Strength,
                    PrimaryStatType.Dexterity => this.Dexterity,
                    PrimaryStatType.Intelligence => this.Intelligence,
                    _ => 0
                };
            }
            public int SetInitiative()
            {
                Initiative = (Dexterity - 10) / 2;
                return Initiative;
            }
            public NPC Clone()
            {
                return new NPC(Name, Description, _primaryStat, Strength, Dexterity, Intelligence, Constitution, MoveSet, LootTable, GoldDrop);
            }
            public NPC(string Name, string Description, PrimaryStatType PrimaryStat, int Strength, int Dexterity, int Intelligence, int Constitution, Move[] MoveSet, ItemType[] LootTable, int GoldDrop = 0)
            {
                this.Name = Name;
                this.Description = Description;
                this._primaryStat = PrimaryStat;
                this.Strength = Strength;
                this.Dexterity = Dexterity;
                this.Intelligence = Intelligence;
                this.Constitution = Constitution;
                this.MoveSet = MoveSet;
                foreach (Move move in this.MoveSet)
                {
                    move.Description = move.Description.Replace("__", this.Name);
                }
                this.Health = Constitution * 10;
                this.Mana = Intelligence * 10;
                this.MaxHealth = this.Health;
                this.MaxMana = this.Mana;
                this.LootTable = LootTable;
                this.GoldDrop = GoldDrop;

            }
        }

        static Move Bite = new Move("Bite", "The __ sinks its teeth into you", 12, 0);
        static Move Club = new Move("Club", "The __ swings a wooden club", 15, 0);
        static Move Tackle = new Move("Tackle", "The __ tackles you", 10, 0);


        static Move[] goblinMoves = { Club };
        static ItemType[] goblinLoot = { ItemType.HealthPotion, ItemType.Rock };
        static NPC Goblin = new NPC("Goblin", "A small green creature", PrimaryStatType.Strength, 10/*STR*/, 6/*DEX*/, 4/*INT*/, 10/*CON*/, goblinMoves, goblinLoot, 5/*Gold*/);
        static Move[] wolfMoves = { Bite };
        static Item[] wolfLoot = new Item[0];
        static NPC Wolf = new NPC("Wolf", "A hungry forest predator", PrimaryStatType.Dexterity, 6/*STR*/, 12/*DEX*/, 4/*INT*/, 6/*CON*/, wolfMoves, wolfLoot, 0/*Gold*/);
    }
}