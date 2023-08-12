using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void GameMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync(1);
    }

    public void GoToStore()
    {
        // Chuyển sang Scene Store (nếu có)
        SceneManager.LoadScene("Store");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private bool CheckSavedData()
    {
        return PlayerPrefs.HasKey("SavedData");
    }
}