using UnityEngine;

public class Restart : MonoBehaviour
{
    public SceneController sceneController;

    public void OnClickRestart()
    {
        sceneController.LoadNextScene("1");
        StartCoroutine(sceneController.LoadSceneCoroutine());
    }
}
