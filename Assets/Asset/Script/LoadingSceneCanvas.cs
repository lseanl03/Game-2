using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneCanvas : MonoBehaviour
{
    public float waitTime = 3f;
    public CanvasGroup loadingScenePanel;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;
    public PlayerManager playerManager => PlayerManager.instance;
    protected SceneChanger sceneChanger => SceneChanger.instance;
    public void LoadScene(int sceneIndex)
    {
        if (!playerManager.deckSaved) return;
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        LoadScenePanelState(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;
        float progressValue = 0;

        while (!asyncOperation.isDone)
        {
            progressValue = Mathf.MoveTowards(progressValue, asyncOperation.progress, Time.deltaTime/waitTime);
            loadingSlider.value = progressValue;

            loadingText.text = "Loading..." + Mathf.RoundToInt(progressValue * 100) + "%";
            if (progressValue >= 0.9f)
            {
                yield return new WaitForSeconds(waitTime);
                loadingSlider.value = 1;
                loadingText.text = "Loading..." + 100 + "%";
                sceneChanger.OpenGamePlayScene();
                yield return new WaitForSeconds(waitTime);
                LoadScenePanelState(false);
                asyncOperation.allowSceneActivation = true;

            }
            yield return null;
        }
    }
    public void LoadScenePanelState(bool state)
    {
        loadingScenePanel.gameObject.SetActive(state);
    }
}
