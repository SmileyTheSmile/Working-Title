using UnityEngine;
using Cinemachine;

public class VictoryItem : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private AudioSourcePlayer _victorySound;
    [SerializeField] private AudioSourcePlayer _explosionSound;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private SpriteRenderer _sprite;

    private BoxCollider2D _trigger;

    private void Awake()
    {
        _trigger = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        _victorySound.Play();
        _explosionSound.Play();

        Destroy(_player.gameObject);

        _camera.Follow = this.transform;
        _sprite.enabled = false;
        UIManager.Instance.ShowVictoryScreen();
    }
}
