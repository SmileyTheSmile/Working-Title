using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private MenuAudio _menuAudio;
    [SerializeField] private Slider _slider;

    private void OnEnable()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(float value)
    {
        _menuAudio.PlaySliderValueChangedSound(value);
    }
}