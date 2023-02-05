using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class EffectContoller : MonoBehaviour
    {
        [Header("Layers")]
        public UnityEvent OnStart;
        public UnityEvent OnDay;
        public UnityEvent OnNight;
        public UnityEvent OnDayNightTransition;

        public UnityEvent OnBrokenLevel01;
        public UnityEvent OnBrokenLevel02;
        public UnityEvent OnBrokenLevel03;

        [Header("Interactable")]
        public UnityEvent OnBreak;
        public UnityEvent OnFixStart;
        public UnityEvent OnFixOnFixAbourt;
        public UnityEvent OnFixOnFixComplete;

        [Header("Movement")]
        public UnityEvent OnMovementStart;
        public UnityEvent OnMovementStep;
        public UnityEvent OnMovementStop;
        public UnityEvent OnMovementBoost;
    }
}