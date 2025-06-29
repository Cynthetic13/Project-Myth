using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Myth.Game;
using Myth.World;
using Myth.World.Blocks;
using TMPro;

namespace Myth.UI
{
    public class Hotbar : MonoBehaviour
    {
        public GameObject[] hotbarSlots;
        public RectTransform hotbarCursor;
        public Sprite emptySprite;
        public TextMeshProUGUI selectionText;
        public bool useSetItems = true;
        public float cursorSpeed = 10.0f;
        public float fadeDuration = 1.0f;
        public float displayTime = 2.0f;

        private readonly List<Image> _hotbarImages = new();
        private readonly List<TextMeshProUGUI> _slotQualityLabel = new();
        private readonly HotbarSlot[] _slots = new HotbarSlot[10];
        private Vector2 _targetCursor = Vector2.zero;
        private Vector2 _lerpCursor = Vector2.zero;
        private Player _player;
        
        private CanvasGroup _selectionCanvasGroup;
        private Coroutine _selectionCoroutine;
        private Coroutine _fadeCoroutine;

        private void Start()
        {
            if (!Camera.main)
            {
                Debug.LogError("Camera.main is null, please set the main camera!");
                return;
            }
            
            // Get all hotbar images and quantity text
            for (int i = 0; i < hotbarSlots.Length; i++)
            {
                _hotbarImages.Add(hotbarSlots[i].GetComponentsInChildren<Image>()[1]);
                _slotQualityLabel.Add(hotbarSlots[i].GetComponentInChildren<TextMeshProUGUI>());
                
                _hotbarImages[i].sprite = emptySprite;
                _slotQualityLabel[i].text = "";
            }
            
            // Get CanvasGroup for the Selection Text fading
            _selectionCanvasGroup = selectionText.GetComponent<CanvasGroup>();
            if (_selectionCanvasGroup == null)
                _selectionCanvasGroup = selectionText.gameObject.AddComponent<CanvasGroup>();

            _selectionCanvasGroup.alpha = 0.0f; // Set Invisible
            
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            if (!useSetItems) return;
            
            SetHotbarBlock(0, 1, 32);
            SetHotbarBlock(1, 2, 16);
            SetHotbarBlock(2, 3, 64);
            SetHotbarBlock(3, 4, 8);
            SetHotbarBlock(4, 5, 1);
            
            SelectSlot(0);
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
            if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);
            if (Input.GetKeyDown(KeyCode.Alpha6)) SelectSlot(5);
            if (Input.GetKeyDown(KeyCode.Alpha7)) SelectSlot(6);
            if (Input.GetKeyDown(KeyCode.Alpha8)) SelectSlot(7);
            if (Input.GetKeyDown(KeyCode.Alpha9)) SelectSlot(8);
            if (Input.GetKeyDown(KeyCode.Alpha0)) SelectSlot(9);
        }

        private void SelectSlot(int index)
        {
            byte selectedBlockID = _slots[index].BlockID;
            
            _targetCursor.x = index * 64;
            _player.SetHotbarBlock(selectedBlockID);
            
            if (_selectionCoroutine != null)
            {
                StopCoroutine(_selectionCoroutine);
            }

            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            
            _selectionCoroutine = StartCoroutine(UpdateCursor());
            
            if (_slots[index].SetDisplayName(selectedBlockID) == "") return;
            _fadeCoroutine = StartCoroutine(FadeSelectionText(_slots[index].SetDisplayName(selectedBlockID)));
        }

        private IEnumerator UpdateCursor()
        {
            Vector2 startPosition = _lerpCursor;
            float timeElapsed = 0.0f;

            while (timeElapsed < cursorSpeed)
            {
                _lerpCursor = Vector2.Lerp(startPosition, _targetCursor, timeElapsed / cursorSpeed);
                hotbarCursor.anchoredPosition = _lerpCursor;
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            
            hotbarCursor.anchoredPosition = _targetCursor;
        }

        private void SetHotbarBlock(int index, byte blockID, int quantity = 1)
        {
            _slots[index] = new HotbarSlot(blockID, quantity);
            
            _hotbarImages[index].sprite = BlockRegistry.GetHotbarIcon(blockID) ?? emptySprite;
            _slotQualityLabel[index].text = quantity > 1 ? quantity.ToString() : "";
        }

        private IEnumerator FadeSelectionText(string text)
        {
            selectionText.text = text;
            yield return StartCoroutine(FadeText(1.0f));

            yield return new WaitForSeconds(displayTime);

            yield return StartCoroutine(FadeText(0.0f));
        }

        private IEnumerator FadeText(float targetAlpha)
        {
            float startAlpha = _selectionCanvasGroup.alpha;
            float timeElapsed = 0.0f;

            while (timeElapsed < fadeDuration)
            {
                _selectionCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timeElapsed / fadeDuration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _selectionCanvasGroup.alpha = targetAlpha;
        }
    }
}