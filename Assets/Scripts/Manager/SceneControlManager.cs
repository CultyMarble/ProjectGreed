using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControlManager : SingletonMonobehaviour<SceneControlManager>
{
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Image loadingScreenImage;

    [SerializeField] private GameObject player;

    private readonly float loadingScreenDuration = 0.5f;

    private bool isLoadingScreenActive = default;
    public bool IsLoadingScreenActive => isLoadingScreenActive;

    private bool isLoadingScene = default;
    public bool IsLoadingScene => isLoadingScene;

    public SceneName CurrentActiveScene = default;
    public GameplayState CurrentGameplayState = default;

    //===========================================================================
    private void Start()
    {
        // Initial loading sequences
        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        loadingScreenCanvasGroup.alpha = 1.0f;

        OptionMenuGUI.Instance.SetContentActive(true);
        OptionMenuGUI.Instance.SetContentActive(false);

        MainMenuGUI.Instance.SetContentActive(true);
        CurrentActiveScene = SceneName.MainMenu;
        CurrentGameplayState = GameplayState.Pause;

        StartCoroutine(LoadingScreen(0.0f));
    }

    //===========================================================================
    private IEnumerator UnloadAndSwitchScene(string sceneName, Vector3 spawnPosition)
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        GameOverMenuGUI.Instance.SetContentActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventManager.CallAfterSceneLoadEvent();

        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.gameObject.SetActive(true);

        if (sceneName == SceneName.DemoSceneHub.ToString())
        {
            Player.Instance.GetComponent<PlayerHeart>().ResetPlayerHeart();
            Player.Instance.GetComponentInChildren<RangeAbility>().ResetAbilityCharge();
            Player.Instance.GetComponentInChildren<BombAbility>().ResetBombAmount();
        }

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameplayState = GameplayState.Ongoing;
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
    private IEnumerator LoadDemoSceneHub()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        MainMenuGUI.Instance.SetContentActive(false);
        SaveSelectMenuGUI.Instance.SetContentActive(false);
        DemoOverMenuGUI.Instance.SetContentActive(false);
        ObjectPoolingManager.Instance.SetAllObjectsActive(false);
        Player.Instance.GetComponent<PlayerCurrencies>().ResetCurrency();

        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.DemoSceneHub.ToString()));
        EventManager.CallAfterSceneLoadEvent();
        CurrentActiveScene = SceneName.DemoSceneHub;

        Player.Instance.transform.position = Vector3.zero;
        Player.Instance.gameObject.SetActive(true);

        SaveDataManager.Instance.LoadPlayerDataToRuntimeData(SaveDataSlot.save01);
        SaveDataManager.Instance.LoadUpgradeSaveData();

        StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameplayState = GameplayState.Ongoing;
        isLoadingScene = false;
    }

    private IEnumerator LoadMainMenu()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        PauseMenuGUI.Instance.SetContentActive(false);
        GameOverMenuGUI.Instance.SetContentActive(false);
        DemoOverMenuGUI.Instance.SetContentActive(false);
        ObjectPoolingManager.Instance.SetAllObjectsActive(false);
        Player.Instance.GetComponent<PlayerCurrencies>().ResetCurrency();


        DisplayItemUpgradeIcon.Instance.Clear();
        MapGenerator.Instance.ClearMap();

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        
        MainMenuGUI.Instance.SetContentActive(true);
        CurrentActiveScene = SceneName.MainMenu;

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameplayState = GameplayState.Ongoing;
        isLoadingScene = false;
    }

    private IEnumerator LoadDemoDungeon()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        MainMenuGUI.Instance.SetContentActive(false);
        SaveSelectMenuGUI.Instance.SetContentActive(false);
        ObjectPoolingManager.Instance.SetAllObjectsActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.DemoSceneDungeon.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        CurrentActiveScene = SceneName.DemoSceneDungeon;
        CurrentGameplayState = GameplayState.Pause;

        Player.Instance.transform.position = Vector3.zero;

        while (CurrentGameplayState != GameplayState.Ongoing)
            yield return null;

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        isLoadingScene = false;
    }

    private IEnumerator LoadTutorial()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        ObjectPoolingManager.Instance.SetAllObjectsActive(false);
        Player.Instance.GetComponent<PlayerCurrencies>().ResetCurrency();

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.Tutorial.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        CurrentActiveScene = SceneName.Tutorial;
        CurrentGameplayState = GameplayState.Pause;

        Player.Instance.transform.position = Vector3.zero;

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();

        CurrentGameplayState = GameplayState.Ongoing;
        isLoadingScene = false;
    }

    //===========================================================================
    //===== LIST OF SCENE LOAD WRAPPER ==========================================
    //===========================================================================
    public void LoadScene(string sceneName, Vector3 spawnPosition)
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            CurrentGameplayState = GameplayState.Pause;
            StartCoroutine(UnloadAndSwitchScene(sceneName, spawnPosition));
        }
    }

    public void LoadMainMenuWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            CurrentGameplayState = GameplayState.Pause;

            StartCoroutine(LoadMainMenu());
        }
    }

    public void LoadDemoSceneHubWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            CurrentGameplayState = GameplayState.Pause;

            StartCoroutine(LoadDemoSceneHub());
        }
    }

    public void LoadDemoDungeonWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            CurrentGameplayState = GameplayState.Pause;

            StartCoroutine(LoadDemoDungeon());
        }
    }

    public void LoadTutorialWrapper()
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            CurrentGameplayState = GameplayState.Pause;

            StartCoroutine(LoadTutorial());
        }
    }

    public void RespawnPlayerAtHub()
    {
        if (isLoadingScreenActive == false)
        {
            isLoadingScene = true;
            CurrentGameplayState = GameplayState.Pause;

            MainMenuGUI.Instance.SetContentActive(false);
            PauseMenuGUI.Instance.SetContentActive(false);
            GameOverMenuGUI.Instance.SetContentActive(false);
            DemoOverMenuGUI.Instance.SetContentActive(false);

            DisplayItemUpgradeIcon.Instance.Clear();
            MapGenerator.Instance.ClearMap();

            StartCoroutine(UnloadAndSwitchScene(SceneName.DemoSceneHub.ToString(), Vector3.zero));
        }
    }
}