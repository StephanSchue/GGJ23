using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class PropController : MonoBehaviour
    {
        public Prop[] Props { get => _props; }
        private Prop[] _props;

        public void PopulateProps(Prop[] props, UnityEvent onDaySwitch, UnityEvent onNightSwitch)
        {
            _props = props;

            for (int i = 0; i < _props.Length; i++)
            {
                _props[i].RegisterEvents(onDaySwitch, onNightSwitch);
            }
        }
    }
}