using UnityEngine;
using Myth.World;
using Myth.World.Blocks;

namespace Myth.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private BlockScriptableObject[] blocks;
        private GameState _gameState;
        
        private void Awake()
        {
            BlockRegistry.Initialize(blocks);
            SetGameState(GameState.MainMenu);
        }

        public void SetGameState(GameState state)
        {
            _gameState = state;

            switch (_gameState)
            {
                case GameState.MainMenu:
                    SetCursorState(true, 2);
                    return;
                case GameState.Playing:
                    SetCursorState(false, 1);
                    return;
            }
        }
        
        public GameState GetGameState() => _gameState;

        private void SetCursorState(bool state, int lockState)
        {
            Cursor.visible = state;
            Cursor.lockState = (CursorLockMode)lockState;
        }

        public enum GameState
        {
            MainMenu,
            Playing,
            Inventory,
            Options,
            Dead
        }
    }
}