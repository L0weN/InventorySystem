using System;

namespace Mert.Inventory
{
    [Serializable]
    public class Item
    {
        public SerializableGuid Id;
        public ItemDetails details;
        public int quantity;

        public Item(ItemDetails details, int quantity)
        {
            Id = SerializableGuid.NewGuid();
            this.details = details;
            this.quantity = quantity;
        }
    }
}