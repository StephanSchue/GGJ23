using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class CameraController : MonoBehaviour
    {
        public Camera camera;

        public Cinemachine.CinemachineVirtualCamera dayVirtualCamera;
        public Cinemachine.CinemachineVirtualCamera nightVirtualCamera;

        public Color dayColor = Color.white;
        public Color nightColor = Color.black;

        public void Populate(UnityEvent OnDaySwitch, UnityEvent OnNightSwitch)
        {
            OnDaySwitch.AddListener(this.OnDaySwitch);
            OnNightSwitch.AddListener(this.OnNightSwitch);
        }

        private void OnDaySwitch()
        {
            camera.backgroundColor = dayColor;

            dayVirtualCamera.Priority = 1000;
            nightVirtualCamera.Priority = 0;
        }

        private void OnNightSwitch()
        {
            camera.backgroundColor = nightColor;

            dayVirtualCamera.Priority = 0;
            nightVirtualCamera.Priority = 1000;
        }
    }

}