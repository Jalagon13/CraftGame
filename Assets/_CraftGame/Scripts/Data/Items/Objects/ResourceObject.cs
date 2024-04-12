using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Create Item/New Resource")]
public class ResourceObject : ItemObject
{
    public override string GetDescription()
    {
        return string.Empty;
    }
}
