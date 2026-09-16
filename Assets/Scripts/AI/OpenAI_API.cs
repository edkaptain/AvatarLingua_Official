using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class OPENAI_API
{
    private readonly string apiKey;
    private readonly AIManager.Voices voice;

    // Creates a new OpenAI connection
    public OPENAI_API(string api, AIManager.Voices voice = AIManager.Voices.coral)
    {
        apiKey = api;
        this.voice = voice;
    }

    public async Task<AudioClip> GenerateSpeech(string speech)
    {
        string audioPath = GetAudioPath(speech);

        if (File.Exists(audioPath))
        {
            Debug.Log($"Using existing audio: {audioPath}");
            return await LoadAudioClip(audioPath);
        }

        if (String.IsNullOrWhiteSpace(apiKey))
        {
            Debug.LogError("API key is empty");
            return null;
        }

        if (string.IsNullOrWhiteSpace(speech))
        {
            Debug.LogError("Speech text is empty");
            return null;
        }

        // Creates an HTTP client request to inicialize a server conection
        using HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        string json = JsonUtility.ToJson(new SpeechRequest
        {
            model = "gpt-4o-mini-tts",
            input = speech,
            voice = voice.ToString(),
            instructions = "Speak in a cheerful and positive tone."
        });

        using StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

        string endpoint = "https://api.openai.com/v1/audio/speech";

        // CURL
        HttpResponseMessage response = await client.PostAsync(endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();

            Debug.LogError($"OpenAI error: {response.StatusCode}\n{error}");

            return null;
        }

        byte[] audioBytes = await response.Content.ReadAsByteArrayAsync();

        // Supposed that is only used with the title object
        await SaveAudio(audioBytes, audioPath);

        return await LoadAudioClip(audioPath);
    }

    private async Task SaveAudio(byte[] audioBytes, string audioPath)
    {
        if (File.Exists(audioPath))
        {
            Debug.Log($"Audio already exists: {audioPath}");
            return;
        }

        await File.WriteAllBytesAsync(audioPath, audioBytes);

        Debug.Log($"Audio saved at: {audioPath}");
    }

    private string CreateFileName(string speech)
    {
        string[] words = speech.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        string fileName;

        if (words.Length < 10)
        {
            fileName = string.Join("_", words);
        }
        else
        {
            fileName = $"speech_{DateTime.Now:yyyy:MM:dd_HHmmss}";

            foreach (char invalidCharacter in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(invalidCharacter, '_');

            }


        }

        return fileName + ".mp3";
    }

    private async Task<AudioClip> LoadAudioClip(string audioPath)
    {
        string audioUrl = new Uri(audioPath).AbsoluteUri;

        using UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.MPEG);

        UnityWebRequestAsyncOperation operation = request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Audio loading error: {request.error}");
            return null;
        }

        return DownloadHandlerAudioClip.GetContent(request);
    }

    private string GetAudioPath(string speech)
    {
        string audioFolder = Path.Combine(Application.persistentDataPath, "Audio");

        Directory.CreateDirectory(audioFolder);

        string fileName = CreateFileName(speech);

        return Path.Combine(audioFolder, fileName);
    }

    [Serializable]
    private class SpeechRequest
    {
        public string model;
        public string input;
        public string voice;
        public string instructions;
    }


}