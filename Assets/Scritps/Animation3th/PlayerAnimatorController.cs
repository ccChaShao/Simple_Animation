using System;
using UnityEngine;

namespace Animation3th
{
    [RequireComponent(typeof(Animator), typeof(PlayerInputController), typeof(PlayerContorller))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator m_Animator;
        private PlayerInputController m_InputController;
        private PlayerContorller m_PlayerContorller;

        private static int inputX = Animator.StringToHash("inputX");
        private static int inputY = Animator.StringToHash("inputY");
        
        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_InputController = GetComponent<PlayerInputController>();
            m_PlayerContorller = GetComponent<PlayerContorller>();
        }

        private void Update()
        {
            UpdateAnimatorState();
        }

        private void UpdateAnimatorState()
        {
            m_Animator.SetFloat(inputX, m_InputController.moveValue.x * m_PlayerContorller.currentSpeed);
            m_Animator.SetFloat(inputY, m_InputController.moveValue.y * m_PlayerContorller.currentSpeed);
        }
    }
}