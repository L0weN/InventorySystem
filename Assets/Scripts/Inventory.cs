using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mert.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] InventoryView view;
        [SerializeField] int capacity = 20;
        [SerializeField] List<ItemDetails> startingItems = new List<ItemDetails>();

        InventoryController controller;

        private void Awake()
        {
            controller = new InventoryController.Builder(view).WithStartingItems(startingItems).WithCapacity(capacity).Build();
        }
    }
}