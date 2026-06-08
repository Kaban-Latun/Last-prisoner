using UnityEngine;
public class SettingsManager : MonoBehaviour
{
    [Header("Кнопки выбора (основное меню настроек)")]
    [SerializeField] private GameObject graphics_button;
    [SerializeField] private GameObject audio_button;
    [SerializeField] private GameObject screen_button;
    [Header("Панели контента")]
    [SerializeField] private GameObject graphics_panel;
    [SerializeField] private GameObject audio_panel;
    [SerializeField] private GameObject screen_panel;
    private void Awake()
    {
        // Автопоиск, если не перетащили
        if (graphics_button == null) graphics_button = transform.Find("graphics_button")?.gameObject;
        if (audio_button == null) audio_button = transform.Find("audio_button")?.gameObject;
        if (screen_button == null) screen_button = transform.Find("screen_button")?.gameObject;
        if (graphics_panel == null) graphics_panel = transform.Find("graphics_panel")?.gameObject;
        if (audio_panel == null) audio_panel = transform.Find("audio_panel")?.gameObject;
        if (screen_panel == null) screen_panel = transform.Find("screen_panel")?.gameObject;
    }
    private void Start()
    {
        ShowSelection();
    }
    public void OpenSettings()
    {
        ShowSelection();
    }
    // Показываем только кнопки выбора
    public void ShowSelection()
    {
        SetSelectionButtonsActive(true);
        SetContentPanelsActive(false);
    }
    public void ShowGraphics()
    {
        SetSelectionButtonsActive(false);
        SetContentPanelsActive(false);
        if (graphics_panel != null) graphics_panel.SetActive(true);
    }
    public void ShowAudio()
    {
        SetSelectionButtonsActive(false);
        SetContentPanelsActive(false);
        if (audio_panel != null) audio_panel.SetActive(true);
    }
    public void ShowScreen()
    {
        SetSelectionButtonsActive(false);
        SetContentPanelsActive(false);
        if (screen_panel != null) screen_panel.SetActive(true);
    }
    private void SetSelectionButtonsActive(bool active)
    {
        if (graphics_button != null) graphics_button.SetActive(active);
        if (audio_button != null) audio_button.SetActive(active);
        if (screen_button != null) screen_button.SetActive(active);
    }
    private void SetContentPanelsActive(bool active)
    {
        if (graphics_panel != null) graphics_panel.SetActive(active);
        if (audio_panel != null) audio_panel.SetActive(active);
        if (screen_panel != null) screen_panel.SetActive(active);
    }
    // Для крестиков внутри панелей Графика/Аудио/Экран
    public void BackToSelection()
    {
        ShowSelection();
    }
}