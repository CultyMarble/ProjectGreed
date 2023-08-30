using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuGUI : MonoBehaviour
{
    [Header("Main Menu Animation:")]
    [SerializeField] private Image mainMenuBGImage = default;
    [SerializeField] private Sprite[] images = default;

    private readonly float effectAnimationSpeed = 0.15f;
    private float effectAnimationTimer = default;
    private int currentAnimationIndex = default;

    [Header("Button Settings:")]
    [SerializeField] private Button mm_startButton = default;
    [SerializeField] private TextMeshProUGUI mm_startButtonText = default;
    [SerializeField] private Button mm_exitButton = default;

    private readonly float targetAlphaMax = 1.0f;
    private readonly float targetAlphaMin = 0.1f;
    private readonly float fadeSpeed = 1.25f;

    private float targetAlpha = 0.1f;

    //===========================================================================
    private void Start()
    {
        // Main Menu
        mm_startButton.onClick.AddListener(() => SceneControlManager.Instance.LoadStartingSceneWrapper());

        mm_exitButton.onClick.AddListener(() => Application.Quit());
    }

    private void Update()
    {
        BackgroundAnimation();

        StartButtonInteract();
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

    private void StartButtonInteract()
    {
        mm_startButtonText.alpha = Mathf.MoveTowards(mm_startButtonText.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

        if (Mathf.Abs(mm_startButtonText.alpha - targetAlpha) <= 0.01f)
        {
            mm_startButtonText.alpha = targetAlpha;

            if (mm_startButtonText.alpha == targetAlphaMax)
            {
                targetAlpha = targetAlphaMin;
            }
            else if (mm_startButtonText.alpha == targetAlphaMin)
            {
                targetAlpha = targetAlphaMax;
            }
        }
    }
}
