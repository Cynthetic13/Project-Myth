using System;
using UnityEngine;
using TMPro;

namespace Myth.Misc
{
    public class InfoDisplay : MonoBehaviour
    {
        private float _deltaTime = 0.0f;
        private int _seed;
        private Transform _player;
        private TextMeshProUGUI _displayText;

        private void Start()
        {
            _displayText = GetComponent<TextMeshProUGUI>();
            _seed = World.World.Instance.seed;
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
            
            Vector3Int position = new Vector3Int(Mathf.FloorToInt(_player.position.x), Mathf.FloorToInt(_player.position.y), Mathf.FloorToInt(_player.position.z));
            
            float msec = _deltaTime * 1000.0f;
            float fps = 1.0f / _deltaTime;
            _displayText.text = $"Coords: ({position.x}, {position.y}, {position.z})\nFPS: {fps:0.}\nms: {msec:0.00}\nSeed: {_seed}";
        }
    }
}