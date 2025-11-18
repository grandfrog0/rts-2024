using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseGenerator : MonoBehaviour
{
    private int _worldSize;
    private float _baseRange;
    [SerializeField] List<string> basesNames;
    [SerializeField] GameObject townHall;
    [SerializeField] GameObject worker, archer;
    [SerializeField] TMP_Text titleText;
    [SerializeField] Transform unitsParent;
    [SerializeField] Transform uiParent;
    private List<Vector2> _basesPositions = new();
    public List<Vector2> BasesPosition => _basesPositions;

    public void Initialize(int worldSize, float baseRange)
    {
        _worldSize = worldSize;
        _baseRange = baseRange;

        GenerateBases();
        SpawnBases();
    }
    private void GenerateBases()
    {
        _basesPositions.Clear();

        float angleDelta = -360 / basesNames.Count;

        float currentAngle = Random.Range(0, 360);
        Vector2 currentPos = Quaternion.Euler(0, 0, currentAngle) * Vector2.up * (_worldSize / 2 - _baseRange * 2);
        _basesPositions.Add(currentPos);
        
        for (int i = 0; i < basesNames.Count - 1; i++)
        {
            currentAngle += angleDelta;
            currentPos = Quaternion.Euler(0, 0, currentAngle) * Vector2.up * (_worldSize / 2 - _baseRange * 2);
            _basesPositions.Add(currentPos);
        }
    }
    private void SpawnBases()
    {
        Vector2 position;
        string title;

        for (int i = 0; i < _basesPositions.Count; i++)
        {
            position = _basesPositions[i];
            title = basesNames[i];

            Instantiate(townHall, new Vector3(position.x, 1, position.y), Quaternion.Euler(0, 180, 0), unitsParent);

            Instantiate(worker, new Vector3(position.x - 2, 0, position.y - 3), Quaternion.identity, unitsParent);
            Instantiate(worker, new Vector3(position.x - 1, 0, position.y - 3), Quaternion.identity, unitsParent);
            Instantiate(worker, new Vector3(position.x, 0, position.y - 3), Quaternion.identity, unitsParent);

            Instantiate(archer, new Vector3(position.x + 1, 0, position.y - 3), Quaternion.identity, unitsParent);
            Instantiate(archer, new Vector3(position.x + 2, 0, position.y - 3), Quaternion.identity, unitsParent);

            TMP_Text text = Instantiate(titleText, uiParent);
            text.rectTransform.anchoredPosition = position;
            text.text = title;
        }
    }
}
