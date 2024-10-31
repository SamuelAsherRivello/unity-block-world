using RMC.BlockWorld.Mini.Model.Data;
using UnityEngine;
using UnityEngine.InputSystem;

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace RMC.BlockWorld.Standard.Objects
{
    /// <summary>
    /// Represents the 3D Graphics in front of the <see cref="Environment"/>
    /// </summary>
    public class Player : MonoBehaviour
    {
        //  Properties ------------------------------------

        public CharacterData CharacterData
        {
            get { return _characterData; }
            set
            {
                _characterData = value;

                if (_head == null)
                {
                    return;
                }
                CustomColorUtility.SetColorAsync(_head.material, _characterData.HeadColor, CustomColorUtility.DefaultDuration);
                CustomColorUtility.SetColorAsync(_chest.material, _characterData.ChestColor, CustomColorUtility.DefaultDuration);
                CustomColorUtility.SetColorAsync(_legs.material, _characterData.LegsColor, CustomColorUtility.DefaultDuration);
            }
        }

        
        //  Fields ----------------------------------------

        public bool IsPlayerEnabled { get; set; } = false;

        [SerializeField]
        private Renderer _head;

        [SerializeField]
        private Renderer _chest;

        [SerializeField]
        private Renderer _legs;

        [SerializeField]
        private float _angularSpeed = 100f;

        [SerializeField]
        private float _linearSpeed = 5f;

        private CharacterData _characterData;
        private float _currentMovementSpeed = 0f;
        private float _currentRotationSpeed = 0f;
        private const float MovementSmoothingTime = 0.125f;
        private const float RotationSmoothingTime = 0.125f;
        
        // Input
        private InputAction _playerMoveInputAction;

        //  Unity Methods ---------------------------------
        
        protected void Awake()
        {
            _playerMoveInputAction = InputSystem.actions.FindAction("Move");
        }

        private void Update()
        {
            if (!IsPlayerEnabled)
            {
                return;
            }
            
            HandleMovement();
        }

        
        protected void OnEnable()
        {
            _playerMoveInputAction.Enable();
        }
        
        
        protected void OnDisable()
        {
            _playerMoveInputAction.Disable();
        }

        
        protected void OnDestroy()
        {
            // Optional: Handle any cleanup here...
        }
    
        
        //  Methods ---------------------------------
        private void HandleMovement()
        {
         
            Vector2 moveInput = _playerMoveInputAction.ReadValue<Vector2>();
    
            float targetMovementSpeed = moveInput.y * _linearSpeed;
            float targetRotationSpeed = moveInput.x * _angularSpeed;

            // Smoothly interpolate movement speed
            _currentMovementSpeed = Mathf.Lerp(
                _currentMovementSpeed, 
                targetMovementSpeed, 
                Time.deltaTime / MovementSmoothingTime
            );

            // Smoothly interpolate rotation speed
            _currentRotationSpeed = Mathf.Lerp(
                _currentRotationSpeed, 
                targetRotationSpeed, 
                Time.deltaTime / RotationSmoothingTime
            );

            // Apply movement and rotation
            float movement = _currentMovementSpeed * Time.deltaTime;
            float rotation = _currentRotationSpeed * Time.deltaTime;

            transform.Rotate(0, rotation, 0);
            transform.Translate(0, 0, -movement);
        }

    }
}
