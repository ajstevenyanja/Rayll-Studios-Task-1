using UnityEngine;

// Script to play a list of tracks
// next audio clip is played on finish of current one

public class PlayClipFromList : MonoBehaviour
{
    public AudioClip[] audioClips;
    private AudioSource audioSource;
    private int currentClipIndex = 0;
    int clipsCount;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clipsCount = audioClips.Length;
        PlayNextClip();
    }

    void Update()
    {
        // Check if the current audio clip has finished playing
        if (!audioSource.isPlaying)
        {
            // Play the next clip in the array
            PlayNextClip();
        }
    }

    void PlayNextClip()
    {
        // Check if there are more clips in the array
        if (currentClipIndex < clipsCount)
        {
            // Set the current clip and play it
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();

            // Move to the next clip in the array
            currentClipIndex++;
        }
        else
        {
            // If all clips have been played, you can restart from the beginning or handle it as needed
            // For now, let's restart from the beginning
            currentClipIndex = 0;
        }
    }
}
