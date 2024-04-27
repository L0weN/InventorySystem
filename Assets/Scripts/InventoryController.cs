using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mert.Inventory
{
    public class InventoryController
    {
        readonly InventoryView view;
        readonly InventoryModel model;
        readonly int capacity;

        InventoryController(InventoryView view, InventoryModel model, int capacity)
        {
            Debug.Assert(view != null, "View is null");
            Debug.Assert(model != null, "Model is null");
            Debug.Assert(capacity > 0, "Capacity is less than 1");
            this.view = view;
            this.model = model;
            this.capacity = capacity;

            view.StartCoroutine(Initialize());
        }

        IEnumerator Initialize()
        {
            yield return view.InitializeView(capacity);

            view.OnDrop += HandleDrop;
            model.OnModelChanged += HandleModelChanged;

            RefreshView();
        }

        void HandleDrop(Slot originalSlot, Slot closestSlot)
        {
            if (originalSlot.Index == closestSlot.Index || closestSlot.ItemId.Equals(SerializableGuid.Empty))
            {
                model.Swap(originalSlot.Index, closestSlot.Index);
                return;
            }

            var sourceItemId = model.Items[originalSlot.Index].details.Id;
            var targetItemId = model.Items[closestSlot.Index].details.Id;

            if (sourceItemId.Equals(targetItemId) && model.Items[closestSlot.Index].details.maxStack > 1)
            {
                model.Combine(originalSlot.Index, closestSlot.Index);
            }
            else
            {
                model.Swap(originalSlot.Index, closestSlot.Index);
            }
        }

        void HandleModelChanged(IList<Item> items) => RefreshView();

        void RefreshView()
        {
            for (int i = 0; i < capacity; i++)
            {
                var item = model.Get(i);
                if (item == null)
                {
                    view.Slots[i].Set(SerializableGuid.Empty, null);
                }
                else
                {
                    view.Slots[i].Set(item.Id, item.details.Icon, item.quantity);
                }
            }
        }

        public class Builder
        {
            InventoryView view;
            IEnumerable<ItemDetails> itemDetails;
            int capacity = 20;

            public Builder(InventoryView view)
            {
                this.view = view;
            }

            public Builder WithStartingItems(IEnumerable<ItemDetails> itemDetails)
            {
                this.itemDetails = itemDetails;
                return this;
            }

            public Builder WithCapacity(int capacity)
            {
                this.capacity = capacity;
                return this;
            }

            public InventoryController Build()
            {
                InventoryModel model = itemDetails != null ? new InventoryModel(itemDetails, capacity) : new InventoryModel(Array.Empty<ItemDetails>(), capacity);

                return new InventoryController(view, model, capacity);
            }
        }
    }
}