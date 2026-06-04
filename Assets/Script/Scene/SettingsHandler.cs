using UnityEngine;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [Header("Графика")]
    [SerializeField] private Slider contrast_slider;
    [SerializeField] private Slider brightness_slider;
    [SerializeField] private Toggle bloom_toggle;
    [SerializeField] private Dropdown quality_dropdown;        // Legacy Dropdown

    [Header("Аудио")]
    [SerializeField] private Slider main_slider;
    [SerializeField] private Slider voice_slider;
    [SerializeField] private Slider ambient_slider;

    [Header("Экран")]
    [SerializeField] private Dropdown resolution_dropdown;     // Legacy
    [SerializeField] private Dropdown screen_dropdown;         // Legacy
    [SerializeField] private Toggle v_sync_toogle;

    [Header("Текстовые значения (Legacy Text)")]
    [SerializeField] private Text contrastValueText;
    [SerializeField] private Text brightnessValueText;
    [SerializeField] private Text mainValueText;
    [SerializeField] private Text voiceValueText;
    [SerializeField] private Text ambientValueText;

    private void Start()
    {
        SetupAllControls();
        LoadSettings();
    }

    private void SetupAllControls()
    {
        // Графика
        SetupSlider(contrast_slider, UpdateContrast, contrastValueText);
        SetupSlider(brightness_slider, UpdateBrightness, brightnessValueText);

        if (bloom_toggle != null)
            bloom_toggle.onValueChanged.AddListener(UpdateBloom);

        if (quality_dropdown != null)
            quality_dropdown.onValueChanged.AddListener(UpdateQuality);

        // Аудио
        SetupSlider(main_slider, UpdateMainVolume, mainValueText);
        SetupSlider(voice_slider, UpdateVoiceVolume, voiceValueText);
        SetupSlider(ambient_slider, UpdateAmbientVolume, ambientValueText);

        // Экран
        if (resolution_dropdown != null)
            resolution_dropdown.onValueChanged.AddListener(UpdateResolution);

        if (screen_dropdown != null)
            screen_dropdown.onValueChanged.AddListener(UpdateScreenMode);

        if (v_sync_toogle != null)
            v_sync_toogle.onValueChanged.AddListener(UpdateVSync);
    }

    private void SetupSlider(Slider slider, UnityEngine.Events.UnityAction<float> listener, Text valueText)
    {
        if (slider != null)
        {
            slider.minValue = 0;
            slider.maxValue = 100;
            slider.onValueChanged.AddListener(listener);

            if (valueText != null)
                valueText.text = Mathf.RoundToInt(slider.value).ToString();
        }
    }

    // ====================== ОБРАБОТЧИКИ ======================

    private void UpdateContrast(float value)
    {
        if (contrastValueText != null) contrastValueText.text = Mathf.RoundToInt(value).ToString();
        PlayerPrefs.SetFloat("Contrast", value);
    }

    private void UpdateBrightness(float value)
    {
        if (brightnessValueText != null) brightnessValueText.text = Mathf.RoundToInt(value).ToString();
        PlayerPrefs.SetFloat("Brightness", value);
    }

    private void UpdateBloom(bool isOn)
    {
        PlayerPrefs.SetInt("Bloom", isOn ? 1 : 0);
    }

    private void UpdateQuality(int index)
    {
        PlayerPrefs.SetInt("Quality", index);
        QualitySettings.SetQualityLevel(index);
    }

    private void UpdateMainVolume(float value)
    {
        if (mainValueText != null) mainValueText.text = Mathf.RoundToInt(value).ToString();
        PlayerPrefs.SetFloat("MainVolume", value);
    }

    private void UpdateVoiceVolume(float value)
    {
        if (voiceValueText != null) voiceValueText.text = Mathf.RoundToInt(value).ToString();
        PlayerPrefs.SetFloat("VoiceVolume", value);
    }

    private void UpdateAmbientVolume(float value)
    {
        if (ambientValueText != null) ambientValueText.text = Mathf.RoundToInt(value).ToString();
        PlayerPrefs.SetFloat("AmbientVolume", value);
    }

    private void UpdateResolution(int index)
    {
        PlayerPrefs.SetInt("Resolution", index);
    }

    private void UpdateScreenMode(int index)
    {
        PlayerPrefs.SetInt("ScreenMode", index);
    }

    private void UpdateVSync(bool isOn)
    {
        PlayerPrefs.SetInt("VSync", isOn ? 1 : 0);
        QualitySettings.vSyncCount = isOn ? 1 : 0;
    }

    private void LoadSettings()
    {
        if (contrast_slider != null) contrast_slider.value = PlayerPrefs.GetFloat("Contrast", 50f);
        if (brightness_slider != null) brightness_slider.value = PlayerPrefs.GetFloat("Brightness", 50f);
        if (bloom_toggle != null) bloom_toggle.isOn = PlayerPrefs.GetInt("Bloom", 1) == 1;
        if (quality_dropdown != null) quality_dropdown.value = PlayerPrefs.GetInt("Quality", 2);

        if (main_slider != null) main_slider.value = PlayerPrefs.GetFloat("MainVolume", 80f);
        if (voice_slider != null) voice_slider.value = PlayerPrefs.GetFloat("VoiceVolume", 80f);
        if (ambient_slider != null) ambient_slider.value = PlayerPrefs.GetFloat("AmbientVolume", 70f);

        if (resolution_dropdown != null) resolution_dropdown.value = PlayerPrefs.GetInt("Resolution", 0);
        if (screen_dropdown != null) screen_dropdown.value = PlayerPrefs.GetInt("ScreenMode", 1);
        if (v_sync_toogle != null) v_sync_toogle.isOn = PlayerPrefs.GetInt("VSync", 1) == 1;
    }
}