using UnityEngine;

public class SpawnCurrency : MonoBehaviour
{
    [SerializeField] private Transform pfTempCurrency;
    [SerializeField] private int minAmount = default;
    [SerializeField] private int maxAmount = default;

    //===========================================================================
    public void SpewOutCurrency()
    {
        int _amount = Random.Range(minAmount, maxAmount);
        for (int i = 0; i < _amount; i++)
        {
            Vector3 _position = this.transform.position + CultyMarbleHelper.GetRandomDirection() * UnityEngine.Random.Range(0.25f, 0.75f);
            Transform currency = Instantiate(pfTempCurrency, _position, Quaternion.identity);
            currency.transform.parent = this.transform.parent;
        }
    }
}
