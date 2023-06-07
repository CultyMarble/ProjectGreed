using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGUI : MonoBehaviour
{
    [SerializeField] private Image mainMenuBGImage = default;
    [SerializeField] private Sprite[] images = default;

    private readonly float effectAnimationSpeed = 0.15f;
    private float effectAnimationTimer = default;
    private int currentAnimationIndex = default;

    //===========================================================================
    private void Update()
    {
        BackgroundAnimation();
    }

    //===========================================================================
    private void BackgroundAnimation()
    {
        effectAnimationTimer += Time.deltaTime;
        if (effectAnimationTimer >= effectAnimationSpeed)
        {
            effectAnimationTimer -= effectAnimationSpeed;

            mainMenuBGImage.sprite = images[currentAnimationIndex];

            currentAnimationIndex++;

            if (currentAnimationIndex == images.Length)
                currentAnimationIndex = 0;
        }
    }
}
