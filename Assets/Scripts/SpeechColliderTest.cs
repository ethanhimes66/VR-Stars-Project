using System.IO;
using HuggingFace.API;
using TMPro;
using UnityEngine;

public class SpeechColliderTest : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI textResponse;

    private AudioClip clip;
    private byte[] bytes;
    private bool recording;
    private float startTime;

    // private const float silenceThreshold = 0.01f; // Adjust based on the noise level in your environment
    // private float silenceStartTime; // Tracks when silence starts
    // private const float maxSilenceDuration = 5.0f; // 5 seconds of silence before stopping

    private void Start() {
        recording = false;
        // silenceStartTime = 0f;
    }

    private void Update() {
        // if (recording) {
        //     // Check if the microphone position is valid and has data to analyze
        //     int micPosition = Microphone.GetPosition(null);
        //     if (micPosition > 0 && micPosition <= clip.samples) {
        //         // Only check for silence if we have data
        //         if (IsSilent()) {
        //             // If silence lasts longer than maxSilenceDuration, stop recording
        //             if (Time.time >= silenceStartTime + maxSilenceDuration) {
        //                 StopRecording();
        //             }
        //         } else {
        //             // Reset silence timer if sound is detected
        //             silenceStartTime = Time.time;
        //         }
        //     }
        // }
        if (recording && Microphone.GetPosition(null) >= clip.samples) {
            StopRecording();
        }
    }

    // private bool IsSilent() {
    //     // Get the current position in the microphone input
    //     int micPosition = Microphone.GetPosition(null);

    //     // Ensure we have a valid clip and that the micPosition is within bounds
    //     if (clip == null || micPosition <= 0 || micPosition > clip.samples) {
    //         return true;
    //     }

    //     // Create a buffer to hold recent audio samples (256 samples)
    //     float[] samples = new float[256];

    //     // Get audio data from the clip, starting from the microphone position minus the buffer size
    //     clip.GetData(samples, micPosition - samples.Length);

    //     // Calculate the average absolute sample value
    //     float sum = 0;
    //     foreach (float sample in samples) {
    //         sum += Mathf.Abs(sample);
    //     }

    //     // Calculate average amplitude of the samples
    //     float averageAmplitude = sum / samples.Length;

    //     // Return whether the average amplitude is below the silence threshold
    //     return averageAmplitude < silenceThreshold;
    // }

    private void OnTriggerEnter(Collider other) {
        // Only start recording if not already recording and the player enters the collider
        if (!recording && other.CompareTag("Player")) {
            StartRecording();
        }
    }

    private void StartRecording() {
        text.color = Color.white;
        text.text = "Recording...";
        
        // Ensure only one recording starts
        if (Microphone.IsRecording(null)) {
            Debug.Log("Already recording.");
            return; // Avoid starting a new recording if already recording
        }

        // Start recording from the microphone
        clip = Microphone.Start(null, false, 5, 44100); // 10-second recording time, can be adjusted
        startTime = Time.time;
        recording = true;
    }

    private void StopRecording() {
        int position = Microphone.GetPosition(null);
        Microphone.End(null);

        // Get the recorded data from the clip
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);

        // Encode the audio data to WAV format
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        recording = false;
        SendRecording();
    }

    private void SendRecording() {
        text.color = Color.yellow;
        text.text = "Sending...";
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response => {
            text.color = Color.white;
            text.text = response;

            // Log to confirm the response
            Debug.Log("Response from ASR: " + response);

            // Check for null and update textResponse only if it's not null
            if (response == "Hello." || response == "Hello") {
                Debug.Log("Setting textResponse.text to 'Hello there!'");
                textResponse.text = "Hello there!";  // Update textResponse
            }
            
        }, error => {
            text.color = Color.red;
            text.text = error;
        });
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16); // PCM format
                writer.Write((ushort)1); // mono or stereo (1 or 2)
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2); // byte rate
                writer.Write((ushort)(channels * 2)); // block align
                writer.Write((ushort)16); // bit depth
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2); // data size

                // Write samples to the WAV file
                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}
