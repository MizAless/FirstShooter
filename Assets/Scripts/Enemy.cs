using System.Collections;
using UnityEngine;
public class Enemy : MonoBehaviour {
    public int Health;
    [SerializeField] private ParticleSystem _meetEffectPrefab;
    [SerializeField] private Mover player;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDuration = 4.4f / 3f;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private EnemyProjectile _enemyProjectile;
    [SerializeField] private float _attackForce;
    [SerializeField] private float _agroDistance = 10f;
    private float _shootCooldown;
    private float _movespeed;
    private float _directionValue;
    private bool IsAttack;
    private void Awake() {
        _movespeed = Random.Range(3, 5);
        _shootCooldown = Random.Range(4, 7);
        _directionValue = Random.value;
    }
    private void Update() {
        if (Health <= 0) {
            Destroy(gameObject);   
            Instantiate(_meetEffectPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
            _meetEffectPrefab.Play();
        }
        if (Vector3.Distance(player.transform.position, transform.position) < _agroDistance) {
            transform.position += (_directionValue < 0.5f ? transform.right : transform.right * -1) * _movespeed * Time.deltaTime;
            transform.LookAt(player.transform);
            _animator.SetBool("IsWalking", true);
            if (!IsAttack) StartCoroutine(Attacking());
        }
        else {
            _animator.SetBool("IsWalking", false);
        }
    }
    private IEnumerator Attacking() {
        IsAttack = true;
        _animator.SetBool(nameof(IsAttack), IsAttack);
        yield return new WaitForSeconds(0.5f);
        var projectile = Instantiate(_enemyProjectile, _firePoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce((player.transform.position - _firePoint.transform.position).normalized * _attackForce, ForceMode.Impulse);
        yield return new WaitForSeconds(_attackDuration - 0.5f);
        _animator.SetBool(nameof(IsAttack), false);
        yield return new WaitForSeconds(_shootCooldown - _attackDuration - 0.5f);
        IsAttack = false;   
    }
}