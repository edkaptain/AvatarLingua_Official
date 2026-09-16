using UnityEngine;

public class AIManager : MonoBehaviour
{
    public enum Voices
    {
        alloy,ash,ballad,coral,echo,fable,nova,onyx,sage,shimmer,verse,marin,cedar
    }
    public static AIManager Instance;
    [Header("API configuration")]
    public string api;
    [Header("Voice Configuration")]
    public Voices voiceSelected = Voices.coral;
    private OPENAI_API openAI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        openAI = new OPENAI_API(api, voiceSelected);
    }

    public void PlayClickSound()
    {
        SoundManager.Instance.ClickPlay();
    }

    public async void PronunceItemName(string text)
    {
        AudioClip audioClip =  await openAI.GenerateSpeech(text);

        if (audioClip == null)
        {
            Debug.LogError("The AudioClip could not be generated.");
            return;
        }

        SoundManager.Instance.PlaySound(audioClip);
    }
}
