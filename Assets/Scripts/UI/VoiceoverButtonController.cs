using UnityEngine;
using UnityEngine.UI;

public class VoiceoverButtonController : MonoBehaviour
{
    private Button button;
    private int carIndex = 0;
    private AudioManager audioManager;

    void Start()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(PlayVoiceover);
        }

        audioManager = FindObjectOfType<AudioManager>();

        // Try to find which car this belongs to
        FindCarIndex();
    }

    void FindCarIndex()
    {
        // Walk up the parent chain to find the vehicle
        Transform current = transform.parent;
        while (current != null)
        {
            VehicleIdentifier identifier = current.GetComponent<VehicleIdentifier>();
            if (identifier != null)
            {
                carIndex = identifier.GetCarIndex();
                break;
            }
            current = current.parent;
        }
    }

    void PlayVoiceover()
    {
        if (audioManager != null)
        {
            audioManager.PlayVoiceover(carIndex);

            // Optional: Change button color temporarily to show it's playing
            if (button != null)
            {
                ColorBlock colors = button.colors;
                colors.normalColor = Color.green;
                button.colors = colors;

                // Reset after 20 seconds (voiceover max length)
                Invoke(nameof(ResetButtonColor), 20f);
            }
        }
    }

    void ResetButtonColor()
    {
        if (button != null)
        {
            ColorBlock colors = button.colors;
            colors.normalColor = new Color(0.035f, 0.518f, 0.890f); // Back to blue
            button.colors = colors;
        }
    }
}