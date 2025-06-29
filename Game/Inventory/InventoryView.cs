using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Myth.Game.Inventory
{
    public class InventoryView : StorageView
    {
        [SerializeField] private string panelName = "Inventory";
        
        public override IEnumerator InitializeView(int size = 6)
        {
            Slots = new Slot[size];
            Root = document.rootVisualElement;
            Root.Clear();

            Root.styleSheets.Add(styleSheet);
            
            Container = Root.CreateChild("container");

            var inventory = Container.CreateChild("inventory");
            inventory.CreateChild("inventoryFrame");
            inventory.CreateChild("inventoryHeader").Add(new Label(panelName));

            var slotsContainer = inventory.CreateChild("slotsContainer");

            for (int i = 0; i < size; i++)
            {
                var slot = slotsContainer.CreateChild<Slot>("slot");
                Slots[i] = slot;
            }

            ghostIcon = Container.CreateChild("ghostIcon");
            ghostIcon.BringToFront();
            
            yield return null;
        }
    }
}