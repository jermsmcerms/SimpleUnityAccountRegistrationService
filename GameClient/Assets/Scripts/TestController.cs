using Shared.Models;
using UnityEngine;

public class TestController : MonoBehaviour
{
    [ContextMenu("Test Get")]
    public async void TestGet() {
        var httpClient = new HttpClient(new JsonDeserializationOption());
        var result = await httpClient.Get<User>("https://jsonplaceholder.typicode.com/todos/1");
    }

    private void Start() {
        TestGet();
    }
}
