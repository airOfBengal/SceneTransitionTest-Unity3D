using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private int noOfScenes;
    private static Transform timelineItemsParent;
    public GameObject timelineItemPrefab;
    public GameObject timelineItemsLinkPrefab;
    public static string currentSceneIndexString = "1";
    public static string prevSceneIndexString = "1";
    public GameObject cameraPrefab;
    public GameObject directionalLightPrefab;
    public GameObject eventSystemPrefab;
    public GameObject timelineLinkImagePrefab;
    private static GameObject canvas;
    public float sceneTransitionDelay = 0.5f;

    private static GameObject canvasFadeInGO;
    private static GameObject canvasFadeOutGO;

    public GameObject[] sphereGameObjects;
    private static Sphere[] spheres;

    private void Awake()
    {
        //Debug.Log("awake called in scene " + currentSceneIndexString);
        
        if (instance != null)
        {
            StartCoroutine(UpdateTimelineItemBackground(currentSceneIndexString));
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
    IEnumerator Start()
    {
        canvas = GameObject.Find("Canvas");
        // Draw timeline
        PopulateTimelineItems();
        yield return StartCoroutine(UpdateTimelineItemBackground(currentSceneIndexString));
        DrawTimelineLink();

        //Debug.Log("start called in scene" + currentSceneIndexString);
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

        
        canvasFadeInGO = canvas.transform.Find("FadeInImage").gameObject;
        canvasFadeOutGO = canvas.transform.Find("FadeOutImage").gameObject;

        if (currentSceneIndexString != prevSceneIndexString)
        {
            canvasFadeOutGO.SetActive(true);
            yield return new WaitForSeconds(1f);
            canvasFadeOutGO.SetActive(false);
        }

        if (currentSceneIndexString == "2")
        {            
            spheres = new Sphere[sphereGameObjects.Length];
            for(int i = 0; i < spheres.Length; i++)
            {
                spheres[i] = sphereGameObjects[i].GetComponent<Sphere>();
                //Debug.Log("sphere " + i + " is null: " + spheres[i] == null);
            }
            //Debug.Log("spheres len: " + spheres.Length);
        }
    }

    public void LoadNextScene(string sceneIndexString)
    {
        prevSceneIndexString = currentSceneIndexString;
        currentSceneIndexString = sceneIndexString;
        Debug.Log("prev scene: " + prevSceneIndexString + " curr scene: " + currentSceneIndexString);
        if (prevSceneIndexString == "2" && currentSceneIndexString == "3")
        {
            Debug.Log("spheres len: " + spheres.Length);
            foreach (Sphere sphere in spheres)
            {
                Debug.Log("pressed on sphere: " + sphere.isPressedOnIt);
                if (!sphere.isPressedOnIt)
                {
                    sphere.shouldRotate = false;
                    sphere.FadeOut();
                }
            }
        }

        if (!(prevSceneIndexString == "2" && currentSceneIndexString == "3"))
        {
            canvasFadeInGO.SetActive(true);
        }
    }

    public IEnumerator LoadSceneCoroutine()
    {
        AsyncOperation asyncOperation = null;
        if (prevSceneIndexString == "2" && currentSceneIndexString == "3")
        {
            asyncOperation = SceneManager.LoadSceneAsync("Scene" + currentSceneIndexString, LoadSceneMode.Additive);
        }
        else        
        {            
            asyncOperation = SceneManager.LoadSceneAsync("Scene" + currentSceneIndexString);
        }

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        canvasFadeInGO.SetActive(false);
    }

    public void PopulateTimelineItems()
    {
        GameObject timeline = Instantiate(timelineItemsLinkPrefab);        
        timeline.transform.SetParent(canvas.transform, false);
        timelineItemsParent = timeline.transform;
        noOfScenes = SceneManager.sceneCountInBuildSettings;
        for (int i=1;i<= noOfScenes; i++)
        {
            GameObject go = Instantiate(timelineItemPrefab);
            go.transform.SetParent(timelineItemsParent, false);
            TimelineItem timelineItem = go.GetComponent<TimelineItem>();
            timelineItem.sceneTransitionButtonText.text = i.ToString();
        }
    }

    IEnumerator UpdateTimelineItemBackground(string sceneIndexString)
    {
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
    }

    private void DrawTimelineLink()
    {
        GameObject timelineLinkImageGO = Instantiate(timelineLinkImagePrefab);
        timelineLinkImageGO.transform.SetParent(canvas.transform, false);
        float startX = 0, endX = 0;

        int i = 1;
        foreach (Transform t in timelineItemsParent)
        {
            if (i == 1)
            {
                startX = t.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
            }
            else if (i == noOfScenes)
            {
                endX = t.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
            }
            i++;
        }
        //Debug.Log("end transform pos: " + endX + " start transform pos: " + startX);
        timelineLinkImageGO.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(endX - startX), 5f);
    }
}
