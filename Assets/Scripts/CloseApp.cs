using System;
using UnityEngine;
using UnityEngine.UI;

public class CloseApp : MonoBehaviour
{
    [SerializeField] private Button closeBtt;

    private void OnEnable()
    {
        closeBtt.onClick.AddListener(CloseAppNow);
    }

    private void OnDisable()
    {
        closeBtt.onClick.RemoveListener(CloseAppNow);
    }

    private void CloseAppNow()
    {
        Application.Quit();
        Debug.Log("App Closed");
    }
}