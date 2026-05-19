using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("Menu Buttons")]
    [SerializeField] private Button btnMarkerless;
    [SerializeField] private Button btnMarkerBased;
    [SerializeField] private Button btnQuit;

    [Header("Vehicle UI Panel")]
    [SerializeField] private GameObject vehicleUIPanel;
    [SerializeField] private Button btnChangeColor;
    [SerializeField] private Button btnChangeWheel;
    [SerializeField] private Button btnEngineToggle;
    [SerializeField] private Button btnRotationToggle;

    [Header("Voiceover Button - World Space)")]
    [SerializeField] private Button voiceoverButton;

    private bool isUIVisible = false;

    void Start()
    {
        // Menu buttons
        if (btnMarkerless != null)
            btnMarkerless.onClick.AddListener(StartMarkerlessMode);

        if (btnMarkerBased != null)
            btnMarkerBased.onClick.AddListener(StartMarkerBasedMode);

        if (btnQuit != null)
            btnQuit.onClick.AddListener(QuitGame);

        // Vehicle UI buttons - these will be connected after vehicle spawns
        if (btnChangeColor != null)
            btnChangeColor.onClick.AddListener(OnColorButtonPressed);

        if (btnChangeWheel != null)
            btnChangeWheel.onClick.AddListener(OnWheelButtonPressed);

        if (btnEngineToggle != null)
            btnEngineToggle.onClick.AddListener(OnEngineButtonPressed);

        if (btnRotationToggle != null)
            btnRotationToggle.onClick.AddListener(OnRotationButtonPressed);

        if (voiceoverButton != null)
            voiceoverButton.onClick.AddListener(OnVoiceoverButtonPressed);

        // Hide vehicle UI initially
        if (vehicleUIPanel != null)
            vehicleUIPanel.SetActive(false);
    }

    void StartMarkerlessMode()
    {
        // Play click sound
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        // Load markerless scene (Member 1's scene)
        SceneManager.LoadScene("AR_Markerless");
    }

    void StartMarkerBasedMode()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        SceneManager.LoadScene("AR_MarkerBased");
    }

    void QuitGame()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Called by Member 2's vehicle spawn script
    public void OnVehicleSpawned(GameObject vehicle)
    {
        ShowVehicleUI();

        // Connect vehicle reference to interaction scripts
        VehicleInteraction vehicleInteraction = vehicle.GetComponent<VehicleInteraction>();
        if (vehicleInteraction != null)
        {
            vehicleInteraction.SetUIController(this);
        }
    }

    public void ShowVehicleUI()
    {
        if (vehicleUIPanel != null)
            vehicleUIPanel.SetActive(true);
        isUIVisible = true;
    }

    public void HideVehicleUI()
    {
        if (vehicleUIPanel != null)
            vehicleUIPanel.SetActive(false);
        isUIVisible = false;
    }

    void OnColorButtonPressed()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        // Find current vehicle and change color
        GameObject vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        if (vehicle != null)
        {
            VehicleCustomization customization = vehicle.GetComponent<VehicleCustomization>();
            if (customization != null)
                customization.ChangeColor();
        }
    }

    void OnWheelButtonPressed()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        GameObject vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        if (vehicle != null)
        {
            VehicleCustomization customization = vehicle.GetComponent<VehicleCustomization>();
            if (customization != null)
                customization.ChangeWheels();
        }
    }

    void OnEngineButtonPressed()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        AudioManager.Instance.ToggleEngine();

        // Also update button visual
        Text buttonText = btnEngineToggle.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            bool isRunning = AudioManager.Instance.IsEngineRunning();
            buttonText.text = isRunning ? "ENGINE ON" : "ENGINE OFF";
        }
    }

    void OnRotationButtonPressed()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayButtonClick();

        GameObject vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        if (vehicle != null)
        {
            VehicleTurntable turntable = vehicle.GetComponent<VehicleTurntable>();
            if (turntable != null)
                turntable.ToggleRotation();

            // Update button visual
            Text buttonText = btnRotationToggle.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                bool isRotating = turntable != null && turntable.IsRotating();
                buttonText.text = isRotating ? "ROTATE ON" : "ROTATE OFF";
            }
        }
    }

    void OnVoiceoverButtonPressed()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
            // Play voiceover for current car (0 or 1 based on which car is active)
            AudioManager.Instance.PlayVoiceover(GetCurrentCarIndex());
        }
    }

    int GetCurrentCarIndex()
    {
        GameObject vehicle = GameObject.FindGameObjectWithTag("Vehicle");
        if (vehicle != null)
        {
            VehicleIdentifier identifier = vehicle.GetComponent<VehicleIdentifier>();
            if (identifier != null)
                return identifier.carIndex;
        }
        return 0;
    }
}