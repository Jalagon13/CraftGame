using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public Transform pfDamagePopup;

    private static GameAssets _i;

    public static GameAssets Instance
    {
        get
        {
            if (_i == null)
                _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
            return _i;
        }
    }
}
