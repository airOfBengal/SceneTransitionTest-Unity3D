using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private int noOfScenes;
    private static Transform timelineItemsParent;
    public GameObject timelineItemPrefab;
    public GameObject timelineItemsLinkPrefab;
    private static string currentSceneIndexString = "1";
    public GameObject cameraPrefab;
    public GameObject directionalLightPrefab;
    public GameObject eventSystemPrefab;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        //DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject camera = GameObject.Find("Main Camera");
        if(camera == null)
        {
            Instantiate(cameraPrefab);
        }
        GameObject directionalLight = GameObject.Find("Directional Light");
        if(directionalLight == null)
        {
            Instantiate(directionalLightPrefab);
        }
        GameObject eventSystem = GameObject.Find("EventSystem");
        if(eventSystem == null)
        {
            Instantiate(eventSystemPrefab);
        }
        PopulateTimelineItems();
        UpdateTimelineItemBackground(currentSceneIndexString);
    }

    public void LoadNextScene(string sceneIndexString)
    {
        if (currentSceneIndexString == "2" && sceneIndexString == "3")
        {
            SceneManager.LoadSceneAsync("Scene" + sceneIndexString, LoadSceneMode.Additive);
            UpdateTimelineItemBackground(sceneIndexString);
        }
        else
        {
            SceneManager.LoadSceneAsync("Scene" + sceneIndexString);
            //PopulateTimelineItems();
        }
        currentSceneIndexString = sceneIndexString;        
        //UpdateTimelineItemBackground(currentSceneIndexString);
    }

    public void PopulateTimelineItems()
    {
        GameObject timeline = Instantiate(timelineItemsLinkPrefab);
        GameObject canvas = GameObject.Find("Canvas");
        timeline.transform.SetParent(canvas.transform, false);
        timelineItemsParent = timeline.transform;
        noOfScenes = SceneManager.sceneCountInBuildSettings;
        for (int i=1;i<= noOfScenes; i++)
        {
            GameObject go = Instantiate(timelineItemPrefab);
            //UnityEditor.Selection.activeObject = go;
            go.transform.SetParent(timelineItemsParent, false);
            TimelineItem timelineItem = go.GetComponent<TimelineItem>();
            timelineItem.sceneTransitionButtonText.text = i.ToString();
        }
    }

    private void UpdateTimelineItemBackground(string sceneIndexString)
    {

        Debug.Log("current scene index: " + sceneIndexString);
        foreach (Transform t in timelineItemsParent)
        {
            TimelineItem timelineItem = t.gameObject.GetComponent<TimelineItem>();
            if (timelineItem.sceneTransitionButtonText.text == sceneIndexString)
            {
                timelineItem.UpdateBackgroundColor(true);
            }
            else
            {
                timelineItem.UpdateBackgroundColor(false);
            }
        }
    }
}
