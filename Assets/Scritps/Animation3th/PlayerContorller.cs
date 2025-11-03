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
        
        [Header("输入")]
        private Vector3 m_Direction;

        [Header("移动")] 
        [ReadOnly, ShowInInspector] private float m_CurrentSpeed = 0.5f;
        public float rotationSpeed = 1f;
        public float walkSpeed = 1f;
        public float runeSpeed = 4f;    
        private Vector3 m_MoveDirection;
        public float currentSpeed => m_CurrentSpeed;

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
             // 移动 CharacterController
             m_CharacterController.Move(deltaPosition);
        }

        private void SetPlayerRotation(float deltaTime)
        {
            float yRotation = m_Camera.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * deltaTime);
        }
    }
}