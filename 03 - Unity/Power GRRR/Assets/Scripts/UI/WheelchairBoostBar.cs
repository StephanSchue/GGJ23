using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelchairBoostBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _sliderFill;

    // Update is called once per frame
    void Update()
    {
        _sliderFill.color = Color.Lerp(Color.red, Color.green, _slider.value / 100);
    }
}
