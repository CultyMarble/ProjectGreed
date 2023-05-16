using UnityEngine;

public class EnemyStatusEffect : MonoBehaviour
{
    [SerializeField] private Transform hostEntity;
    [SerializeField] private SpriteRenderer effectVFX;

    public Transform Host { get => hostEntity; private set { } }
    public SpriteRenderer EffectVFX { get => effectVFX; private set { } }
}