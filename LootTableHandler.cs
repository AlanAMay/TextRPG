namespace TextRPGOne
{
    partial class Program
    {
        class LootTableHandler
        {
            private static Random random = new Random();
            //Needs to return what loot is dropped based on the available items from the loot table
            public static List<Item> DropItems(LootTableItem[] items)
            {
                List<Item> DroppedItems = new List<Item>();
                foreach (LootTableItem item in items)
                {
                    //Pull the random roll randomOneHundred
                    //If the items float value is <= the float, add that item to a List of a new LootTableItem<> called DroppedItems

                    float dropRoll = random.Next(101);
                    Console.WriteLine($"Drop Roll{dropRoll}");
                    Console.WriteLine($"Item Name{item.Name.ToString()}");
                    Console.WriteLine($"Item Drop Rate{item.DropPercent}");
                    if (dropRoll <= item.DropPercent)
                    {
                        DroppedItems.Add(ItemDatabase.Get(item.Name));
                    }
                }
                return DroppedItems;
            }
        }
    }

}
