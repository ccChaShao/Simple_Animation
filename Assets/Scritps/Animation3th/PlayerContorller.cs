using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animation3th
{
    [RequireComponent(typeof(Animator), typeof(CharacterController), typeof(PlayerInputController))]
    public class PlayerContorller : MonoBehaviour
    {
        private CharacterController m_CharacterController;
        private PlayerInputController m_InputController;
        
        [Header("输入")]
        private Vector3 m_Direction;

        [Header("移动")] 
        public float speed = 5.0f;
        private Vector3 m_MoveDirection;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_InputController = GetComponent<PlayerInputController>();
        }

        private void Update()
        {
            SetPlayerMove(Time.deltaTime);
        }

        private void SetPlayerMove(float deltaTime)
        {
            // 根据输入常见移动方向向量
            m_Direction = new Vector3(m_InputController.moveValue.x, 0.0f, m_InputController.moveValue.y);
            
            // 方向转换
            m_MoveDirection = transform.TransformDirection(m_Direction);
            m_CharacterController.Move(m_MoveDirection.normalized * speed * deltaTime);
        }
    }
}