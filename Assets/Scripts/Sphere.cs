using System.Collections;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float rotationAroundSpeed = 45f;
    public float transitionSpeed = 10f;
    public bool isPressedOnIt = false;
    public bool isMoved = false;
    public SceneController sceneController;
    private Animator animator;
    public bool shouldRotate = true;
    public float delayToDestroy = 0.8f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressedOnIt && !isMoved)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, transitionSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, Vector3.zero) <= Mathf.Epsilon)
            {
                isMoved = true;
            }

            return;
        }
        if (shouldRotate)
        {
            transform.Rotate(Vector3.forward, rotationAroundSpeed * Time.deltaTime);
        }        
    }

    private void OnMouseDown()
    {
        //Debug.Log("clicked on sphere!");
        if (!isPressedOnIt)
        {
            shouldRotate = false;
            isPressedOnIt = true;

            transform.parent = null;
            if(transform.childCount > 0)
            {
                GameObject child = transform.GetChild(0).gameObject;
                child.transform.parent = null;
            }

            sceneController.MakeTransition();
            StartCoroutine(sceneController.LoadSceneCoroutine());
        }
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
        StartCoroutine(DestroyerCoroutine());
    }

    IEnumerator DestroyerCoroutine()
    {
        yield return new WaitForSeconds(delayToDestroy);
        Destroy(gameObject);
    }
}
