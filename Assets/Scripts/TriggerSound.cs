using System.Collections;
using UnityEngine;
public class TriggerSound : MonoBehaviour {
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _destroyebleObject;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<Mover>(out _)) _audioSource.Play();
        if (_destroyebleObject != null) StartCoroutine(Destroy¿fterWhile());
    }
    private IEnumerator Destroy¿fterWhile() {
        yield return new WaitForSeconds(33);
        Destroy(_destroyebleObject);
    }
}