using UnityEngine;

public class AudioLoudnessDetection : MonoBehaviour
{
    public int sampleWindow = 1024;
    private AudioClip microphoneClip;
    private static AudioLoudnessDetection instance;

    private void Start()
    {
        // Ensure there is only one instance of AudioLoudnessDetection in the scene
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MicrophoneToAudioClip();
        }
        else
        {
            Destroy(this);
        }

        MicrophoneToAudioClip();
    }

    public void MicrophoneToAudioClip()
    {
        string microphoneName = Microphone.devices[0];
        microphoneClip = Microphone.Start(microphoneName, true, 20, AudioSettings.outputSampleRate);
    }

    public float GetLoudnessFromMicrophone()
    {
        return GetLoudnessFromAudioClip(Microphone.GetPosition(Microphone.devices[0]), microphoneClip);
    }

    public float GetLoudnessFromAudioClip(int clipPosition, AudioClip clip)
    {
        int startPosition = clipPosition - sampleWindow;

        if (startPosition < 0)
        {
            return 0;
        }

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPosition);

        float sum = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            sum += waveData[i] * waveData[i];
        }
        float rmsValue = Mathf.Sqrt(sum / sampleWindow);

        // Normalize the RMS value to a range between 0 and 1
        float loudness = Mathf.Clamp01(rmsValue);

        return loudness;
    }
}
