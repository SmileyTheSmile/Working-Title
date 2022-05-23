using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//TODO Fix sounds playing on fade out
public class ButtonAudio : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private MenuAudio _menuAudio;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void OnButtonClick()
    {
        _menuAudio.PlayButtonClickSound();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _menuAudio.PlayButtonSelectSound();
    }
}