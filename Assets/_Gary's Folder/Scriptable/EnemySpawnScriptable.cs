using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnData", menuName = "EnemySpawnScriptable", order = 1)]
public class EnemySpawnScriptable : ScriptableObject
{
    [Header("Enemies Variants")]
    public GameObject[] commonEnemies;

}
