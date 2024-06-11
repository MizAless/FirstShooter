using UnityEngine;
public class Enemy : MonoBehaviour {
    public int Health;
    [SerializeField] private ParticleSystem _meetEffectPrefab;
    [SerializeField] private FirstPersonShooterController player;
    [SerializeField] private Animator _animator;
    private float movespeed;
    private float directionValue;
    private void Awake() {
        movespeed = Random.Range(3, 5);
        directionValue = Random.value;
    }
    private void Update() {
        if (Health <= 0) {
            Destroy(gameObject);   
            Instantiate(_meetEffectPrefab, transform.position, Quaternion.identity);
            _meetEffectPrefab.Play();
        }
        if (Vector3.Distance(player.transform.position, transform.position) < 10)
        {
            transform.position += (directionValue < 0.5f ? transform.right : transform.right * -1) * movespeed * Time.deltaTime;
            transform.LookAt(player.transform);
            _animator.SetBool("IsWalking", true);
        }
        else
        {
            _animator.SetBool("IsWalking", false);
        }
    }
}