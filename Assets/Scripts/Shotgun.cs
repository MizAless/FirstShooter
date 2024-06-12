using System.Collections;
using UnityEngine;
public class Shotgun : MonoBehaviour {
    [SerializeField] private Mover _mover;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float spreadAngle = 0.15f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private int projectileCount = 15;
    [SerializeField] private float _shootCooldown = 0.8f;
    [SerializeField] private float _distance = 10f;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private float recoilUpDuration = 0.2f;
    [SerializeField] private float recoilDownDuration = 0.2f;
    [SerializeField] private float recoilAngleUp = 5f;
    [SerializeField] private float recoilAngleDown = 5f;
    [SerializeField] private float recoilAngleSide = 2f;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private ParticleSystem _muzzleFlash;
    private Vector3 originalRotation;
    private bool IsShooting = false;
    private bool IsWalking = false;
    private bool CanShoot = true;
    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
        IsWalking = _mover.move.x != 0 || _mover.move.z != 0;
        _animator.SetBool(nameof(IsShooting), IsShooting);
        _animator.SetBool(nameof(IsWalking), IsWalking);
    }
    void Shoot() {
        if (!CanShoot) return;
        _muzzleFlash.Play();
        StartCoroutine(ShootProcess());
        StartCoroutine(ShootingAnimationProcess());
        for (int i = 0; i < projectileCount; i++) {
            Vector3 spread = Camera.main.transform.forward + Random.insideUnitSphere * spreadAngle;
            Ray ray = new Ray(Camera.main.transform.position, spread);
            TrailRenderer newTrail = Instantiate(_trail, firePoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(newTrail, Camera.main.transform.position + spread * 10f));
            if (Physics.Raycast(ray, out RaycastHit hit, _distance) && hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
                enemy.Health -= (int)damage;
        }
        StartCoroutine(RecoilEffect());
    }
    private IEnumerator ShootProcess() {
        CanShoot = false;
        _shootSound.gameObject.SetActive(true);
        _shootSound.transform.position = firePoint.position;
        yield return new WaitForSeconds(_shootCooldown);
        _shootSound.gameObject.SetActive(false);
        CanShoot = true;
    }
    private IEnumerator ShootingAnimationProcess() {
        IsShooting = true;
        yield return new WaitForSeconds(0.800f);
        IsShooting = false;
    }
    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 endPoint) {
        float time = 0;
        Vector3 spawnPosition = trail.transform.position;
        while (time < 1) {
            trail.transform.position = Vector3.Lerp(spawnPosition, endPoint, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = endPoint;
        Destroy(trail.gameObject);
    }
    private IEnumerator RecoilEffect() {
        float recoilTimer = 0f;
        Vector3 recoilRotation = new Vector3(-recoilAngleUp, Random.Range(-recoilAngleSide, recoilAngleSide), 0f);
        int iterations = (int)(recoilUpDuration / Time.fixedDeltaTime);
        float deltaAngle = -recoilAngleUp / iterations;
        while (recoilTimer < recoilUpDuration) {
            recoilTimer += Time.fixedDeltaTime;
            _mover.verticalRotation += deltaAngle;
            yield return new WaitForFixedUpdate();
        }
        recoilTimer = 0;
        iterations = (int)(recoilDownDuration / Time.fixedDeltaTime);
        deltaAngle = recoilAngleDown / iterations;
        while (recoilTimer < recoilDownDuration) {
            recoilTimer += Time.fixedDeltaTime;
            _mover.verticalRotation += deltaAngle;
            yield return new WaitForFixedUpdate();
        }
    }
}