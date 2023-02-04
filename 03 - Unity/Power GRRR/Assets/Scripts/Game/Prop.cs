using UnityEngine;

namespace GGJ23.Game
{
    public class Prop : MonoBehaviour
    {
        private bool _isNight = false;

        public bool IsNight => _isNight;

        public void OnDaySwitch()
        {
            // Debug.Log("OnDaySwitch");
            _isNight = false;
        }

        public void OnNightSwitch()
        {
            // Debug.Log("OnNightSwitch");
            _isNight = true;
        }
    }
}