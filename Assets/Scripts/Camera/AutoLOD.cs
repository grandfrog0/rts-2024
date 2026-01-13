using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LODGroup))]
public class AutoLOD : MonoBehaviour
{
    private void OnValidate()
    {
        LODGroup group = GetComponent<LODGroup>();
        group.SetLODs(new LOD[]{ new(0.03f, transform.GetComponentsInChildren<Renderer>()) });
    }
}
