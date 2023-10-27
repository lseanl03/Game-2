using UnityEngine;

public class ShowFPS : MonoBehaviour
{
    public float deltaTime = 0.0f;

    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    private void OnGUI()
    {
        int fps = Mathf.RoundToInt(1.0f / deltaTime);
        string text = "FPS: " + fps;

        GUI.Label(new Rect(20, 20, 200, 50), text);
    }
}
