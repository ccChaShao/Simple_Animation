using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Animation3th
{
    public class GMService : MonoBehaviour
    {
        [ReadOnly] 
        public bool enableAnimSpeedSyn = true;

        [Button]
        public void SwitchEnableAnimSpeedSyn()
        {
            enableAnimSpeedSyn = !enableAnimSpeedSyn;
            var animatorControllers = FindObjectsOfType<PlayerAnimatorController>();
            if (animatorControllers == null || animatorControllers.Length <= 0)
            {
                return;
            }
            foreach (var c in animatorControllers)
            {
                c.Animator.speed = enableAnimSpeedSyn
                    ? 1 / c.Animator.humanScale * c.transform.localScale.x
                    : 1;
                Debug.Log("[SwitchEnableAnimSpeedSyn] : 动画更新完成。 " + c.name + "-" + c.Animator.speed);
            }
        }
    }
}