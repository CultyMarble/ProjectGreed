using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerFuel : MonoBehaviour
{
    [Header("Component References:")]
    [SerializeField] private BasicAbility basicAbility;

    [Header("Canvas References:")]
    [SerializeField] private Image playerFuelFrameImage;
    [SerializeField] private Image playerFuelBarImage;

    //===========================================================================
    private void Update()
    {
        playerFuelBarImage.rectTransform.localScale = 
            new Vector3(basicAbility.CurrentFuel / basicAbility.MaxFuel, 1.0f, 1.0f);
    }
}