using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public SelectCardCanvas selectCardCanvas;
    public BattleCanvas battleCanvas;
    public LoadingSceneCanvas loadingSceneCanvas;
    public TutorialCanvas tutorialCanvas;
    public CanvasGroup fader;

    [Header("Switch Toggle")]
    public BGMSwitchToggle bGMSwitchToggle;
    public SFXSwitchToggle sFXSwitchToggle;
    protected PlayerManager deckManager => PlayerManager.instance;
    protected CollectionManager collectionManager => CollectionManager.instance;
    protected NotificationManager notificationManager => NotificationManager.instance;
    protected TooltipManager tooltipManager => TooltipManager.instance;
    protected GamePlayManager gamePlayManager => GamePlayManager.instance;
    protected AudioManager audioManager => AudioManager.instance;
    protected SceneChanger sceneChanger => SceneChanger.instance;
    public Action OnDead;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        Application.targetFrameRate = 60;
    }
    public void Start()
    {
        audioManager.PlayTheme();
        GetBGMState();
        GetSFXState();
    }
    public void Return()
    {
        sceneChanger.OpenMainMenuScene();
    }
    public void StartGame(int index)
    {
        loadingSceneCanvas.LoadScene(index);
    }
    public IEnumerator Fade(bool isOn)
    {
        float timer = isOn ? 1f : 0f;
        float duration = 0.9f;
        fader.blocksRaycasts = isOn;
        fader.DOFade(timer, duration);
        yield return new WaitForSeconds(duration);
    }
    public void HideTooltip()
    {
        tooltipManager.tooltipCanvas.CanvasState(false);
        if(tooltipManager.tooltipCanvas.characterCardTooltip != null)
        {
            CharacterCardTooltip characterCardTooltip = tooltipManager.tooltipCanvas.characterCardTooltip;
            characterCardTooltip.HideSkillDes();
            characterCardTooltip.HideStatusDes();
            characterCardTooltip.HideBreakingStatusDes();
        }
    }
    public void HideSettingMenu()
    {
        if(battleCanvas.settingPanel.isShowingSettingMenu)
            battleCanvas.settingPanel.ChangeSettingMenuState();
    }
    public void SetBGMState(bool state)
    {
        audioManager.ToggleBGMState(state);
    }
    public void SetSFXState(bool state)
    {
        audioManager.ToggleSFXState(state);
    }
    public void _OnButtonClick()
    {
        audioManager.PlayOnClickButton();
    }
    public void _OnClickSkill()
    {
        audioManager.PlayOnClickSkill();
    }
    public void _OnClick()
    {
        audioManager.PlayOnClick();
    }
    public void GetBGMState()
    {
        if (PlayerPrefs.HasKey("BGMState"))
        {
            bool bgmIsOn = PlayerPrefs.GetInt("BGMState") == 1;
            SetBGMState(bgmIsOn);
        }
    }
    public void GetSFXState()
    {
        if (PlayerPrefs.HasKey("SFXState"))
        {
            bool sfxIsOn = PlayerPrefs.GetInt("SFXState") == 1;
            SetSFXState(sfxIsOn);
        }
    }
}
