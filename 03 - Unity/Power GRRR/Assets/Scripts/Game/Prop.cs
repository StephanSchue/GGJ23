using UnityEngine;

namespace GGJ23.Game
{
    public class Prop : MonoBehaviour
    {
        public BoxCollider2D collider;

        private bool _isNight = false;

        public bool IsNight => _isNight;

        public void OnDaySwitch()
        {
            // Debug.Log("OnDaySwitch");
            _isNight = false;
            collider.enabled = true;
        }

        public void OnNightSwitch()
        {
            // Debug.Log("OnNightSwitch");
            _isNight = true;
            collider.enabled = false;
        }
    }
}