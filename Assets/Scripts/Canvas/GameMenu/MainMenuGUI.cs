using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuGUI : SingletonMonobehaviour<MainMenuGUI>
{
    [Header("Main Menu Animation:")]
    [SerializeField] private Image mainMenuBGImage = default;
    [SerializeField] private Sprite[] images = default;

    private readonly float effectAnimationSpeed = 0.15f;
    private float effectAnimationTimer = default;
    private int currentAnimationIndex = default;

    [Header("Button Settings:")]
    [SerializeField] private Button mm_startButton = default;
    [SerializeField] private Button mm_exitButton = default;

    [Header("Content")]
    [SerializeField] private GameObject content = default;

    //===========================================================================
    private void Start()
    {
        // Main Menu
        mm_startButton.onClick.AddListener(() =>
        {
            SetActive(false);
            SaveSelectMenuGUI.Instance.SetActive(true);
        });

        mm_exitButton.onClick.AddListener(Application.Quit);
    }

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

    //===========================================================================
    public void SetActive(bool active)
    {
        content.SetActive(active);
    }
}