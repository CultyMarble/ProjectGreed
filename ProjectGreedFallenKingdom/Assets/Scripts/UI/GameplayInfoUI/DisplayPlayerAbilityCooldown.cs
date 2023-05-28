using UnityEngine;
using TMPro;

public class DisplayPlayerAbilityCooldown : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CoreAbility rangeAbility;
    [SerializeField] private CoreAbility areaAbility;
    [SerializeField] private BasicAbility basicAbility;

    [SerializeField] private TextMeshProUGUI dashCooldownText;
    [SerializeField] private TextMeshProUGUI rangeAbilityCDText;
    [SerializeField] private TextMeshProUGUI areaAbilityCDText;
    [SerializeField] private TextMeshProUGUI basicAbilityFuelText;


    //===========================================================================
    private void Update()
    {
        dashCooldownText.SetText("Dash CD: " + playerMovement.GetDashCDCounter().ToString("F1"));
        rangeAbilityCDText.SetText("Range Ability CD: " + rangeAbility.CooldownTimer.ToString("F1"));
        areaAbilityCDText.SetText("AoE Ability CD: " + areaAbility.CooldownTimer.ToString("F1"));
        basicAbilityFuelText.SetText("Basic Ability Fuel: " + basicAbility.CurrentFuel.ToString("F1"));
    }
}