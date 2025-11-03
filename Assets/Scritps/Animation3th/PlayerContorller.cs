using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animation3th
{
    [RequireComponent(typeof(Animator), typeof(CharacterController), typeof(PlayerInputController))]
    public class PlayerContorller : MonoBehaviour
    {
        private Camera m_Camera;
        private CharacterController m_CharacterController;
        private PlayerInputController m_InputController;
        private Animator m_Animator;
        
        [Header("角色状态")]
        [ReadOnly, ShowInInspector] private float m_CurrentSpeed = 0.5f;

        [ReadOnly, ShowInInspector] private bool m_IsInGround = false;

        [Header("移动")] 
        public float rotationSpeed = 1f;
        public float walkSpeed = 1f;
        public float runeSpeed = 4f;    
        private Vector3 m_MoveDirection;

        [Header("跳跃")] 
        public float jumpHeight = 2.0f;
        public float gravity = -9.81f;

        private Vector3 m_VerticalVelocity;
        public float currentSpeed => m_CurrentSpeed;
        public bool isGrounded => m_IsInGround;
        public Vector3 verticalVelocity => m_VerticalVelocity;
        

        private void Awake()
        {
            m_Camera = FindAnyObjectByType<Camera>();
            m_CharacterController = GetComponent<CharacterController>();
            m_InputController = GetComponent<PlayerInputController>();
            m_Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            SetPlayerRotation(Time.deltaTime);
            SetPlayerGravity(Time.deltaTime);
            SetPlayerJump(Time.deltaTime);
        }

        private void OnAnimatorMove()
        {
            if (m_InputController.moveValue.magnitude > 0.1f)
            {
                if (m_InputController.isSprinting)
                {
                    m_CurrentSpeed = runeSpeed;
                }
                else
                {
                    m_CurrentSpeed = walkSpeed;
                }
            }
            else
            {
                m_CurrentSpeed = 0.0f;
            }
        
            // 当前帧相对于上一帧的位移增量
             Vector3 deltaPosition = m_Animator.deltaPosition;   
             m_CharacterController.Move(deltaPosition);
        }

        private void SetPlayerRotation(float deltaTime)
        {
            float yRotation = m_Camera.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * deltaTime);
        }

        private void SetPlayerGravity(float deltaTime)
        {
            m_CharacterController.Move(m_VerticalVelocity * deltaTime);
            m_IsInGround = m_CharacterController.isGrounded;
            
            if (m_IsInGround)
            {
                // 当isGrounded为true时，将Y轴方向的速度设为了一个较小的负值，这是为了确保能稳定触碰地面，避免isGrounded误判为false
                m_VerticalVelocity = new Vector3(0, -2f, 0);
            }
            else
            {
                m_VerticalVelocity.y += gravity * deltaTime;
            }
        }

        private void SetPlayerJump(float deltaTime)
        {
            if (m_IsInGround && m_InputController.isJump)
            {
                m_VerticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }
    }
}