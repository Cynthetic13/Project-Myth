using System;
using UnityEngine;
using UnityEngine.UI;

namespace Myth.UI
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private GameObject _hotbarSlots;
        [SerializeField] private Sprite _slotSprite;

        private void Start()
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject _newSlot = Instantiate(new GameObject($"Slot {i}"), _hotbarSlots.transform);
                Image _sprite = _newSlot.AddComponent<Image>();
                _sprite.sprite = _slotSprite;
            }
        }
    }
}