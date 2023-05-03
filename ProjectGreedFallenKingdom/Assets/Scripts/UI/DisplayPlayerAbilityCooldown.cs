using UnityEngine;
using TMPro;

public class DisplayPlayerAbilityCooldown : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CoreAbility rangeAbility;
    [SerializeField] private CoreAbility areaAbility;

    [SerializeField] private TextMeshProUGUI dashCooldownText;
    [SerializeField] private TextMeshProUGUI rangeAbilityCDText;
    [SerializeField] private TextMeshProUGUI areaAbilityCDText;

    //===========================================================================
    private void Update()
    {
        dashCooldownText.SetText("Dash CD: " + playerMovement.GetDashCDCounter().ToString("F1"));
        rangeAbilityCDText.SetText("Range Ability CD: " + rangeAbility.CooldownTimer.ToString("F1"));
        areaAbilityCDText.SetText("Range Ability CD: " + areaAbility.CooldownTimer.ToString("F1"));
    }
}