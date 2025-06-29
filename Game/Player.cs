using System;
using UnityEngine;
using Myth.World;
using Myth.World.Blocks;
using UnityEngine.Rendering.Universal;

namespace Myth.Game
{
    // TODO... Switch Movement to another script that gets events instead of bool flags
    public class Player : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float mouseSensitivity = 1.0f;
        [SerializeField] private float controllerSensitivity = 1.0f;
        [SerializeField] private float jumpHeight = 2.0f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private Transform cameraTransform;
        
        [SerializeField] private GameObject selectionBox;
        [SerializeField] private float selectionRange = 6.0f;
        [SerializeField] private float currentTemp = 20f;
        [SerializeField] private FullScreenPassRendererFeature freezingData;
        [SerializeField] private Material freezingMaterial;
        [SerializeField] private FullScreenPassRendererFeature overheatingData;
        [SerializeField] private Material overheatingMaterial;
        [SerializeField] private AudioSource freezingAudio;
        
        private CharacterController _characterController;
        private Vector3 _velocity;
        private bool _isGrounded;

        private PlayerInputHandler _input;
        private float _cameraSensitivity;
        
        private Material _selectionMaterial;
        
        private byte _setBlock;
        private byte _airBlock;
        private Vector2 _rotation;

        private float freezingAmount = 25f;
        private float overheatingAmount = 0.001f;
        private bool _isFreezing = false;
        
        private void Start()
        {
            _input = GetComponent<PlayerInputHandler>();
            _characterController = GetComponent<CharacterController>();
            
            _selectionMaterial = selectionBox.GetComponent<Renderer>().sharedMaterial;
            
            _airBlock = 0;

            _cameraSensitivity = !_input.IsController ? mouseSensitivity : controllerSensitivity;
        }
        
        private void Update()
        {
            RaycastHit hit = new RaycastHit();
            bool ray = Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, selectionRange);

            // Ground Check
            _isGrounded = _characterController.isGrounded;

            // Stick if grounded
            if (_isGrounded && _velocity.y < 0)
                _velocity.y = 0f;
            
            // Selection Box
            if (ray)
            {
                selectionBox.transform.position = World.Terrain.GetBlockWorldPosition(hit);
                _selectionMaterial.color = new Color(1, 1, 1, 1);
            }
            else
                _selectionMaterial.color = new Color(1, 1, 1, 0);
            
            // Break Block
            if (_input.AttackInput && ray)
                    World.Terrain.SetBlock(hit, _airBlock);
            
            // Place Block
            if (_input.PlaceInput && ray)
                World.Terrain.SetBlock(hit, _setBlock, true);
            
            // Get Rotation
            _rotation = new Vector2(
                _rotation.x + _input.LookInput.x * _cameraSensitivity,
                _rotation.y + _input.LookInput.y * _cameraSensitivity);

            // Clamp Y Rotation
            _rotation.y = Mathf.Clamp(_rotation.y, -90f, 90f);

            // Player Rotation
            transform.localRotation = Quaternion.Euler(0, _rotation.x, 0);
            cameraTransform.localRotation = Quaternion.Euler(_rotation.y, 0, 0);

            // Player Movement
            Vector3 move = transform.right * _input.MoveInput.x + transform.forward * _input.MoveInput.y;
            move = Vector3.ClampMagnitude(move, 1.0f);
            
            // Jumping
            if (_input.JumpInput && _isGrounded)
                _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            
            // Gravity
            _velocity.y += gravity * Time.deltaTime;
            
            // Combine and Move
            Vector3 finalMove = (move * moveSpeed) + (_velocity.y * Vector3.up);
            _characterController.Move(finalMove * Time.deltaTime);
            
            // Temp System
            if (currentTemp > 32)
            {
                overheatingData.SetActive(true);
                
                overheatingAmount = Mathf.Lerp(overheatingAmount, 0.05f, (currentTemp - 32) * Time.deltaTime);
                overheatingMaterial.SetFloat("_Power", overheatingAmount);
            }
            else if (currentTemp < 0)
            {
                freezingData.SetActive(true);

                freezingAmount = Mathf.Lerp(freezingAmount, 5.0f, -(currentTemp) * Time.deltaTime);
                freezingMaterial.SetFloat("_Power", freezingAmount);
                if (!_isFreezing)
                {
                    freezingAudio.Play();
                    _isFreezing = true;
                }
            }
            else
            {
                freezingAmount = Mathf.Lerp(freezingAmount, 25f, (currentTemp) * Time.deltaTime);
                overheatingAmount = Mathf.Lerp(overheatingAmount, 0, -(currentTemp - 32) * Time.deltaTime);
                overheatingMaterial.SetFloat("_Power", overheatingAmount);
                freezingMaterial.SetFloat("_Power", freezingAmount);

                if (freezingAmount > 24f)
                {
                    freezingData.SetActive(false);
                }

                if (overheatingAmount < 0.0025)
                {
                    overheatingData.SetActive(false);
                }
                
                freezingAudio.Stop();
                _isFreezing = false;
            }
        }

        private void OnDisable()
        {
            overheatingData.SetActive(false);
            freezingData.SetActive(false); 
        }

        public void SetHotbarBlock(byte blockID) => _setBlock = blockID;
    }
}