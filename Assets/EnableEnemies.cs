using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableEnemies : MonoBehaviour
{
    public GameObject enemies;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemies.gameObject.SetActive(true);
    }
}
