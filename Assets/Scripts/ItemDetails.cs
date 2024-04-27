using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mert.Inventory
{
    [CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item", order =1)]
    public class ItemDetails : ScriptableObject
    {
        public string Name;
        public int maxStack = 1;
        public SerializableGuid Id = SerializableGuid.NewGuid();

        private void AssignNewGuid()
        {
            Id = SerializableGuid.NewGuid();
        }

        public Sprite Icon;

        public string Description;

        public Item Create(int quantity)
        {
            return new Item(this, quantity);
        }
    }
}