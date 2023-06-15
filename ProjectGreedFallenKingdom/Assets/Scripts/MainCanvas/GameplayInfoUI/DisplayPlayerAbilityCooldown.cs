using UnityEngine;
using TMPro;

public class DisplayPlayerAbilityCooldown : MonoBehaviour
{
    [SerializeField] private PlayerController playerMovement;

    [SerializeField] private CoreAbility rangeAbility;
    [SerializeField] private CoreAbility areaAbility;
    [SerializeField] private BasicAbility basicAbility;

    [SerializeField] private TextMeshProUGUI dashCooldownText;
    [SerializeField] private TextMeshProUGUI rangeAbilityCDText;
    [SerializeField] private TextMeshProUGUI areaAbilityCDText;

    //===========================================================================
    private void Update()
    {
        dashCooldownText.SetText("[Space] Dash CD: " + playerMovement.GetDashCDCounter().ToString("F1"));
        rangeAbilityCDText.SetText("[Right-click] Range Ability CD: " + rangeAbility.CooldownTimer.ToString("F1"));
        areaAbilityCDText.SetText("[Q] AoE Ability CD: " + areaAbility.CooldownTimer.ToString("F1"));
    }
}