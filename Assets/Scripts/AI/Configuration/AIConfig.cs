using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "AI/API Configuration")]
public class AIConfig : ScriptableObject
{
    [Header("Authentication")]
    public string apiKey;

    [Header("Models")]
    public string chatModel = "gpt-4o-mini";
    public string speechModel = "gpt-4o-mini-tts";
    public string transcriptionModel = "gpt-4o-mini-transcribe";
}
