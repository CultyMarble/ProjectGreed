using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuExtraManagerScript : MonoBehaviour
{
    /*
    [Header("Script References")]
    //private GameManagerScript gameManagerScript;
    //private MainMenuManagerScript mainMenuManagerScript;

    [Header("Main Menu Submenu Components")]
    public GameObject customizeSubmenu;
    public GameObject statisticSubmenu;
    public GameObject settingSubmenu;

    [Header("Main Menu Customize Components")]
    public TextMeshProUGUI colorText;

    // Ghost Trail Default Colors
    public GameObject ghostTrailDefaultPreview;

    // Ghost Trail Base Colors
    public GameObject ghostTrailRedPreview;
    public GameObject ghostTrailOrangePreview;
    public GameObject ghostTrailYellowPreview;
    public GameObject ghostTrailGreenPreview;
    public GameObject ghostTrailBluePreview;
    public GameObject ghostTrailPurplePreview;
    public GameObject ghostTrailPinkPreview;

    // Ghost Trail Shade Colors
    public GameObject ghostTrailWhitePreview;
    public GameObject ghostTrailGreyPreview;
    public GameObject ghostTrailBlackPreview;

    // Ghost Trail Special Colors
    public GameObject ghostTrailRainbowPreview;
    public GameObject ghostTrailKonamiPreview;

    [Header("Main Menu Statistics Components")]
    public TextMeshProUGUI playerTotalDeathJump;
    public TextMeshProUGUI playerTotalDeathDash;
    public TextMeshProUGUI playerTotalDeathText;
    public TextMeshProUGUI dashyEightScoreText;

    [Header("Main Menu Setting Components")]
    public Slider volumeSlider;
    public Toggle vignetteToggle;
    public Toggle motionBlurToggle;

    private void Awake()
    {
        //gameManagerScript = FindObjectOfType<GameManagerScript>();
        //mainMenuManagerScript = FindObjectOfType<MainMenuManagerScript>();
    }

    private void Start()
    {
        GhostTrailPreviewSetFalse();
        GhostTrailPreviewUpdate();
        //playerTotalDeathJump.text = gameManagerScript.playerTotalJump.ToString();
        //playerTotalDeathDash.text = gameManagerScript.playerTotalDash.ToString();
        //playerTotalDeathText.text = gameManagerScript.playerTotalDeath.ToString();
        //dashyEightScoreText.text = gameManagerScript.dashyEightBestScore.ToString();
        //volumeSlider.value = gameManagerScript.settingVolume;
        //vignetteToggle.isOn = gameManagerScript.settingVignette;
        //motionBlurToggle.isOn = gameManagerScript.settingMotionBlur;
    }

    private void OnEnable()
    {
        //MainMenuManagerScript.mainMenuExtraEndEvent += SubmenuClose;
    }

    private void OnDisable()
    {
        //MainMenuManagerScript.mainMenuExtraEndEvent -= SubmenuClose;
    }

    // Main Menu Submenu Button Managers
    public void CustomizeSubmenuOpen()
    {
        //mainMenuManagerScript.buttonClickAudio.Play();
        customizeSubmenu.SetActive(true);
    }

    public void StatisticSubmenuOpen()
    {
        //mainMenuManagerScript.buttonClickAudio.Play();
        statisticSubmenu.SetActive(true);
    }

    public void SettingSubmenuOpen()
    {
        //mainMenuManagerScript.buttonClickAudio.Play();
        settingSubmenu.SetActive(true);
    }

    public void SubmenuClose()
    {
        //mainMenuManagerScript.buttonClickAudio.Play();
        customizeSubmenu.SetActive(false);
        statisticSubmenu.SetActive(false);
        settingSubmenu.SetActive(false);
    }

    public void GhostTrailPreviewSetFalse()
    {
        // Ghost Trail Default Colors
        ghostTrailDefaultPreview.SetActive(false);

        // Ghost Trail Base Colors
        ghostTrailRedPreview.SetActive(false);
        ghostTrailOrangePreview.SetActive(false);
        ghostTrailYellowPreview.SetActive(false);
        ghostTrailGreenPreview.SetActive(false);
        ghostTrailBluePreview.SetActive(false);
        ghostTrailPurplePreview.SetActive(false);
        ghostTrailPinkPreview.SetActive(false);

        // Ghost Trail Shade Colors
        ghostTrailWhitePreview.SetActive(false);
        ghostTrailGreyPreview.SetActive(false);
        ghostTrailBlackPreview.SetActive(false);

        // Ghost Trail Special Colors
        ghostTrailRainbowPreview.SetActive(false);
        ghostTrailKonamiPreview.SetActive(false);
    }

    public void GhostTrailPreviewUpdate()
    {
        //colorText.text = gameManagerScript.ghostTrailCurrentColor;

        // Ghost Trail Default Colors
        if (colorText.text == ("Default"))
        {
            ghostTrailDefaultPreview.SetActive(true);
        }

        // Ghost Trail Base Colors
        if (colorText.text == ("Red"))
        {
            ghostTrailRedPreview.SetActive(true);
        }

        if (colorText.text == ("Orange"))
        {
            ghostTrailOrangePreview.SetActive(true);
        }

        if (colorText.text == ("Yellow"))
        {
            ghostTrailYellowPreview.SetActive(true);
        }

        if (colorText.text == ("Green"))
        {
            ghostTrailGreenPreview.SetActive(true);
        }

        if (colorText.text == ("Blue"))
        {
            ghostTrailBluePreview.SetActive(true);
        }

        if (colorText.text == ("Purple"))
        {
            ghostTrailPurplePreview.SetActive(true);
        }

        if (colorText.text == ("Pink"))
        {
            ghostTrailPinkPreview.SetActive(true);
        }

        // Ghost Trail Shade Colors
        if (colorText.text == ("White"))
        {
            ghostTrailWhitePreview.SetActive(true);
        }

        if (colorText.text == ("Grey"))
        {
            ghostTrailGreyPreview.SetActive(true);
        }

        if (colorText.text == ("Black"))
        {
            ghostTrailBlackPreview.SetActive(true);
        }

        // Ghost Trail Special Colors
        if (colorText.text == ("Rainbow"))
        {
            ghostTrailRainbowPreview.SetActive(true);
        }

        if (colorText.text == ("Konami"))
        {
            ghostTrailKonamiPreview.SetActive(true);
        }
    }

    // Main Menu Setting Toggle Managers
    public void AudioMixerSetVolume(float volume)
    {
        gameManagerScript.audioMixer.SetFloat("AudioVolume", Mathf.Log10(volume) * 20);
        gameManagerScript.settingVolume = volumeSlider.value;
        gameManagerScript.SaveGameData();
    }

    public void MotionBlurToggle()
    {
        if (motionBlurToggle.isOn)
        {
            gameManagerScript.settingMotionBlur = true;
            gameManagerScript.SaveGameData();
        }

        else
        {
            gameManagerScript.settingMotionBlur = false;
            gameManagerScript.SaveGameData();
        }
    }

    public void VignetteToggle()
    {
        if (vignetteToggle.isOn)
        {
            gameManagerScript.settingVignette = true;
            gameManagerScript.SaveGameData();
        }

        else
        {
            gameManagerScript.settingVignette = false;
            gameManagerScript.SaveGameData();
        }
    }

    // Main Menu Button Managers
    public void Website()
    {
        mainMenuManagerScript.buttonClickAudio.Play();
        Application.OpenURL("https://garygames.itch.io/");

        if (!gameManagerScript.ghostTrailRainbowUnlock)
        {
            gameManagerScript.ghostTrailRainbowUnlock = true;
            gameManagerScript.SaveGameData();
        }
    }

    public void QuitGame()
    {
        mainMenuManagerScript.buttonClickAudio.Play();
        gameManagerScript.SaveGameData();
        Application.Quit();
    }
    */
}
