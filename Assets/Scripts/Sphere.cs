using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float rotationAroundSpeed = 45f;
    public float transitionSpeed = 10f;
    public bool isPressedOnIt = false;
    public bool isMoved = false;
    public SceneController sceneController;
    private Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressedOnIt && !isMoved)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, transitionSpeed * Time.deltaTime);

            if(Mathf.Abs(transform.position.x) <= Mathf.Epsilon)
            {
                isMoved = true;
            }

            return;
        }
        transform.Rotate(Vector3.forward, rotationAroundSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked on sphere!");
        if (!isPressedOnIt)
        {
            isPressedOnIt = true;
            sceneController.MakeTransition();
        }
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }
}
