using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Init : MonoBehaviour
{
    private float startTime = 0f;

    [ContextMenu("MyFunc")]
    void Subscribe()
    {
        EditorApplication.update += MyEditorUpdate;
        startTime = Time.realtimeSinceStartup;
    }

    public void MyEditorUpdate()
    {
        float currTime = Time.realtimeSinceStartup;

        Debug.Log((currTime - startTime) % 1);

        Animator animator = gameObject.GetComponent<Animator>();
        animator.Update(Time.deltaTime);
    }
}