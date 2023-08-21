using UnityEngine;
using TMPro;

public class DisplayPlayerBombAbilityCharge : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private BombAbility bombAbility = default;
    [SerializeField] private TextMeshProUGUI chargeText = default;

    //===========================================================================
    private void OnEnable()
    {
        bombAbility.OnChargeChangedEvent += BombAbility_OnChargeChangedEvent; ;
    }

    private void OnDisable()
    {
        bombAbility.OnChargeChangedEvent -= BombAbility_OnChargeChangedEvent; ;
    }

    //===========================================================================
    private void BombAbility_OnChargeChangedEvent(object sender, BombAbility.OnChargeChangedEventArgs e)
    {
        chargeText.SetText("x" +  e.charge.ToString());
    }
}
