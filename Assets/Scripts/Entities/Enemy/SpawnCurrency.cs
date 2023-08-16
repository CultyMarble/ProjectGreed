using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCurrency : MonoBehaviour
{
    [SerializeField] private Transform pfTempCurrency;

    private int minAmount = 3;
    private int maxAmount = 7;

    private void Start()
    {
        GetComponent<EnemyHealth>().OnDespawnEvent += SpawnCurrency_OnDead;
    }

    private void SpawnCurrency_OnDead(object sender, System.EventArgs e)
    {
        SpewOutCurrency();
    }

    private void SpewOutCurrency()
    {
        GameObject folder = GameObject.Find("Collectable Items");
        int _amount = UnityEngine.Random.Range(minAmount, maxAmount);
        for (int i = 0; i < _amount; i++)
        {
            Vector3 _position = this.transform.position + CultyMarbleHelper.GetRandomDirection() * UnityEngine.Random.Range(0.25f, 0.75f);
            Transform currency = Instantiate(pfTempCurrency, _position, Quaternion.identity);
            currency.parent = folder.transform;
        }
    }
}
