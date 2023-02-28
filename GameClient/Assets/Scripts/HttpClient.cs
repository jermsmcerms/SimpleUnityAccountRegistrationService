using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class HttpClient {
    public static async Task<T> 
    Get<T>(string endpoint) {
        var getRequest = CreateRequest(endpoint);
        getRequest.SendWebRequest();

        while(!getRequest.isDone) {
            await Task.Delay(10);
        }

        var result = JsonConvert
            .DeserializeObject<T>(getRequest.downloadHandler.text);

        Debug.Log(result);

        return result;
    }

    public static async Task<AuthenticationResponse> 
    Register(string endpoint, object payload) {
        try {
            var postRequest = CreateRequest(
                endpoint,
                RequestType.POST,
                payload);
            postRequest.SendWebRequest();

            while (!postRequest.isDone) {
                await Task.Delay(10);
            }

            if (postRequest.result == UnityWebRequest.Result.Success) {
                return JsonConvert
                    .DeserializeObject<AuthenticationResponse>(postRequest.downloadHandler.text);
            }
            return default;
        }
        catch(Exception e) {
            Debug.LogError($"Error: {e.Message}");
            return default;
        }
    }

    private static UnityWebRequest 
    CreateRequest(string path, 
        RequestType type = RequestType.GET, 
        object data = null) 
    {
        var request = new UnityWebRequest(path, type.ToString());

        if(data != null) {
            string serializedObject = JsonConvert.SerializeObject(data);
            Debug.Log(serializedObject);
            var bodyRaw = 
                Encoding.UTF8.GetBytes(serializedObject);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        }

        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        return request;
    }
}

public enum RequestType {
    GET,
    POST,
    PUT,
    DELETE
}
