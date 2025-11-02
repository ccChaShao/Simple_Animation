using System;
using UnityEngine;

namespace Animation3th
{
    public class GameRoot : MonoSingleton<GameRoot>
    {
        private InputService m_InputService;
        
        private void Awake()
        {
            m_InputService = InputService.Instance;
        }
    }
}