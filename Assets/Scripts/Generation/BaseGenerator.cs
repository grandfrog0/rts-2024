using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseGenerator : MonoBehaviour
{
    private int _worldSize;
    private float _baseRange;
    private List<string> _basesNames;
    [SerializeField] GameObject townHall;
    [SerializeField] GameObject worker, archer;
    [SerializeField] TMP_Text titleText;
    [SerializeField] Transform uiParent;
    private List<Vector2> _basesPositions = new();
    public List<Vector2> BasesPosition => _basesPositions;

    public void Initialize(GameConfig gameConfig, float baseRange, GenerationData generationData)
    {
        _worldSize = gameConfig.WorldSize;
        _basesNames = new() { gameConfig.PlayerData.Name };
        _basesNames.AddRange(gameConfig.EnemiesData.Select(x => x.Name));

        _baseRange = baseRange;

        GenerateBases();
        SpawnBases();

        generationData.PlayerBase = _basesPositions[0];
        generationData.EnemyBases = _basesPositions.GetRange(1, _basesPositions.Count - 1);
    }
    private void GenerateBases()
    {
        _basesPositions.Clear();

        float angleDelta = -360 / _basesNames.Count;

        float currentAngle = Random.Range(0, 360);
        float distanceMultiplier = (_worldSize / 2 - _baseRange * 8);
        Vector2 currentPos = Quaternion.Euler(0, 0, currentAngle) * Vector2.up * distanceMultiplier;
        _basesPositions.Add(currentPos);
        
        for (int i = 0; i < _basesNames.Count - 1; i++)
        {
            currentAngle += angleDelta;
            currentPos = Quaternion.Euler(0, 0, currentAngle) * Vector2.up * distanceMultiplier;
            _basesPositions.Add(currentPos);
        }
    }
    private void SpawnBases()
    {
        Vector2 position;
        string title;

        Quaternion rotation = Quaternion.Euler(0, 180, 0);
        Quaternion towerRotation = Quaternion.Euler(0, 0, 0);

        for (int i = 0; i < _basesPositions.Count; i++)
        {
            position = _basesPositions[i];
            title = _basesNames[i];

            EntitySpawner.Spawn(townHall, new Vector3(position.x, 0, position.y), towerRotation, i);

            EntitySpawner.Spawn(worker, new Vector3(position.x - 2, 0, position.y - 3), rotation, i);
            EntitySpawner.Spawn(worker, new Vector3(position.x - 1, 0, position.y - 3), rotation, i);
            EntitySpawner.Spawn(worker, new Vector3(position.x, 0, position.y - 3), rotation, i);
            
            EntitySpawner.Spawn(archer, new Vector3(position.x + 1, 0, position.y - 3), rotation, i);
            EntitySpawner.Spawn(archer, new Vector3(position.x + 2, 0, position.y - 3), rotation, i);

            TMP_Text text = Instantiate(titleText, uiParent);
            text.rectTransform.anchoredPosition = position;
            text.text = title;
        }
    }
}
