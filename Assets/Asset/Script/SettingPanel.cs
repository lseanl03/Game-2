using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : PanelBase
{
    public bool isShowingSettingMenu;
    public Button settingButton;
    public Button giveUpButton;
    public GameObject settingMenu;
    public GameObject popupSettingObj;
    public void OnEnable()
    {
        isShowingSettingMenu = false;
        settingMenu.SetActive(isShowingSettingMenu);
    }
    public void ChangeSettingMenuState()
    {
        isShowingSettingMenu = !isShowingSettingMenu;
        settingMenu.SetActive(isShowingSettingMenu);

        if (isShowingSettingMenu)
        {
            if (sceneChanger.currentScene == SceneType.MainMenu) GiveUpButtonState(false);
            else GiveUpButtonState(true);
        }
    }
    public void GiveUpButtonState(bool state)
    {
        giveUpButton.gameObject.SetActive(state);
    }
    public void PopupSettingState(bool state)
    {
        popupSettingObj.SetActive(state);
    }
}
