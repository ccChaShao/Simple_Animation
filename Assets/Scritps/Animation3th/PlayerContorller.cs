using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

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
        [ReadOnly, ShowInInspector] private float m_CurrentSpeed; // 当前速度
        [ReadOnly, ShowInInspector] private float m_CurrentAngle; // 当前角度
        [ReadOnly, ShowInInspector] private bool m_IsInGround;
        [ReadOnly, ShowInInspector] private bool m_IsTurning;
        [ReadOnly, ShowInInspector] private bool m_IsIdle;

        [Header("移动")] 
        public float walkSpeed = 1f;
        public float runeSpeed = 4f;    
        private Vector3 m_MoveDirection;

        [Header("跳跃")] 
        public float jumpHeight = 2.0f;
        public float gravity = -9.81f;

        [BoxGroup("转向")] 
        public bool enableCustomTurn;
        [BoxGroup("转向/传统")]
        public float customTurnSpeed = 1f;
        [BoxGroup("转向/Pubg")]
        public float angleThreshold = 45.0f; // 角度阈值
        public float turnSpeed = 8.0f; // 转向速度
        public float turnDelay = 3.0f; // 转生动画延迟发生时间
        public float turnTime = 0.6f; // 转身需要的时间
        private float lastTurnTime; // 上次转身时间
        private float turnStartTime; // 转身开始时间
        private Quaternion startRotation; // 转身开始时的角度
        private Quaternion targetRotation; // 转身的目标角度

        private Vector3 m_PlayerForward; // 玩家水平面正前方
        private Vector3 m_CameraForward; // 相机水平面正前方
        private Vector3 m_HorizontalVelocity;
        private Vector3 m_VerticalVelocity;
        
        public float currentSpeed => m_CurrentSpeed;
        public bool isGrounded => m_IsInGround;
        public Vector3 horizontalVelocity => m_HorizontalVelocity;
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
            CalculateCameraAngle();
            SetPlayerRotation(Time.deltaTime);
            SetPlayerJump(Time.deltaTime);
            SetGroundVeticalVelocity(Time.deltaTime);
            SetAirHorizontalVelocity(Time.deltaTime);
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
            if (enableCustomTurn)
            {
                SetPlayerRotationCustom(deltaTime);
            }
            else
            {
                SetPlayerRotationPubg(deltaTime);
            }
        }

        private void SetPlayerRotationCustom(float deltaTime)
        {
            float yRotation = m_Camera.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, customTurnSpeed * deltaTime);
        }

        private void SetPlayerRotationPubg(float deltaTime)
        {
            
        }

        private void SetPlayerJump(float deltaTime)
        {
            if (m_IsInGround && m_InputController.isJump)
            {
                m_VerticalVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }

        private void SetGroundVeticalVelocity(float deltaTime)
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

        private void SetAirHorizontalVelocity(float deltaTime)
        {
            if (!m_IsInGround)
            {
                m_CharacterController.Move(m_HorizontalVelocity * deltaTime);
                //将该向量从局部坐标系转换为世界坐标系，得到最终的移动方向
                var direction = new Vector3(m_InputController.moveValue.x, 0, m_InputController.moveValue.y);
                m_HorizontalVelocity  = transform.TransformDirection(direction) * currentSpeed;
            }
        }

        private void CalculateCameraAngle()
        {
            // 更新相机水平面正方向
            m_CameraForward = m_Camera.transform.forward;
            m_CameraForward.y = 0;
            m_CameraForward.Normalize();
            
            // 更新角色水平面正方向
            m_PlayerForward = transform.forward;
            m_PlayerForward.y = 0;
            m_PlayerForward.Normalize();
            
            // 当前夹角
            m_CurrentAngle = Vector3.Angle(m_PlayerForward, m_CameraForward);
            
            // 叉乘判断左右
            float cross = Vector3.Cross(m_PlayerForward, m_CameraForward).y;
            
            // y小于0 说明相机在人的左侧，左侧定义为负方向
            if (cross < 0)
            {
                m_CurrentAngle *= -1;
            }
        }
    }
}