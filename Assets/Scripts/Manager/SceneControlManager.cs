using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControlManager : SingletonMonobehaviour<SceneControlManager>
{
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Image loadingScreenImage;

    [SerializeField] private GameObject player;

    [Header("Starting Scene:")]
    [SerializeField] private SceneName startingScene;
    [SerializeField] private Transform startingPosition;

    [Header("Menu Settings:")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private PauseMenuGUI pauseMenu;

    [Header("Gameover Menu")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button gv_respawnButton;
    [SerializeField] private Button gv_mainMenuButton;

    private readonly float loadingScreenDuration = 0.75f;

    private bool isLoadingScreenActive = default;
    public bool IsLoadingScreenActive => isLoadingScreenActive;

    private bool isLoadingScene = default;
    public bool IsLoadingScene => isLoadingScene;

    public GameState CurrentGameState = default;

    //===========================================================================
    private void OnEnable()
    {
        // Gameover Menu
        gv_mainMenuButton.onClick.AddListener(() => StartCoroutine(BackToMainMenu()));
        gv_respawnButton.onClick.AddListener(() => StartCoroutine(UnloadAndSwitchScene(SceneName.DemoSceneHub.ToString(), Vector3.zero)));
    }

    private void Start()
    {
        // First Time Load
        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        loadingScreenCanvasGroup.alpha = 1.0f;

        mainMenu.SetActive(true);

        StartCoroutine(LoadingScreen(0.0f));
    }
    //===========================================================================
    private IEnumerator UnloadAndSwitchScene(string sceneName, Vector3 spawnPosition)
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        Player.Instance.transform.position = spawnPosition;
        player.SetActive(true);
        gameOverMenu.SetActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventManager.CallAfterSceneLoadEvent();

        yield return new WaitForSecondsRealtime(1.0f);
        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        isLoadingScene = false;
    }

    //===========================================================================
    private IEnumerator LoadingScreen(float targetAlpha)
    {
        isLoadingScreenActive = true;

        loadingScreenCanvasGroup.blocksRaycasts = true;

        float _loadSpeed = Mathf.Abs(loadingScreenCanvasGroup.alpha - targetAlpha) / loadingScreenDuration;

        while (Mathf.Approximately(loadingScreenCanvasGroup.alpha, targetAlpha) == false)
        {
            loadingScreenCanvasGroup.alpha = Mathf.MoveTowards(loadingScreenCanvasGroup.alpha, targetAlpha, _loadSpeed * Time.deltaTime);
            yield return null;
        }

        isLoadingScreenActive = false;
        loadingScreenCanvasGroup.blocksRaycasts = false;
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    //===========================================================================
    private IEnumerator LoadStartingScene()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        mainMenu.SetActive(false);
        Player.Instance.gameObject.SetActive(true);

        yield return StartCoroutine(LoadSceneAndSetActive(startingScene.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        Player.Instance.transform.position = startingPosition.position;

        StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameState = GameState.GameplayHub;
    }

    private IEnumerator BackToMainMenu()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);
        pauseMenu.SetPauseMenuActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameState = GameState.MainMenu;
    }

    //===========================================================================
    public void LoadStartingSceneWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            StartCoroutine(LoadStartingScene());
        }
    }

    public void BackToMainMenuWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            StartCoroutine(BackToMainMenu());
            CurrentGameState = GameState.MainMenu;
        }
    }
    public void Respawn()
    {
        if (isLoadingScreenActive == false)
        {
            pauseMenu.SetPauseMenuActive(false);
            StartCoroutine(UnloadAndSwitchScene(SceneName.DemoSceneHub.ToString(), Vector3.zero));
        }
    }

    public void LoadScene(string sceneName, Vector3 spawnPosition)
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            StartCoroutine(UnloadAndSwitchScene(sceneName, spawnPosition));

            if (sceneName == SceneName.DemoSceneDungeon.ToString())
                CurrentGameState = GameState.GameplayDungeon;
        }
    }
}