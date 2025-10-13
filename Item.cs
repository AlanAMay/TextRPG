using System.Diagnostics.CodeAnalysis;

namespace TextRPGOne
{
    partial class Program
    {
        class Item
        {
            private string _itemName;
            private string _itemDescription;
            private int _itemCode;

            public string ItemName { get => _itemName; [MemberNotNull(nameof(_itemName))] set => _itemName = value; }
            public string ItemDescription { get => _itemDescription; [MemberNotNull(nameof(_itemDescription))] set => _itemDescription = value; }
            public int ItemCode { get => _itemCode; set => _itemCode = value; }

            public Item(string ItemName, string ItemDescription)
            {
                this.ItemName = ItemName;
                this.ItemDescription = ItemDescription;
            }
            public Item Clone()
            {
                return new Item(ItemName, ItemDescription);
            }
        }
        static void OpenInventory()
        {

        }

    }
}