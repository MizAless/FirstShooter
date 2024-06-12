using UnityEngine;
public class Boss : MonoBehaviour {
    [SerializeField] private Camera _firstCamera;
    [SerializeField] private Camera _secondCamera;
    [SerializeField] private EndCoroutine _endCoroutine;
    private void OnDestroy() {
        _firstCamera.gameObject.SetActive(false);
        _secondCamera.gameObject.SetActive(true);
        foreach (Enemy enemy in FindObjectsOfType<Enemy>()) enemy.Health = 0;
        _endCoroutine.Go();
    }
}