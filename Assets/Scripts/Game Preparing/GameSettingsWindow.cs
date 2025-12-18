using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using TMPro;
using UnityEngine;
using Xenia.ColorPicker;

public class GameSettingsWindow : MonoBehaviour
{
    [SerializeField] TMP_Dropdown enemiesCountDropdown;
    [SerializeField] TMP_Dropdown hardnessDropdown;
    [SerializeField] TMP_InputField worldSizeInputField;
    [SerializeField] EnemyDataItem enemyItemPrefab;
    [SerializeField] Transform itemsParent;
    [SerializeField] EnemyDataItem playerDataItem;
    private List<EnemyDataItem> _enemyItems = new List<EnemyDataItem>();
    private int _hardness;
    private int _worldSize;

    [SerializeField] GamePreparer gamePreparer;

    [SerializeField] ColorPicker colorPicker;
    private EnemyDataItem _currentDataItem;

    public void PrepareGame()
    {
        GameConfig gameConfig = GetConfig();
        gamePreparer.Prepare(gameConfig);
    }
    private GameConfig GetConfig()
    {
        GameConfig gameConfig = new GameConfig();

        gameConfig.PlayerData = playerDataItem.EnemyData;
        gameConfig.EnemiesData = _enemyItems.Select(x => x.EnemyData).ToList();
        gameConfig.Hardness = _hardness;
        gameConfig.WorldSize = _worldSize / 6;

        return gameConfig;
    }
    private void OnEnable()
    {
        OnEnemiesCountChanged(enemiesCountDropdown.value);
        OnHardnessChanged(hardnessDropdown.value);
        OnWorldSizeChanged(worldSizeInputField.text);

        playerDataItem.OnNameChanged("Игрок 1");
        playerDataItem.SetColor(Color.green);
        playerDataItem.ColorClickAction = OpenColorPicker;

        void OpenColorPicker()
        {
            colorPicker.Open();
            _currentDataItem = playerDataItem;
        }
    }
    public void OnHardnessChanged(int value)
        => _hardness = value;
    public void OnWorldSizeChanged(string value)
        => int.TryParse(value, out _worldSize);
    public void OnEnemiesCountChanged(int value)
    {
        value++;

        int c = _enemyItems.Count;
        while (c != value)
        {
            if (c > value)
            {
                EnemyDataItem obj = _enemyItems[^1];
                Destroy(obj.gameObject);
                _enemyItems.Remove(obj);
            }
            else
            {
                EnemyDataItem obj = Instantiate(enemyItemPrefab, itemsParent);
                obj.transform.SetSiblingIndex(4);
                _enemyItems.Add(obj);

                Color color = Random.ColorHSV();
                while (_enemyItems.Any(x => x.EnemyData.Color == color))
                {
                    color = Random.ColorHSV();
                }
                obj.SetColor(color);

                obj.OnNameChanged("Противник " + (c + 1));

                obj.ColorClickAction = OpenColorPicker;

                void OpenColorPicker()
                {
                    colorPicker.Open();
                    _currentDataItem = obj;
                }
            }
            c = _enemyItems.Count;
        }
    }

    public void OnColorPickerClosed()
    {
        if (_currentDataItem != null)
        {
            _currentDataItem.SetColor(colorPicker.CurrentColor);
        }
    }
}
