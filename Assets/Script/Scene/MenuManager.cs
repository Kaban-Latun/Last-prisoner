using UnityEngine;

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

    private void Start()
    {
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
        main_panel.SetActive(false);
        settings_panel.SetActive(true);
    }
}