using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine.UI;


public enum ApiModel
{
    da1,
    da2,
}

public class ChatGPTRequest : MonoBehaviour
{
    [SerializeField] private Loading loading;
    [SerializeField] private ApiModel models;
    [SerializeField] private Button submit;

    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text answer;
    [SerializeField] private string model;
    [SerializeField] private int maxTokens = 2048;
    [SerializeField] private float temperature = 0.7f;

    private void OnEnable()
    {
        submit.onClick.AddListener(Submit);
    }

    private void OnDisable()
    {
        submit.onClick.RemoveListener(Submit);
        StopCoroutine(Datasend());
    }

    private void Submit()
    {
        loading.On();
        if (models == ApiModel.da1)
        {
            model = "text-davinci-002";
        }

        if (models == ApiModel.da2)
        {
            model = "text-davinci-003";
        }
        StartCoroutine(Datasend());
    }

    IEnumerator Datasend()
    {
        string url = "https://api.openai.com/v1/completions";
        string apiKey = "your open ai key here";
        string prompt = text.text;

        answer.text = "";
        
        // Create the request object
        UnityWebRequest request = UnityWebRequest.Post(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes("{\"model\": \"" + model + "\", \"prompt\":\"" + prompt +
                                                "\", \"max_tokens\":" + maxTokens + ", \"temperature\":" + temperature +
                                                "}");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + apiKey);
        request.SetRequestHeader("Content-Type", "application/json");

        // Send the request
        yield return request.SendWebRequest();

        // Handle the response
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("API request failed: " + request.error);
            loading.Off();
        }
        else
        {
            // Debug.Log("API response: " + request.downloadHandler.text);
            // answer.text = request.downloadHandler.text;

            string responseText = request.downloadHandler.text; // Fetch the response
            int startIndex = responseText.IndexOf("text\":\"") + "text\":\"".Length;
            int endIndex = responseText.IndexOf("\",", startIndex);
            int length = endIndex - startIndex;
            string response = responseText.Substring(startIndex, length);
            Debug.Log(response);
            loading.Off();
            answer.text = response;
            
            
        }
    }
}