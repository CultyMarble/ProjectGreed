using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyRoomManager : MonoBehaviour
{
    [SerializeField] private GameObject[] commonEnemies;

    public void SpawnRandomEnemies(Transform location)
    {
        int random = Random.Range(0, commonEnemies.Length);
        Instantiate(commonEnemies[random], location);
    }

}
