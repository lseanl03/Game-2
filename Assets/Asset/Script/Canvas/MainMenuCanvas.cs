using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCanvas : CanvasBase
{
    public void Play()
    {
        sceneChanger.SceneChange(SceneType.SelectCard);
    }
    public void Quit()
    {
        sceneChanger.QuitGame();
    }
}
