using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class Prop : MonoBehaviour
    {
        // -- Variables ---
        public BoxCollider2D collider;

        private bool _isNight = false;

        // --- Properties ---
        public bool IsNight => _isNight;

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
            collider.enabled = true;
        }

        private void OnNightSwitch()
        {
            // Debug.Log("OnNightSwitch");
            _isNight = true;
            collider.enabled = false;
        }

        #endregion
    }
}