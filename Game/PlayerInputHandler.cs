using UnityEngine;
using UnityEngine.InputSystem;

namespace Myth.Game
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool AttackInput { get; private set; }
        public bool PlaceInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool IsController { get; private set; }
        
        private InputSystem_Actions _input;
        private PlayerInput _playerInput;

        private void Awake()
        {
            _input = new InputSystem_Actions();
            _playerInput = GetComponent<PlayerInput>();

            if (_playerInput.defaultControlScheme == "MnK")
                IsController = false;
            else if (_playerInput.defaultControlScheme == "Controller")
                IsController = true;

            _input.Player.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
            _input.Player.Move.canceled += ctx => MoveInput = Vector2.zero;
            
            _input.Player.Look.performed += ctx =>
            {
                var value = ctx.ReadValue<Vector2>();

                if (ctx.control.device is Mouse)
                    value.y *= -1;

                LookInput = value;
            };
            _input.Player.Look.canceled += ctx => LookInput = Vector2.zero;

            _input.Player.Attack.performed += _ => AttackInput = true;
            _input.Player.Attack.canceled += _ => AttackInput = false;
            
            _input.Player.Place.performed += _ => PlaceInput = true;
            _input.Player.Place.canceled += _ => PlaceInput = false;

            _input.Player.Jump.performed += _ => JumpInput = true;
            _input.Player.Jump.canceled += ctx => JumpInput = false;
        }

        private void LateUpdate()
        {
            AttackInput = false;
            PlaceInput = false;
        }

        private void OnEnable() => _input.Enable();
        private void OnDisable() => _input.Disable();
    }
}