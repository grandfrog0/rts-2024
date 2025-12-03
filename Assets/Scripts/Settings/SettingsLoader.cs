using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsLoader : MonoBehaviour
{
    [SerializeField] List<Vector2Int> screenSizes;

    [SerializeField] TMP_Dropdown screenSizeDropdown;
    [SerializeField] ToggleSwitch windowModeToggle;
    [SerializeField] ToggleSwitch musicToggle;
    [SerializeField] Slider volumeSlider;

    private int _screenSize;
    private bool _isWindowed;
    private bool _isMusicOn;
    private float _volume;

    public void Load()
    {
        Clear();
        HandleUI();

        // load data from file
    }
    public void HandleUI()
    {
        screenSizeDropdown.SetValueWithoutNotify(_screenSize);
        windowModeToggle.IsOn = _isWindowed;
        musicToggle.IsOn = _isMusicOn;
        volumeSlider.value = _volume;
    }
    public void Save()
    {
        Screen.SetResolution(screenSizes[_screenSize].x, screenSizes[_screenSize].y, Screen.fullScreen);
        Screen.fullScreenMode = _isWindowed ? FullScreenMode.Windowed : FullScreenMode.FullScreenWindow;

        // save data to file
    }
    public void Clear()
    {
        _screenSize = 0;
        _isWindowed = false;
        _isMusicOn = true;
        _volume = 0.5f;
    }

    public void OnScreenSizeChanged(int value)
    {
        _screenSize = value;
    }
    public void OnWindowModeChanged(bool value)
    {
        _isWindowed = value;
    }
    public void OnMusicChanged(bool value)
    {
        _isMusicOn = value;
    }
    public void OnVolumeChanged(float value)
    {
        _volume = value;
    }

    private void Start()
    {
        Load();
    }
}
