using System.Collections;
using UnityEngine;
public class MusicSystem : MonoBehaviour
{
    [SerializeField] private AudioClip[] _musicParts;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private Shotgun _shotgunScript;
    private bool isFight = true;
    private void Awake(){
        Time.timeScale = 0f;
    }
    public void CloseMenu(){
        _startMenu.SetActive(false);
        //StartCoroutine(SmoothChangeMusic(1));
        SetMusic(1);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        _shotgunScript.enabled = true;
    }
    private void Update(){
        Collider[] colliders = Physics.OverlapSphere(_shotgunScript.transform.position, 20f);

        bool isEnemyNear = false;

        foreach (Collider collider in colliders){
            if (collider.TryGetComponent<Enemy>(out _))
                isEnemyNear = true;
        }
        if (!_startMenu.active)
            if (isEnemyNear && !isFight){
                isFight = true;
                StartCoroutine(SmoothChangeMusic(1));
            }
            else if (!isEnemyNear && isFight){
                isFight = false;
                StartCoroutine(SmoothChangeMusic(2));
            }
    }
    private void SetMusic(int index){
        _audioSource.clip = _musicParts[index];
        _audioSource.Play();
    }
    private IEnumerator SmoothChangeMusic(int index){
        StartCoroutine(SmoothChange(1, 0, 2));
        yield return new WaitForSeconds(2);
        SetMusic(index);
        StartCoroutine(SmoothChange(0, 1, 0.6f));
    }
    private IEnumerator SmoothChange(float from, float to, float duration){
        float time = 0;
        while (time < duration){
            time += Time.fixedDeltaTime;
            _audioSource.volume = Mathf.Lerp(from, to, time / duration);
            yield return new WaitForFixedUpdate();
        }
    }
}
