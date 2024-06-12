using UnityEngine;
public class DeadZone : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent(out Enemy enemy)) enemy.Health = 0;
        if (other.gameObject.TryGetComponent(out Mover mover)) mover.TakeDamage(1000);
    }
}