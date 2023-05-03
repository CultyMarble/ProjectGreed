using UnityEngine;

public class EnemyStatusEffect : MonoBehaviour
{
    [SerializeField] private Transform hostEntity;

    public Transform HostEntity
    {
        get => hostEntity;
        private set { }
    }
}