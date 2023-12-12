using UnityEngine;
using UnityEngine.UI;

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
        mm_startButton.onClick.AddListener(SceneControlManager.Instance.LoadDemoSceneHubWrapper);

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
    public void SetContentActive(bool active)
    {
        content.SetActive(active);
    }
}