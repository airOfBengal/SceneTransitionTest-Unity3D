using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineItem : MonoBehaviour
{
    public Image backgroundImage;
    public Text sceneTransitionButtonText;
    public Color defaultBGColor;
    public Color activeBGColor;

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
}
