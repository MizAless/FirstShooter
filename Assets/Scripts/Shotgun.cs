using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    [SerializeField] private FirstPersonShooterController _mover;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float spreadAngle = 0.15f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float _shootCooldown = 0.8f;
    [SerializeField] private float _distance = 10f;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private float recoilUpDuration = 0.2f;
    [SerializeField] private float recoilDownDuration = 0.2f;
    [SerializeField] private float recoilAngleUp = 5f;
    [SerializeField] private float recoilAngleSide = 2f;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;

    private Vector3 originalRotation;
    private bool IsShooting = false;
    private bool IsWalking = false;
    private bool CanShoot = true;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        IsWalking = _mover.move.x != 0 || _mover.move.z != 0;

        //print($"{_mover.move.x} {_mover.move.y} {_mover.move.z}");

        _animator.SetBool(nameof(IsShooting), IsShooting);
        _animator.SetBool(nameof(IsWalking), IsWalking);
    }

    void Shoot()
    {
        if (!CanShoot)
            return;

        CanShoot = false;
        int count = 0;

        for (int i = 0; i < 10; i++)
        {
            Vector3 spread = Camera.main.transform.forward + Random.insideUnitSphere * spreadAngle;
            Ray ray = new Ray(Camera.main.transform.position, spread);
            Debug.DrawLine(firePoint.position, Camera.main.transform.position + spread * 10f, Color.red, 0.1f);

            TrailRenderer newTrail = Instantiate(_trail, firePoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(newTrail, Camera.main.transform.position + spread * 10f));

            if (Physics.Raycast(ray, out RaycastHit hit, _distance))
                if (hit.collider.TryGetComponent<Enemy>(out Enemy enemy))
                {
                    count++;
                    enemy.Health -= (int)damage;
                }
        }

        StartCoroutine(RecoilEffect());

        Invoke(nameof(SetCanShoot), _shootCooldown);

        print(count);
    }

    private void SetCanShoot()
    {
        CanShoot = true;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 endPoint)
    {
        float time = 0;
        Vector3 spawnPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(spawnPosition, endPoint, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = endPoint;

        Destroy(trail.gameObject);
    }

    private IEnumerator RecoilEffect()
    {
        IsShooting = true;
        float recoilTimer = 0f;
        Vector3 recoilRotation = new Vector3(-recoilAngleUp, Random.Range(-recoilAngleSide, recoilAngleSide), 0f);

        //targetRotation = originalRotation + recoilRotation;
        Vector3 endRotation = Camera.main.transform.localEulerAngles;

        while (recoilTimer < recoilUpDuration)
        {
            recoilTimer += Time.deltaTime;
            float progress = recoilTimer / recoilUpDuration;
            //Camera.main.transform.localEulerAngles = Vector3.Lerp(originalRotation, targetRotation, progress);
            //Camera.main.transform.localEulerAngles += Vector3.Lerp(Vector3.zero, recoilRotation, progress);
            _mover.verticalRotation += Mathf.Lerp(0, -recoilAngleUp, progress);

            yield return null;
        }

        //print(endRotation.x);
        //Camera.main.transform.localEulerAngles = endRotation;

        recoilTimer = 0;

        while (recoilTimer < recoilDownDuration)
        {
            recoilTimer += Time.deltaTime;
            float progress = recoilTimer / recoilDownDuration;
            //Camera.main.transform.localEulerAngles = Vector3.Lerp(targetRotation, originalRotation, progress);
            //Camera.main.transform.localEulerAngles += Vector3.Lerp(-recoilRotation, Vector3.zero, progress);
            _mover.verticalRotation -= Mathf.Lerp(0, -recoilAngleUp, progress);

            yield return null;
        }

        IsShooting = false;
    }

    //private IEnumerator RecoilEffect()
    //{
    //    isRecoiling = true;
    //    float recoilTimer = 0f;
    //    Vector3 recoilRotation = new Vector3(-recoilAngleUp, Random.Range(-recoilAngleSide, recoilAngleSide), 0f);
    //    Vector3 startRotation = Camera.main.transform.localEulerAngles;
    //    Vector3 endRotation = Camera.main.transform.localEulerAngles;

    //    while (recoilTimer < recoilUpDuration)
    //    {
    //        recoilTimer += Time.deltaTime;
    //        float progress = recoilTimer / recoilUpDuration;
    //        Camera.main.transform.localEulerAngles = startRotation + Vector3.Lerp(Vector3.zero, recoilRotation, progress);

    //        if (recoilTimer >= recoilUpDuration)
    //        {
    //            endRotation = Camera.main.transform.localEulerAngles;
    //        }

    //        yield return null;
    //    }

    //    Camera.main.transform.localEulerAngles = endRotation;

    //    recoilTimer = 0;

    //    while (recoilTimer < recoilDownDuration)
    //    {
    //        recoilTimer += Time.deltaTime;
    //        float progress = recoilTimer / recoilDownDuration;
    //        Camera.main.transform.localEulerAngles = startRotation + Vector3.Lerp(-recoilRotation, Vector3.zero, progress);

    //        yield return null;
    //    }

    //    isRecoiling = false;
    //}
}