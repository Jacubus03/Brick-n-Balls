using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{
    [SerializeField] private GameObject MenuUI;
    [SerializeField] private GameObject GameOverUI;

    public void OnStartClick()
    {
        SceneManager.LoadScene("GameScene", LoadSceneMode.Additive);
        MenuUI.SetActive(false);
    }

    public void OnBackToMenuClick()
    {
        SceneManager.UnloadSceneAsync("GameScene");
        GameOverUI.SetActive(false);
        MenuUI.SetActive(true);
    }
}
