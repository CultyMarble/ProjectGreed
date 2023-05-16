using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SceneName
{
    Scene01_ManagerScene,
    Scene02_TutorialScene,
    Scene03_HubArea,
    Scene04_Room1,
    Scene05_Room1_5,
    Scene06_Room2,
    Scene07_Room3,
    Scene08_Room4,
    Scene09_Room5,
    Scene10_Room6,
    Scene11_Room7,
    Scene12_BossRoom,
}

public class SceneControlManager : SingletonMonobehaviour<SceneControlManager>
{
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Image loadingScreenImage;
    [SerializeField] private float loadingScreenDuration = 1.0f;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] public GameObject pauseMenuUI;

    [Header("Button References:")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button mainMenuButtonFromDemoOver;
    [SerializeField] private Button mainMenuButtonInPauseMenu;
    [SerializeField] private Button exitButton;

    [Header("Starting Scene:")]
    [SerializeField] private SceneName startingScene;

    private bool isLoadingScreenActive;
    private bool canUnload = false;
    private UnloadSceneZone unloadSceneZone;
    //===========================================================================
    private void Start()
    {
        startGameButton.onClick.AddListener(() =>
        {
            StartCoroutine(LoadStartingScene());
        });

        tutorialButton.onClick.AddListener(() =>
        {
            StartCoroutine(LoadTutorialRoom());
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            StartCoroutine(UnloadSceneAndBackToMainMenu());
        });

        mainMenuButtonInPauseMenu.onClick.AddListener(() =>
        {
            StartCoroutine(UnloadSceneAndBackToMainMenu());
        });

        mainMenuButtonInPauseMenu.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        // First Time Load
        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        loadingScreenCanvasGroup.alpha = 1.0f;

        mainMenuUI.SetActive(true);

        StartCoroutine(LoadingScreen(0.0f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && player.activeInHierarchy == true)
        {
            if (pauseMenuUI.activeSelf == false)
            {
                Time.timeScale = 1.0f;
                pauseMenuUI.SetActive(true);
            }
            else if (pauseMenuUI.activeSelf == true)
            {
                Time.timeScale = 0.0f;
                pauseMenuUI.SetActive(false);
            }
        }

        if (unloadSceneZone == null)
            return;

        this.canUnload = unloadSceneZone.canUnload;

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (canUnload)
                StartCoroutine(UnloadSceneAndBackToMainMenu());
        }
    }

    //===========================================================================
    public void LoadScene(string sceneName, Vector3 spawnPosition)
    {
        if (isLoadingScreenActive == false)
        {
            StartCoroutine(UnloadAndSwitchScene(sceneName, spawnPosition));
        }
    }

    //===========================================================================
    private IEnumerator UnloadAndSwitchScene(string sceneName, Vector3 spawnPosition)
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));
        player.transform.position = spawnPosition;

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventManager.CallAfterSceneLoadEvent();

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();
    }

    public IEnumerator UnloadSceneAndBackToMainMenu()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        player.transform.gameObject.SetActive(false);

        GameplayInfoUIControl.Instance.SetGameplayInfoUIActive(true);
        gameoverUI.SetActive(false);
        pauseMenuUI.SetActive(false);

        mainMenuUI.SetActive(true);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();
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
        mainMenuUI.SetActive(false);

        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        yield return StartCoroutine(LoadingScreen(1.0f));

        yield return StartCoroutine(LoadSceneAndSetActive(startingScene.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        GameplayInfoUIControl.Instance.SetGameplayInfoUIActive(true);
        player.transform.position = this.transform.position;
        player.SetActive(true);

        StartCoroutine(LoadingScreen(0.0f));
    }

    private IEnumerator LoadTutorialRoom()
    {
        mainMenuUI.SetActive(false);

        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        yield return StartCoroutine(LoadingScreen(1.0f));

        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.Scene02_TutorialScene.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        GameplayInfoUIControl.Instance.SetGameplayInfoUIActive(true);
        player.transform.position = this.transform.position;
        player.SetActive(true);

        if (unloadSceneZone == null)
            unloadSceneZone = GameObject.FindObjectOfType<UnloadSceneZone>();

        StartCoroutine(LoadingScreen(0.0f));
    }
}