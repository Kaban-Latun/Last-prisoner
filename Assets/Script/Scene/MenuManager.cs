using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Главное меню")]
    [SerializeField] private GameObject main_panel;

    [Header("Кнопки главного меню")]
    [SerializeField] private GameObject play_button;
    [SerializeField] private GameObject load_button;
    [SerializeField] private GameObject settings_button;
    [SerializeField] private GameObject notes_button;
    [SerializeField] private GameObject exit_button;

    [Header("Панель настроек")]
    [SerializeField] private GameObject settings_panel;

    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }
    public void StartGameWithDelay()
    {
        Invoke("LoadFirstLevel", 0.3f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene("Level1");
    }

    private void Start()
    {
        // Защита от ошибок в других сценах
        if (main_panel == null && settings_panel == null)
            return;

        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        main_panel.SetActive(true);

        play_button.SetActive(true);
        load_button.SetActive(false);
        settings_button.SetActive(true);
        notes_button.SetActive(true);
        exit_button.SetActive(true);

        settings_panel.SetActive(false);
    }

    public void ShowSettings()
    {
        if (settings_panel == null)
        {
            Debug.LogWarning("settings_panel не назначен в MenuManager. Возможно, мы в игровой сцене.");
            return;
        }

        if (main_panel != null) main_panel.SetActive(false);
        settings_panel.SetActive(true);

        SettingsManager sm = settings_panel.GetComponentInChildren<SettingsManager>(true);
        if (sm != null) sm.OpenSettings();
    }
    public void CloseSettings()
    {
        if (main_panel != null) main_panel.SetActive(true);
        if (settings_panel != null) settings_panel.SetActive(false);
    }
}