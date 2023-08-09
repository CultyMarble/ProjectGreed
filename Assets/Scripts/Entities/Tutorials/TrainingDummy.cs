using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    [SerializeField] private float appliedDamage = default;

    //======================================================================
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            StatusEffectPoison _poison = GetComponentInChildren<StatusEffectPoison>();
            _poison.SetDamageFactor(0);
            _poison.SetAppliedDamage(appliedDamage);
            _poison.SetDuration();
            _poison.enabled = true;
        }
    }
}
