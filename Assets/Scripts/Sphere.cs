using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public float rotationAroundSpeed = 45f;
    public float transitionSpeed = 10f;
    public volatile bool isPressedOnIt = false;
    public SceneController sceneController;
    private Animator animator;

    private void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPressedOnIt)
        {
            transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, transitionSpeed * Time.deltaTime);
            return;
        }
        transform.Rotate(Vector3.forward, rotationAroundSpeed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
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
