namespace TextRPGOne
{
    partial class Program
    {
        struct Stats
        {
            private int _strength;
            private int _dexterity;
            private int _intelligence;
            private int _constitution;
            private int _wisdom;
            private int _luck;
            public int Strength { get => _strength; set => _strength = value; }
            public int Dexterity { get => _dexterity; set => _dexterity = value; }
            public int Intelligence { get => _intelligence; set => _intelligence = value; }
            public int Constitution { get => _constitution; set => _constitution = value; }
            public int Wisdom { get => _wisdom; set => _wisdom = value; }
            public int Luck { get => _luck; set => _luck = value; }
            public Stats(int Strength, int Dexterity, int Intelligence, int Constitution, int Wisdom)
            {
                this.Strength = Strength;
                this.Dexterity = Dexterity;
                this.Intelligence = Intelligence;
                this.Constitution = Constitution;
                this.Wisdom = Wisdom;
            }
            public Stats(int Strength, int Dexterity, int Intelligence, int Constitution, int Wisdom, int Luck)
            {
                this.Strength = Strength;
                this.Dexterity = Dexterity;
                this.Intelligence = Intelligence;
                this.Constitution = Constitution;
                this.Wisdom = Wisdom;
                this.Luck = Luck;
            }
        }

    }
}