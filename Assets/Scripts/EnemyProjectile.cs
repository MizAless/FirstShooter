using System.Collections;
using UnityEngine;
public class EnemyProjectile : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.TryGetComponent(out Mover mover)) mover.TakeDamage(10);
        StartCoroutine(SelfDestroy());
    }
    private IEnumerator SelfDestroy() {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}