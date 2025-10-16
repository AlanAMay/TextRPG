namespace TextRPGOne
{
    partial class Program
    {

        class LootTableItem
        {
            private ItemName _name;
            private float _dropRate;

            public ItemName Name { get => _name; }
            public float DropPercent { get => _dropRate; }
            public LootTableItem(ItemName Name, float DropRate)
            {
                this._name = Name;
                this._dropRate = DropRate;
            }
        }
    }

}
