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
        class CharacterSpec
        {
            private string _name;
            private string _description;
            private int _health;
            private int _mana;
            private int _strength;
            private int _dexterity;
            private int _intelligence;
            private int _constitution;
            private PrimaryStatType _primaryStat;
            private Move[] _moveSet;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public int Health { get => _health; set => _health = value; }
            public int Mana { get => _mana; set => _mana = value; }
            public int Strength { get => _strength; set => _strength = value; }
            public int Dexterity { get => _dexterity; set => _dexterity = value; }
            public int Intelligence { get => _intelligence; set => _intelligence = value; }
            public int Constitution { get => _constitution; set => _constitution = value; }
            public PrimaryStatType PrimaryStat { get => _primaryStat; }
            public Move[] MoveSet { get => _moveSet; [MemberNotNull(nameof(_moveSet))] set => _moveSet = value; }

            public CharacterSpec(string Name, string Description, PrimaryStatType PrimaryStat, int Strength, int Dexterity, int Intelligence, int Constitution, Move[] MoveSet)
            {
                this.Name = Name;
                this.Description = Description;
                this._primaryStat = PrimaryStat;
                this.Strength = Strength;
                this.Dexterity = Dexterity;
                this.Intelligence = Intelligence;
                this.Constitution = Constitution;
                this.MoveSet = MoveSet;
                this.Health = 100 + Constitution * 10;
                this.Mana = 100 + Intelligence * 10;
            }
        }
        static Move Slash = new Move("Slash", "You swing your sword", 20, 0);
        static Move Block = new Move("Block", "You raise your sheild", 0, 0);
        static Move[] WarriorMoveSet = {
            Slash,
            Block
        };
        static CharacterSpec Warrior = new CharacterSpec("Warrior", "The Warrior is a stout class with decent damage", PrimaryStatType.Strength, 14/*STR*/, 8/*DEX*/, 6/*INT*/, 10/*CON*/, WarriorMoveSet);

        static Move Stab = new Move("Stab", "You gouge them with your blade", 28, 0);
        static Move Dodge = new Move("Dodge", "You try to avoid the next attack", 0, 0);
        static Move[] RogueMoveSet = {
            Stab,
            Dodge
        };
        static CharacterSpec Rogue = new CharacterSpec("Rogue", "The Rogue is a high damage class with low health", PrimaryStatType.Dexterity, 6/*STR*/, 16/*DEX*/, 8/*INT*/, 8/*CON*/, RogueMoveSet);

        static Move Fireball = new Move("Fireball", "You hurl a ball of fire", 35, 20);
        static Move Barrier = new Move("Barrier", "You cast a barrier around yourself", 0, 35);
        static Move[] MageMoveSet = {
            Fireball,
            Barrier
        };
        static CharacterSpec Mage = new CharacterSpec("Mage", "The Mage is busted", PrimaryStatType.Intelligence, 8/*STR*/, 8/*DEX*/, 16/*INT*/, 6/*CON*/, MageMoveSet);
    }
}