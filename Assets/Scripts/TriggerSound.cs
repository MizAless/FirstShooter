using UnityEngine;
public class TriggerSound : MonoBehaviour {
    [SerializeField] private AudioSource _audioSource;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent<Mover>(out _)) _audioSource.Play();
    }
}