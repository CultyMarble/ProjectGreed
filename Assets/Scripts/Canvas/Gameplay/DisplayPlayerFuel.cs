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
        playerFuelFrameImage.rectTransform.localScale = new Vector3(basicAbility.MaxFuel / 100.0f, 1.0f, 1.0f);
        playerFuelBarImage.rectTransform.localScale =  new Vector3(basicAbility.CurrentFuel / 100.0f, 1.0f, 1.0f);
    }
}