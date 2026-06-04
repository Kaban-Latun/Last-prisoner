using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("Панели настроек")]
    [SerializeField] private GameObject graphics_panel;
    [SerializeField] private GameObject audio_panel;
    [SerializeField] private GameObject screen_panel;

    private void Start()
    {
        // По умолчанию открываем первую вкладку - Графика
        ShowGraphics();
    }

    public void ShowGraphics()
    {
        ActivatePanel(graphics_panel);

        if (graphics_panel != null)
        {
            foreach (Transform child in graphics_panel.transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    public void ShowAudio()
    {
        ActivatePanel(audio_panel);
    }

    public void ShowScreen()
    {
        ActivatePanel(screen_panel);
    }

    private void ActivatePanel(GameObject panelToShow)
    {
        if (graphics_panel != null) graphics_panel.SetActive(false);
        if (audio_panel != null) audio_panel.SetActive(false);
        if (screen_panel != null) screen_panel.SetActive(false);

        if (panelToShow != null)
            panelToShow.SetActive(true);
    }
}