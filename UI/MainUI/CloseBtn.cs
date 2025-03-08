using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseBtn : MonoBehaviour
{
    private Button closeBtn;

    private void Awake()
    {
        closeBtn = GetComponent<Button>();  
    }

    private void Start()
    {
        closeBtn.onClick.AddListener(() => ClosePanel());
    }

    private void ClosePanel()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
