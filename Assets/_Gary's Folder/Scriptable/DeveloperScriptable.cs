using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeveloperData", menuName = "DeveloperScriptable", order = 1)]
public class DeveloperScriptable : ScriptableObject
{
    [Header("DEVELOPER MODE [TURN IT OFF IN BUILD!]")]
    public bool developerMode = false;

    [Space]

    [Header("Player Spawn")]
    public Vector2 playerSpawnPosition;

}
