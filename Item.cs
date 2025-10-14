using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        class Item
        {
            private string _name;
            private string _description;
            private Guid _id;

            public string Name { get => _name; [MemberNotNull(nameof(_name))] set => _name = value; }
            public string Description { get => _description; [MemberNotNull(nameof(_description))] set => _description = value; }
            public Guid Id { get => _id; }
            public Item(string ItemName, string ItemDescription)
            {
                this.Name = ItemName;
                this.Description = ItemDescription;
                Guid newUuid = Guid.NewGuid();
                this._id = newUuid;
            }
        }
        enum ItemType
        {
            HealthPotion,
            ManaPotion,
            RustySword,
            WoodenStaff,
            RustyDagger,
            Rock
        }
        class ItemDatabase
        {
            public static Dictionary<ItemType, Item> _definitions = new()
            {
                {ItemType.HealthPotion , new Item("Health Potion", "A small vial to restore health") },
                {ItemType.ManaPotion , new Item("Mana Potion", "A small vial to restore mana")},
                {ItemType.RustySword , new Item("Rusty Sword", "An old rusty sword")},
                {ItemType.WoodenStaff , new Item("Wooden Staff", "A old wood stick")},
                {ItemType.RustyDagger , new Item("Rusty Dagger", "A small rusted blade") },
                {ItemType.Rock , new Item("Rock", "It's a rock")}
            };
            public static Item Get(ItemType type) => _definitions[type];
        }

    }
}