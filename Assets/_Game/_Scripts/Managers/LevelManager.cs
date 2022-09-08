using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;

public class LevelManager : PersistentSingleton<LevelManager> {
    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    private float _target;

    private void Update() {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 2 * Time.deltaTime);
    }

    public async void LoadScene(string sceneName) {
        _progressBar.fillAmount = 0;
        _target = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        _loaderCanvas.SetActive(true);

        do {
            await Task.Delay(100);
            _target = scene.progress;
        } while (scene.progress < 0.9f);

        await Task.Delay(1000);

        scene.allowSceneActivation = true;
        _loaderCanvas.SetActive(false);
    }
}