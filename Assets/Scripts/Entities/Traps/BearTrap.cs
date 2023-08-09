using UnityEngine;

public class BearTrap : Trap
{
    [SerializeField] private int trapDamage = default;

    //===========================================================================
    protected override void TriggerTrapEffect()
    {
        if (playerInside)
        {
            Player.Instance.gameObject.transform.position = this.transform.position;
            Player.Instance.GetComponent<PlayerHealth>().UpdateCurrentHealth(-trapDamage);
        }
    }
}
