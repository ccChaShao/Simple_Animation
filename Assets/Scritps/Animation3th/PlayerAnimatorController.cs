using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animation3th
{
    [RequireComponent(typeof(Animator), typeof(PlayerInputController), typeof(PlayerContorller))]
    public class PlayerAnimatorController : MonoBehaviour
    {
        private Animator m_Animator;
        private PlayerInputController m_InputController;
        private PlayerContorller m_PlayerContorller;
        
        [TitleGroup("移动动画")]
        public float smoothTime = 5.0f; //平滑插值速度

        private Vector2 m_currentInputBlend = Vector2.zero;
        private static int inputX = Animator.StringToHash("inputX");
        private static int inputY = Animator.StringToHash("inputY");
        private static int isCrouching = Animator.StringToHash("isCrouching");
        private static int jumpSpeed = Animator.StringToHash("jumpSpeed");
        private static int isJump = Animator.StringToHash("isJump");
        private static int isGround = Animator.StringToHash("isGround");
        private static int turnAngle = Animator.StringToHash("turnAngle");
        private static int isAutoTurning = Animator.StringToHash("isAutoTurning");
        
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
            m_Animator.SetFloat(jumpSpeed, m_PlayerContorller.verticalVelocity.y);
            m_Animator.SetBool(isCrouching, m_InputController.isCrouching);
            m_Animator.SetBool(isJump, m_InputController.isJump);
            m_Animator.SetBool(isGround, m_PlayerContorller.isGrounded);
            m_Animator.SetBool(isAutoTurning, m_PlayerContorller.isAutoTurning);
            m_Animator.SetFloat(turnAngle, m_PlayerContorller.currentAngle);
        }
    }
}