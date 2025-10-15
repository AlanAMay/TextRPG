namespace TextRPGOne
{
    partial class Program
    {

        class LootTableItem
        {
            private ItemType _name;
            private float _dropRate;

            public ItemType Name { get => _name; }
            public float DropPercent { get => _dropRate; }
            public LootTableItem(ItemType Name, float DropRate)
            {
                this._name = Name;
                this._dropRate = DropRate;
            }
        }
    }

}
