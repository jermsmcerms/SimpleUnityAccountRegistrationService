using Newtonsoft.Json;
using System;
using UnityEngine;

public class JsonDeserializationOption : ISerializationOption
{
    public string ContentType => "application/json";

    public T Deserialize<T>(string text) {
        try {
            var result = JsonConvert.DeserializeObject<T>(text);
            Debug.Log($"Success: {text}");
            return result;
        } catch (Exception e) {
            Debug.LogError($"Could not parse response. {e.Message}");
            return default;
        }
    }
}
