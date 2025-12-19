using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonEvents : MonoBehaviour
{
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameStateManager _gameStateManager;

    public void OnStartClick()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        _menuUI.SetActive(false);
    }

    public void OnBackToMenuClick()
    {
        StartCoroutine(UnloadGameScene());
    }

    private IEnumerator UnloadGameScene()
    {
        AsyncOperation op = SceneManager.UnloadSceneAsync("GameScene");
        yield return op;
        _gameOverUI.SetActive(false);
        _menuUI.SetActive(true);
        _gameStateManager.Reset();
    }
}
