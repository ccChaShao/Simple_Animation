using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public class AnimancerTest : MonoBehaviour
{
    public AnimancerComponent animancerComponent;
    public AnimationClip idleClip;
    public AnimationClip runeClip;

    private void Awake()
    {
        animancerComponent = GetComponent<AnimancerComponent>();
    }

    [Button(ButtonSizes.Large)]
    public void PlayIdleClip()
    {
        animancerComponent.Play(idleClip);
    }

    [Button(ButtonSizes.Large)]
    public void PlayRuneClip()
    {
        animancerComponent.Play(runeClip);
    }
}
