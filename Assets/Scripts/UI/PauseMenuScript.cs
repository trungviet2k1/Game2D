using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject continueButton;

    private bool isPaused = false;

    private void Start()
    {
        // Ẩn menu Pause ban đầu
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        // Kiểm tra nếu người chơi ấn Pause (ví dụ: bằng cách nhấn phím Esc)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        // Dừng trò chơi và hiển thị menu Pause
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        // Tiếp tục trò chơi và ẩn menu Pause
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void SaveAndGoToMainMenu()
    {
        // Code xử lý để lưu dữ liệu trò chơi

        // Đặt lại thời gian và ẩn menu Pause
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;

        // Chuyển sang Scene MainMenu
        SceneManager.LoadSceneAsync(0);
    }
}