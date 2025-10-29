using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

[RequireComponent(typeof(AnimancerComponent))]
public class AnimancerTest : MonoBehaviour
{

    private void Awake()
    {
        animancerComponent = GetComponent<AnimancerComponent>();
    }

    [Button(ButtonSizes.Large)]
    public void StopAnimation()
    {
        animancerComponent.Stop();
    }

    #region AnimationClip
    
    [BoxGroup("传统 - AnimationClip")] public AnimationClip idleClip;
    [BoxGroup("传统 - AnimationClip")] public AnimationClip runClip;

    [BoxGroup("传统 - AnimationClip"), Button(ButtonSizes.Medium)]
    public void PlayIdleClip()
    {
        animancerComponent.Play(idleClip);
    }

    [BoxGroup("传统 - AnimationClip"), Button(ButtonSizes.Medium)]
    public void PlayRunClip()
    {
        animancerComponent.Play(runClip);
    }
    #endregion

    #region ClipTransition
    
    [BoxGroup("新动画 - ClipTransition"), ReadOnly]
    public AnimancerComponent animancerComponent;
    

    [BoxGroup("新动画 - ClipTransition/idle")] 
    public ClipTransition idleTransition;

    [BoxGroup("新动画 - ClipTransition/idle"), Button(ButtonSizes.Medium)]
    public void PlayIdleTransition(float fadeDuration)
    {
       var state = animancerComponent.Play(idleTransition, fadeDuration, FadeMode.FromStart);
    }
    

    [BoxGroup("新动画 - ClipTransition/run")] 
    public ClipTransition runTransition;
    
    [BoxGroup("新动画 - ClipTransition/run"), Button(ButtonSizes.Medium)]
    public void PlayRunTransition(float fadeDuration)
    {
        var state = animancerComponent.Play(runTransition, fadeDuration, FadeMode.FromStart);
    }
    
    
    [BoxGroup("新动画 - ClipTransition/mixer"), ProgressBar(0.0f, 1.0f), OnValueChanged("OnClipTransitionWeightValueChanged")]
    public float clipTransitionWeight = 0f;
    
    [BoxGroup("新动画 - ClipTransition/mixer")] 
    public LinearMixerTransition mixerTransition;
    
    private AnimancerState mixerState;

    [BoxGroup("新动画 - ClipTransition/mixer"), Button(ButtonSizes.Medium)]
    public void PlayMixerTransition(float fadeDuration)
    {
        var state = animancerComponent.Play(mixerTransition, fadeDuration, FadeMode.FromStart);
        mixerState = state;
    }

    public void OnClipTransitionWeightValueChanged()
    {
        Debug.Log("charsiew : [OnClipTransitionWeightValueChanged] : " + clipTransitionWeight);
    }

    public void AnimancerTestEvent01()
    {
        Debug.Log("charsiew : [AnimancerTestEvent01] : " + animancerComponent.Events.Count);
    }

    public void AnimancerTestEvent02()
    {
        Debug.Log("charsiew : [AnimancerTestEvent02] : " + animancerComponent.Events.Count);
    }

    #endregion
}
