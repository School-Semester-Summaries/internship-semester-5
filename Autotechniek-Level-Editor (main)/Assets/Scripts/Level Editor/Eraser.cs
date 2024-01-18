using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eraser : MonoBehaviour
{
    private UIItemCollection _uiItemCollection;
    private void Start()
    {
        _uiItemCollection = GameObject.FindGameObjectWithTag("UIItemCollection").GetComponent<UIItemCollection>();
    }
    public void OnMouseDown()
    {
        _uiItemCollection.SelectEraser();
    }
}
