using System.Collections;
using UnityEngine;
public class EndCoroutine : MonoBehaviour {    
    [SerializeField] private GameObject _blackPanel;
    [SerializeField] private MusicSystem _musicSystem;
    public void Go() => StartCoroutine(ShowEndScreen());
    private IEnumerator ShowEndScreen() {
        _musicSystem.SetEndMusic();
        _blackPanel.transform.parent.gameObject.SetActive(true);
        float time = 0;
        while (time < 5) {
            time += Time.deltaTime;
            Color newColor = Color.black;
            newColor.a = Mathf.Lerp(0, 1, time / 5f);
            _blackPanel.GetComponent<UnityEngine.UI.Image>().color = newColor;
            yield return null;
        }
    }
}