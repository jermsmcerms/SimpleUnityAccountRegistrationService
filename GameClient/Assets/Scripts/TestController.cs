using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestController : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_Text resultText;

    private string url = "https://localhost:7297/";

    public void OnExit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void OnRegsiter() {
        SceneManager.LoadScene("RegisterScene");
    }

    public void OnReturn() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void OnRegisterSubmit() {
        StartCoroutine(RegisterAsync());
    }

    public IEnumerator RegisterAsync() {
        AuthenticationRequest request = new() {
            UserName = username.text,
            Password = password.text,
        };

        var task = HttpClient.Register(url + "Authentication/register", request);
        yield return new WaitUntil(() => task.IsCompleted);
        
        if(task != null && task.Result != null && !string.IsNullOrEmpty(task.Result.Token)) {
            resultText.text = "Registration Successful!";
        }
        else {
            resultText.text = "Registration failed";
        }
    }
}
