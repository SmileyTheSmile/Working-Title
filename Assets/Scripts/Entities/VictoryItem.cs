using UnityEngine;
using Cinemachine;

public class VictoryItem : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private AudioSourcePlayer _victorySound;
    [SerializeField] private AudioSourcePlayer _explosionSound;
    [SerializeField] private CinemachineVirtualCamera _camera;
    [SerializeField] private Animator _animator;

    private BoxCollider2D _trigger;

    private void Awake()
    {
        _trigger = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        _animator.SetTrigger("explosion");
        _victorySound.Play();
        _explosionSound.Play();

        Destroy(_player.gameObject);
        _camera.Follow = this.transform;

        UIManager.Instance.ShowVictoryScreen();
    }
}
