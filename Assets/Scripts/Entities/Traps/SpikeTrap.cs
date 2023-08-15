using UnityEngine;

public class SpikeTrap : Trap
{
    [SerializeField] private int trapDamage = default;

    //===========================================================================
    protected override void TriggerTrapEffect()
    {
        if (playerInside)
        {
            Player.Instance.GetComponent<PlayerHeartManager>().UpdateCurrentHeart(-trapDamage);
        }
    }
}
