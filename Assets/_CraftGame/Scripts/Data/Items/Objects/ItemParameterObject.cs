using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemParameterObject : ScriptableObject
{
    [field: SerializeField] public string ParameterName { get; private set; }
}
