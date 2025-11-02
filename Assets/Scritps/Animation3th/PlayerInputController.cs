using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Animation3th
{
    public class PlayerInputController : MonoBehaviour
    {
        private InputService m_InputService;

        public Vector2 moveValue => m_InputService.inputSystem.Player.Move.ReadValue<Vector2>();
        public bool isSprint => m_InputService.inputSystem.Player.Sprint.triggered;
        public bool isCrouch => m_InputService.inputSystem.Player.Crouch.triggered;
        public bool isJump => m_InputService.inputSystem.Player.Jump.triggered;
        
        private void Awake()
        {
            m_InputService = InputService.Instance;
            m_InputService.onMovePerformed.AddListener(OnMovePerformed);
            m_InputService.onJumpPerformed.AddListener(OnJumpPerformed);
        }

        private void Update()
        {
            Debug.Log("[Update] : -------" + isSprint + "----" + isCrouch + "----" + isJump);
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
        }
    }
}