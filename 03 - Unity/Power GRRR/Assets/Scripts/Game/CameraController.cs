using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace GGJ23.Game
{
    public class CameraController : MonoBehaviour
    {
        public Camera focusCamera;

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
            focusCamera.DOColor(dayColor, 2f);
            // camera.backgroundColor = dayColor;

            dayVirtualCamera.Priority = 1000;
            nightVirtualCamera.Priority = 0;
        }

        private void OnNightSwitch()
        {
            focusCamera.DOColor(nightColor, 2f);
            // camera.backgroundColor = nightColor;

            dayVirtualCamera.Priority = 0;
            nightVirtualCamera.Priority = 1000;
        }
    }

}