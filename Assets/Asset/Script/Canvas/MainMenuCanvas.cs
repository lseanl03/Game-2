using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvas : CanvasBase
{
    public void Play()
    {
        sceneChanger.OpenSelectCardScene();
    }
    public void Tutorial()
    {
        sceneChanger.OpenTutorialScene();
    }
    public void Quit()
    {
        sceneChanger.QuitGame();
    }
    public void _OnButtonClick()
    {
        uiManager._OnButtonClick();
    }
}
