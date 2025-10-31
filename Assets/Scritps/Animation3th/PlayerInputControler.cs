
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Animation3th
{
    public class PlayerInputControler : MonoSingleton<PlayerInputControler>
    {
        private InputSystem_Actions m_InputSystem;
        
        public InputSystem_Actions inputSystem
        {
            get
            {
                if (m_InputSystem == null)
                {
                    m_InputSystem = new InputSystem_Actions();
                    m_InputSystem.Enable();
                }

                return m_InputSystem;
            }
        }
    
        public UnityEvent<InputAction.CallbackContext> onMovePerformed = new ();
    
        public UnityEvent<InputAction.CallbackContext> onMoveCanceled = new ();
    
        public UnityEvent<InputAction.CallbackContext> onJumpPerformed = new ();

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            onMovePerformed?.Invoke(context);
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            onMoveCanceled?.Invoke(context);
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            onJumpPerformed?.Invoke(context);
        }

        private void OnEnable()
        {
            inputSystem.Enable();
            inputSystem.Player.Move.performed += OnMovePerformed;
            inputSystem.Player.Move.canceled += OnMoveCanceled;
            inputSystem.Player.Jump.performed += OnJumpPerformed;
        }

        private void OnDisable()
        {
            inputSystem.Disable();
            inputSystem.Player.Move.performed -= OnMovePerformed;
            inputSystem.Player.Move.canceled -= OnMoveCanceled;
            inputSystem.Player.Jump.performed -= OnJumpPerformed;
        }
    }
}