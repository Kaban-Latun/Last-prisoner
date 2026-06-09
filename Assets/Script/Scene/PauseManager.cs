using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseManager : MonoBehaviour
{
    [Header("Паузное меню")]
    [SerializeField] private GameObject pauseCanvas;
    [SerializeField] private GameObject pause_panel;
    private bool isPaused = false;
    private void Awake()
    {
        Debug.Log("PauseManager запущен!");
        if (pauseCanvas == null) Debug.LogError("PauseCanvas не назначен в инспекторе!");
        if (pause_panel == null) Debug.LogWarning("PausePanel не назначен!");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC нажат! Сейчас пауза = " + isPaused);
            TogglePause();
        }
    }
    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseCanvas != null) pauseCanvas.SetActive(true);
        if (pause_panel != null) pause_panel.SetActive(true);
        Debug.Log("ИГРА ПАУЗИРОВАНА");
    }
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseCanvas != null) pauseCanvas.SetActive(false);
        if (pause_panel != null) pause_panel.SetActive(false);
        Debug.Log("ИГРА ПРОДОЛЖЕНА");
    }
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}