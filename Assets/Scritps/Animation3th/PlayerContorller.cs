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
        
        [TitleGroup("角色状态")]
        [ReadOnly, ShowInInspector] private float m_CurrentSpeed; // 当前速度
        [ReadOnly, ShowInInspector] private float m_CurrentAngle; // 当前角度
        [ReadOnly, ShowInInspector] private bool m_IsInGround;
        [ReadOnly, ShowInInspector] private bool m_IsAutoTurning;
        [ReadOnly, ShowInInspector] private bool m_IsIdle;

        [TitleGroup("移动")] 
        public float walkSpeed = 1f;
        public float runeSpeed = 4f;    
        private Vector3 m_MoveDirection;

        [TitleGroup("跳跃")] 
        public float jumpHeight = 2.0f;
        public float gravity = -9.81f;

        [TitleGroup("转向")] 
        public bool enableCustomTurn;
        public float trunSpeed = 5.0f;
        public float turnAngleThreshold = 45.0f; // 角度阈值
        public float turnCoolDown = 3.0f; // 转生动画延迟发生时间
        public float turnTime = 0.6f; // 转身需要的时间
        private float m_LastTurnTime; // 上次转身时间
        private float m_TurnStartTime; // 转身开始时间
        private Quaternion m_StartRotation; // 转身开始时的角度
        private Quaternion m_TargetRotation; // 转身的目标角度

        private Vector3 m_PlayerForward; // 玩家水平面正前方
        private Vector3 m_CameraForward; // 相机水平面正前方
        private Vector3 m_HorizontalVelocity;
        private Vector3 m_VerticalVelocity;
        
        public float currentSpeed => m_CurrentSpeed;
        public bool isGrounded => m_IsInGround;
        public Vector3 horizontalVelocity => m_HorizontalVelocity;
        public Vector3 verticalVelocity => m_VerticalVelocity;
        public bool isAutoTurning => m_IsAutoTurning;
        public float currentAngle => m_CurrentAngle;
        

        private void Awake()
        {
            m_Camera = FindAnyObjectByType<Camera>();
            m_CharacterController = GetComponent<CharacterController>();
            m_InputController = GetComponent<PlayerInputController>();
            m_Animator = GetComponent<Animator>();
        }

        private void Update()
        {
            CalculateBaseParams();
            SetPlayerJump(Time.deltaTime);
            SetPlayerRotation(Time.deltaTime);
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
             deltaPosition.y = 0;
             m_CharacterController.Move(deltaPosition);
        }

        /// <summary>
        /// 用于计算基础参数
        /// </summary>
        private void CalculateBaseParams()
        {
            // 状态
            m_IsIdle = m_InputController.moveValue.magnitude < 0.1f;
            
            // 更新相机水平面正方向
            m_CameraForward = m_Camera.transform.forward;
            m_CameraForward.y = 0;
            m_CameraForward.Normalize();
            
            // 更新角色水平面正方向
            m_PlayerForward = transform.forward;
            m_PlayerForward.y = 0;
            m_PlayerForward.Normalize();
            
            // 当前夹角（永远都是正数[0,180]）
            m_CurrentAngle = Vector3.Angle(m_PlayerForward, m_CameraForward);
            // 叉乘向量（用y进行判断左右关系）
            Vector3 crossV3 = Vector3.Cross(m_PlayerForward, m_CameraForward);
            // y小于0 说明相机在人的左侧，左侧定义为负方向
            if (crossV3.y < 0)
            {
                m_CurrentAngle *= -1;
            }
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
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, trunSpeed * deltaTime);
        }

        private void SetPlayerRotationPubg(float deltaTime)
        {
            // 没有移动输入，没有下蹲输入 & 角度大于阈值 & 不在旋转中 & 不在旋转cd中 & 在地面
            if (m_IsIdle && !m_InputController.isCrouching && m_IsInGround && MathF.Abs(m_CurrentAngle) > turnAngleThreshold && !m_IsAutoTurning && Time.time - m_LastTurnTime >= turnCoolDown)
            {
                m_IsAutoTurning = true;
                m_TurnStartTime = Time.time;
                m_StartRotation = transform.rotation;
                m_TargetRotation = Quaternion.LookRotation(m_CameraForward);
            }

            // 有移动输入 || 旋转结束
            if (!m_IsIdle || (m_IsAutoTurning && Time.time - m_TurnStartTime >= turnTime))
            {
                m_IsAutoTurning = false;
                m_LastTurnTime = Time.time;
            }

            // 人物旋转执行
            if (m_IsAutoTurning)
            {
                float turnProgress = Mathf.Clamp01((Time.time - m_TurnStartTime) / turnTime);
                transform.rotation = Quaternion.Slerp(m_StartRotation, m_TargetRotation, turnProgress);
            }
            else if (!m_IsIdle)
            {
                Quaternion endRotation = Quaternion.LookRotation(m_CameraForward);
                transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, trunSpeed * deltaTime);
            }
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
    }
}