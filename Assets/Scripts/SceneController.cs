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
    public GameObject timelineLinkImagePrefab;
    private GameObject canvas;
    public float sceneTransitionDelay = 0.5f;

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
    }

    public void MakeTransition()
    {
        LoadNextScene((int.Parse(currentSceneIndexString)+1).ToString());
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
        StartCoroutine(UpdateTimelineItemBackground(currentSceneIndexString));
    }

    public void LoadNextScene(string sceneIndexString)
    {
        if (currentSceneIndexString == "2" && sceneIndexString == "3")
        {
            SceneManager.LoadScene("Scene" + sceneIndexString, LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadSceneAsync("Scene" + sceneIndexString);
        }
        currentSceneIndexString = sceneIndexString;
    }

    IEnumerator LoadSceneAsync(string sceneIndexString)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene" + sceneIndexString, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        StartCoroutine(UpdateTimelineItemBackground(sceneIndexString));
    }

    public void PopulateTimelineItems()
    {
        GameObject timeline = Instantiate(timelineItemsLinkPrefab);
        canvas = GameObject.Find("Canvas");
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

    IEnumerator UpdateTimelineItemBackground(string sceneIndexString)
    {

        Debug.Log("current scene index: " + sceneIndexString);

        GameObject timelineLinkImageGO = Instantiate(timelineLinkImagePrefab);
        timelineLinkImageGO.transform.SetParent(canvas.transform, false);
        Transform startTransform = null, endTransform = null;

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

        yield return new WaitForEndOfFrame();

        int i = 1;
        foreach(Transform t in timelineItemsParent)
        {
            if (i == 1)
            {
                startTransform = t;
            }
            else if(i == noOfScenes)
            {
                endTransform = t;
            }
            i++;
        }

        timelineLinkImageGO.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(endTransform.position.x - startTransform.position.x), 5f);
    }
}
