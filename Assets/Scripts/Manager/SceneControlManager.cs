using System;
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

    [Header("Pause Menu")]
    [SerializeField] private PlayerCurrencies playerCurrencies;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] private Animator pm_animator;
    [SerializeField] private Button pm_loadMainMenuButton;

    [Header("Option Menu")]
    [SerializeField] public GameObject optionMenu;

    [Header("Gameover Menu")]
    //[SerializeField] private GameObject gameOverMenu;
    //[SerializeField] private Button gv_loadLastCheckPointButton;
    //[SerializeField] private Button gv_mainMenuButton;

    private readonly float loadingScreenDuration = 1.0f;
    private bool isLoadingScreenActive;
    private bool canUnload = false;
    private UnloadSceneZone unloadSceneZone;

    //===========================================================================
    private void OnEnable()
    {
        // Pause Menu
        pm_loadMainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            pm_animator.SetTrigger("Close");
            LoadScene(SceneName.MainMenu, Vector3.zero);
        });

        // Gameover Menu
        //gv_loadLastCheckPointButton.onClick.AddListener(() => StartCoroutine(LoadLastCheckPoint()));
        //gv_mainMenuButton.onClick.AddListener(() => StartCoroutine(UnloadSceneAndBackToMainMenu()));
    }

    private IEnumerator Start()
    {
        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.MainMenu.ToString()));

        StartCoroutine(LoadingScreen(0.0f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && player.activeInHierarchy != false)
        {
            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
                pm_animator.SetTrigger("Close");
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
        }

        if (unloadSceneZone == null)
            return;

        this.canUnload = unloadSceneZone.canUnload;
    }

    //===========================================================================
    private IEnumerator UnloadAndSwitchScene(string sceneName, Vector3 spawnPosition)
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        Player.Instance.transform.position = spawnPosition;

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventManager.CallAfterSceneLoadEvent();

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
    public void LoadScene(SceneName sceneName, Vector3 spawnPosition)
    {
        if (isLoadingScreenActive == false)
        {
            StartCoroutine(UnloadAndSwitchScene(sceneName.ToString(), spawnPosition));
        }
    }

    public IEnumerator LoadLastCheckPoint()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        // gameOverMenu.SetActive(false);
        Player.Instance.transform.position = SaveDataManager.Instance.SAVE01.RetrieveCheckPointSpawnLocation();

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(SaveDataManager.Instance.SAVE01.RetrieveCheckPointSceneData().ToString()));
        EventManager.CallAfterSceneLoadEvent();

        GameplayInfoController.Instance.SetGameplayInfoUIActive(true);

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();
    }

    public void OpenOptionMenuButton()
    {
        optionMenu.SetActive(true);
    }
}