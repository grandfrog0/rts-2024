using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGeneration : MonoBehaviour
{
    [SerializeField] int worldSize = 256;
    [SerializeField] float spawnRate = 0.5f;
    [SerializeField] float threshold = 0.75f;
    [SerializeField] GameObject rockPrefab, treePrefab;
    [SerializeField] Transform resourcesParent;
    private float[,] _map;

    private void Start()
    {
        GenerateMap();
        SpawnResources();
    }

    public void SpawnResources()
    {
        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                if (_map[x, y] < threshold)
                    continue;

                GameObject resource = _map[x, y] > threshold + 0.1f ? rockPrefab : treePrefab;

                Instantiate(resource, new Vector3(x - worldSize / 2, 0, y - worldSize / 2), Quaternion.Euler(0, Random.Range(0, 360), 0), resourcesParent);
            }
        }
    }

    public void GenerateMap()
    {
        _map = new float[worldSize, worldSize];

        for (int y = 0; y < worldSize; y++)
        {
            for (int x = 0; x < worldSize; x++)
            {
                _map[x, y] = Mathf.PerlinNoise((float)x / worldSize * 15, (float)y / worldSize * 15) * spawnRate;
            }
        }

        string text = "";
        foreach (float el in _map)
            text += el + "; ";
        Debug.Log(text);
    }
}