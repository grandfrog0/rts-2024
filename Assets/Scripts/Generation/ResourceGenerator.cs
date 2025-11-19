using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    private int _worldSize;
    [SerializeField] float spawnRate = 0.5f;
    [SerializeField] float threshold = 0.75f;
    [SerializeField] GameObject rockPrefab, treePrefab;
    [SerializeField] Transform resourcesParent;
    private float[,] _map;
    private List<Vector2> _bases = new();
    private float _baseRange;

    public void Initialize(int worldSize, List<Vector2> bases, float baseRange)
    {
        _worldSize = worldSize;

        foreach(Vector2 position in bases)
            _bases.Add(new Vector2(Mathf.RoundToInt(position.x + _worldSize / 2), Mathf.RoundToInt(position.y + _worldSize / 2)));

        _baseRange = baseRange;

        GenerateMap();
        SpawnResources();
    }

    private void SpawnResources()
    {
        for (int y = 0; y < _worldSize; y++)
        {
            for (int x = 0; x < _worldSize; x++)
            {
                if (_map[x, y] < threshold)
                    continue;

                GameObject resource = _map[x, y] > threshold + 0.075f ? rockPrefab : treePrefab;

                Instantiate(resource, new Vector3(x - _worldSize / 2, 0, y - _worldSize / 2), Quaternion.Euler(0, Random.Range(0, 360), 0), resourcesParent);
            }
        }
    }

    private void GenerateMap()
    {
        _map = new float[_worldSize, _worldSize];

        for (int y = 0; y < _worldSize; y++)
        {
            for (int x = 0; x < _worldSize; x++)
            {
                if (!IsInsideBase(x, y))
                    _map[x, y] = Mathf.PerlinNoise((float)x / 2.88f, (float)y / 2.88f) * spawnRate;
            }
        }
    }

    public bool IsInsideBase(int x, int y)
    {
        foreach(Vector2 pos in _bases)
            if (Vector2.Distance(new Vector2(x, y), pos) <= _baseRange)
                return true;
        return false;
    }
}