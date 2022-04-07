using UnityEngine;
using UnityEngine.UI;

public class TimelineItem : MonoBehaviour
{
    public Image backgroundImage;
    public Text sceneTransitionButtonText;
    public Color defaultBGColor;
    public Color activeBGColor;
    public SceneController sceneController;

    public void UpdateBackgroundColor(bool isActive)
    {
        if (isActive)
        {
            backgroundImage.color = activeBGColor;
        }
        else
        {
            backgroundImage.color = defaultBGColor;
        }
    }

    public void SetButtonText(string label)
    {
        sceneTransitionButtonText.text = label;
    }

    public string GetButtonText()
    {
        return sceneTransitionButtonText.text;
    }

    public void OnClickSceneTransitionButton()
    {
        if(sceneTransitionButtonText.text == SceneController.currentSceneIndexString)
        {
            return;
        }

        sceneController.LoadNextScene(sceneTransitionButtonText.text);
        StartCoroutine(sceneController.LoadSceneCoroutine());
    }
}
