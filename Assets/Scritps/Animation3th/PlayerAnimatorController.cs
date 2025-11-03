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
        
        public float smoothTime = 5.0f;//平滑插值速度

        private Vector2 m_currentInputBlend = Vector2.zero;
        private static int inputX = Animator.StringToHash("inputX");
        private static int inputY = Animator.StringToHash("inputY");
        private static int isCrouching = Animator.StringToHash("isCrouching");
        
        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
            m_InputController = GetComponent<PlayerInputController>();
            m_PlayerContorller = GetComponent<PlayerContorller>();
        }

        private void Update()
        {
            UpdateAnimatorState(Time.deltaTime);
        }

        private void UpdateAnimatorState(float deltaTime)
        {
            m_currentInputBlend = Vector2.Lerp(m_currentInputBlend, m_InputController.moveValue, smoothTime * deltaTime);
            m_Animator.SetFloat(inputX, m_currentInputBlend.x * m_PlayerContorller.currentSpeed);
            m_Animator.SetFloat(inputY, m_currentInputBlend.y * m_PlayerContorller.currentSpeed);
            
            m_Animator.SetBool(isCrouching, m_InputController.isCrouching);
        }
    }
}