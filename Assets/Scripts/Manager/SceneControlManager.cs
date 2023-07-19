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
    Scene13_Room8,
    Scene14_Room9,
    Scene15_Room10,
}

public class SceneControlManager : SingletonMonobehaviour<SceneControlManager>
{
    [SerializeField] private CanvasGroup loadingScreenCanvasGroup;
    [SerializeField] private Image loadingScreenImage;

    [SerializeField] private GameObject player;

    [Header("Starting Scene:")]
    [SerializeField] private SceneName startingScene;
    [SerializeField] private Transform startingPosition;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private Button mm_startGameButton;
    [SerializeField] private Button mm_tutorialButton;
    [SerializeField] private Button mm_exitButton;

    [Header("Pause Menu")]
    [SerializeField] private PlayerCurrencies playerCurrencies;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] private Animator pm_animator;
    [SerializeField] private Button pm_loadMainMenuButton;

    [Header("Option Menu")]
    [SerializeField] public GameObject optionMenu;

    [Header("Gameover Menu")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Button gv_loadLastCheckPointButton;
    [SerializeField] private Button gv_mainMenuButton;

    [Header("Gameplay Runtime Data")]
    [SerializeField] private SOListInt generatedItemForSale;

    private readonly float loadingScreenDuration = 0.75f;
    private bool isLoadingScreenActive;
    private bool canUnload = false;
    private UnloadSceneZone unloadSceneZone;

    //===========================================================================
    private void OnEnable()
    {
        // Main Menu
        mm_startGameButton.onClick.AddListener(() => StartCoroutine(LoadStartingScene()));
        mm_tutorialButton.onClick.AddListener(() => StartCoroutine(LoadTutorialRoom()));
        mm_exitButton.onClick.AddListener(() => Application.Quit());

        // Pause Menu
        pm_loadMainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1.0f;
            pm_animator.SetTrigger("Close");
            StartCoroutine(UnloadSceneAndBackToMainMenu());
        });

        // Gameover Menu
        gv_loadLastCheckPointButton.onClick.AddListener(() => StartCoroutine(LoadLastCheckPoint()));
        gv_mainMenuButton.onClick.AddListener(() => StartCoroutine(UnloadSceneAndBackToMainMenu()));
    }

    private void Start()
    {
        // First Time Load
        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        loadingScreenCanvasGroup.alpha = 1.0f;

        mainMenu.SetActive(true);

        StartCoroutine(LoadingScreen(0.0f));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && mainMenu.activeSelf == false || Input.GetKeyDown(KeyCode.P) && player.activeInHierarchy != false)
        {
            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 1;
                pm_animator.SetTrigger("Close");
                playerCurrencies.Close();
            }
            else
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                playerCurrencies.Open();
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

    private void OnDisable()
    {
        generatedItemForSale.itemList.Clear();
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
        gameOverMenu.SetActive(false);

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        EventManager.CallAfterSceneLoadEvent();

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();
    }

    private IEnumerator UnloadSceneAndBackToMainMenu()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        player.transform.gameObject.SetActive(false);
        player.GetComponentInChildren<BasicAbility>().ResetDamage();
        player.GetComponentInChildren<BasicAbility>().ResetMaxFuel();
        player.GetComponent<PlayerHealth>().ResetPlayerHealth();
        player.GetComponent<PlayerController>().dashCD = 3.0f;

        // GameplayRuntimeData
        generatedItemForSale.itemList.Clear();

        GameplayInfoUIControl.Instance.SetGameplayInfoUIActive(false);

        pm_animator.SetTrigger("Close");
        gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);

        SaveDataManager.Instance.SAVE01.SaveCheckPointData(SceneName.Scene03_HubArea, Vector3.zero);

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
    private void SetPlayerActiveTrue()
    {
        player.SetActive(true);

        player.transform.position = this.transform.position;

        player.GetComponent<PlayerHealth>().UpdateCurrentHealth();
    }

    //===========================================================================
    private IEnumerator LoadStartingScene()
    {
        mainMenu.SetActive(false);

        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        yield return StartCoroutine(LoadingScreen(1.0f));

        yield return StartCoroutine(LoadSceneAndSetActive(startingScene.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        GameplayInfoUIControl.Instance.SetGameplayInfoUIActive(true);

        SetPlayerActiveTrue();
        player.transform.position = startingPosition.transform.position;

        StartCoroutine(LoadingScreen(0.0f));
    }

    private IEnumerator LoadTutorialRoom()
    {
        mainMenu.SetActive(false);

        loadingScreenImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        yield return StartCoroutine(LoadingScreen(1.0f));

        yield return StartCoroutine(LoadSceneAndSetActive(SceneName.Scene02_TutorialScene.ToString()));
        EventManager.CallAfterSceneLoadEvent();

        GameplayInfoUIControl.Instance.SetGameplayInfoUIActive(true);
        SetPlayerActiveTrue();

        if (unloadSceneZone == null)
            unloadSceneZone = GameObject.FindObjectOfType<UnloadSceneZone>();

        StartCoroutine(LoadingScreen(0.0f));
    }

    //===========================================================================
    public IEnumerator LoadLastCheckPoint()
    {
        EventManager.CallBeforeSceneUnloadLoadingScreenEvent();
        yield return StartCoroutine(LoadingScreen(1.0f));

        gameOverMenu.SetActive(false);
        player.transform.position = SaveDataManager.Instance.SAVE01.RetrieveCheckPointSpawnLocation();

        EventManager.CallBeforeSceneUnloadEvent();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        yield return StartCoroutine(LoadSceneAndSetActive(SaveDataManager.Instance.SAVE01.RetrieveCheckPointSceneData().ToString()));
        EventManager.CallAfterSceneLoadEvent();

        GameplayInfoUIControl.Instance.SetGameplayInfoUIActive(true);

        yield return StartCoroutine(LoadingScreen(0.0f));
        EventManager.CallAfterSceneLoadedLoadingScreenEvent();
    }

    //===========================================================================
    public void OpenOptionMenuButton()
    {
        optionMenu.SetActive(true);
    }

    public void ExitGameButton()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

}