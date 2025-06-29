using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Myth.Game.Inventory
{
    public abstract class StorageView : MonoBehaviour
    {
        public Slot[] Slots;

        [SerializeField] protected UIDocument document;
        [SerializeField] protected StyleSheet styleSheet;

        protected static VisualElement ghostIcon;

        protected VisualElement Root;
        protected VisualElement Container;

        private IEnumerator Start()
        {
            yield return StartCoroutine(InitializeView());
        }

        public abstract IEnumerator InitializeView(int size = 6);
    }
}