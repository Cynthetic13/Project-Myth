using UnityEngine;
using UnityEngine.UIElements;

namespace Myth.Game.Inventory
{
    public class Slot : VisualElement
    {
        /*public Image Icon;
        public Label StackLabel;
        public int Index => parent.IndexOf(this);
        public SerializableGuid ItemId { get; private set; } = SerializableGuid.Empty;
        public Sprite BaseSprite;

        public Slot()
        {
            Icon = this.CreateChild<Image>("slotIcon");
            StackLabel = this.CreateChild("slotFrame").CreateChild<Label>("stackCount");
        }

        public void Set(SerializableGuid id, Sprite icon, int quanity = 0)
        {
            ItemId = id;
            BaseSprite = icon;
            
            Icon.image = BaseSprite != null ? icon.texture : null;
            StackLabel.text = quanity > 1 ? quanity.ToString() : string.Empty;
            StackLabel.visible = quanity > 1;
        }

        public void Remove()
        {
            ItemId = SerializableGuid.Empty;
            Icon.image = null;
        }*/
    }
}