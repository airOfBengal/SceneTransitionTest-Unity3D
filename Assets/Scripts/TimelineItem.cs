using System;
using UnityEngine;
using UnityEngine.UI;

public class TimelineItem : MonoBehaviour
{
    public Image backgroundImage;
    public Text sceneTransitionButtonText;
    public Color defaultBGColor;
    public Color activeBGColor;

    public event Action<string> OnClickTimelineItemAction;

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
        OnClickTimelineItemAction?.Invoke(sceneTransitionButtonText.text);
    }
}
