using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StaticResourceObject : MonoBehaviour
{
    private void Awake()
    {
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.SetPropertyBlock(block);
    }
}
