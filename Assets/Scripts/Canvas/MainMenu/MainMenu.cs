using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Animation Settings:")]
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup = default;
    [SerializeField] private Image loadingScreenImage = default;
    private readonly float loadingScreenDuration = 0.5f;

    [SerializeField] private Image mmBGImage = default;
    [SerializeField] private Sprite[] mmBGImages = default;

    private readonly float animationSpeed = 0.2f;
    private float animationTimer = default;
    private int currentIndex = default;

    [Header("Button Settings:")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button exitButton;

    //===========================================================================
    private void OnEnable()
    {
        startGameButton.onClick.AddListener(() =>
        {
            StartCoroutine(LoadingScreen(1.0f));
            SceneControlManager.Instance.LoadScene(SceneName.DemoSceneHub, Vector3.zero);
        });

        exitButton.onClick.AddListener(ExitGameButton);
    }

    private void Start()
    {
        // First Time Load
        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        loadingScreenCanvasGroup.alpha = 1.0f;

        StartCoroutine(LoadingScreen(0.0f));
    }

    private void Update()
    {
        PlayAnimation();
    }

    //===========================================================================
    private IEnumerator LoadingScreen(float targetAlpha)
    {
        loadingScreenCanvasGroup.blocksRaycasts = true;

        float _loadSpeed = Mathf.Abs(loadingScreenCanvasGroup.alpha - targetAlpha) / loadingScreenDuration;
        while (Mathf.Approximately(loadingScreenCanvasGroup.alpha, targetAlpha) == false)
        {
            loadingScreenCanvasGroup.alpha = Mathf.MoveTowards(loadingScreenCanvasGroup.alpha, targetAlpha, _loadSpeed * Time.deltaTime);
            yield return null;
        }

        loadingScreenCanvasGroup.blocksRaycasts = false;
    }

    private void PlayAnimation()
    {
        animationTimer += Time.deltaTime;
        if (animationTimer >= animationSpeed)
        {
            animationTimer -= animationSpeed;

            if (currentIndex == mmBGImages.Length)
                currentIndex = 0;

            mmBGImage.sprite = mmBGImages[currentIndex];
            currentIndex++;
        }
    }

    public void ExitGameButton()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}