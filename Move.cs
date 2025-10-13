using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        class Move
        {
            private string _name;
            private string _description;
            private int _damage;
            private int _manaCost;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public int Damage { get => _damage; set => _damage = value; }
            public int ManaCost { get => _manaCost; set => _manaCost = value; }

            public Move(string Name, string Description, int Damage, int ManaCost)
            {
                this.Name = Name;
                this.Description = Description;
                this.Damage = Damage;
                this.ManaCost = ManaCost;
            }
        }
    }
}