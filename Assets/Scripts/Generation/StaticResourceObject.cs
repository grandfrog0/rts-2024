using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StaticResourceObject : MonoBehaviour
{
    private Renderer _renderer;
    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void OnBecameVisible()
    {
        _renderer.enabled = true;
    }
    public void OnBecameInvisible()
    {
        Destroy(gameObject);
        _renderer.enabled = false;
    }
}
