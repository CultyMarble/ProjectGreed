using UnityEngine;

public class SpikeTrap : Trap
{
    [SerializeField] private int trapDamage = default;
    [SerializeField] private bool noDamage = false;


    //===========================================================================
    protected override void TriggerTrapEffect()
    {
        if (noDamage)
            return;

        if (playerInside)
        {
            Player.Instance.GetComponent<PlayerHeart>().UpdateCurrentHeart(-trapDamage);
        }
    }
}
