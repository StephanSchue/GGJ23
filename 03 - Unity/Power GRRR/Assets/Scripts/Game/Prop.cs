using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class Prop : MonoBehaviour
    {
        // -- Variables ---
        private Collider2D[] colliders;

        private bool _isNight = false;

        // --- Properties ---
        public bool IsNight => _isNight;

        private void Awake()
        {
            colliders = GetComponents<Collider2D>();
        }

        #region Public methods

        public void RegisterEvents(UnityEvent onDaySwitch, UnityEvent onNightSwitch)
        {
            onDaySwitch.AddListener(OnDaySwitch);
            onNightSwitch.AddListener(OnNightSwitch);
        }

        #endregion

        #region Events

        private void OnDaySwitch()
        {
            // Debug.Log("OnDaySwitch");
            _isNight = false;

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = true;
            }            
        }

        private void OnNightSwitch()
        {
            // Debug.Log("OnNightSwitch");
            _isNight = true;

            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = false;
            }
        }

        #endregion
    }
}