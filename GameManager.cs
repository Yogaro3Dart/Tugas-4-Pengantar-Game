using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject customizationPanel;
    public Button playButton;
    public Button pauseButton;

    [Header("UI Controls & Displays")]
    public Slider speedSlider;
    public TextMeshProUGUI speedValueText;
    public Slider scaleSpeedSlider;
    public TextMeshProUGUI scaleSpeedValueText;
    public Slider rotationSlider;
    public TextMeshProUGUI rotationValueText;

    [Header("Prefab Objek")]
    public GameObject[] objectPrefabs;

    [Header("Pengaturan Default")]
    public Color[] colorPalette;

    private GameObject activeObject;
    private Cube activeObjectScript;
    private bool isColorSelected = false;

    void Start()
    {
        customizationPanel.SetActive(false);
        if (playButton != null) playButton.gameObject.SetActive(false);
        if (pauseButton != null) pauseButton.gameObject.SetActive(false);
    }

    public void ShowCustomizationUI()
    {
        customizationPanel.SetActive(true);
        if (playButton != null) playButton.gameObject.SetActive(true);
        if (pauseButton != null) pauseButton.gameObject.SetActive(false);
        isColorSelected = false;
        SelectObject(0);
    }

    public void SelectObject(int prefabIndex)
    {
        Vector3 startPosition = new Vector3(0, 1, 5);
        Quaternion startRotation = Quaternion.identity;
        Vector3 startScale = Vector3.one;
        bool wasPlaying = false;
        Cube oldScript = activeObjectScript;

        if (activeObject != null)
        {
            startPosition = activeObject.transform.position;
            startRotation = activeObject.transform.rotation;
            startScale = activeObject.transform.localScale;
            if (oldScript != null) wasPlaying = oldScript.isPlaying;
            Destroy(activeObject);
        }

        if (prefabIndex < 0 || prefabIndex >= objectPrefabs.Length)
        {
            Debug.LogError("Index prefab tidak valid!");
            return;
        }
        
        activeObject = Instantiate(objectPrefabs[prefabIndex], startPosition, startRotation);
        activeObject.transform.localScale = startScale;
        activeObjectScript = activeObject.GetComponent<Cube>();

        if (activeObjectScript != null)
        {
            if (oldScript != null)
            {
                activeObjectScript.speed = oldScript.speed;
                activeObjectScript.scaleSpeed = oldScript.scaleSpeed;
                activeObjectScript.rotationSpeed = oldScript.rotationSpeed;
                activeObjectScript.colorChangeSpeed = oldScript.colorChangeSpeed;
                activeObjectScript.opacitySpeed = oldScript.opacitySpeed;
                activeObjectScript.SetColor(oldScript.GetComponent<MeshRenderer>().material.color);
            }
            
            activeObjectScript.isPlaying = wasPlaying;
            
            // --- BARU: Mengatur batas min/max slider berdasarkan nilai dari Cube.cs ---
            if(speedSlider != null)
            {
                speedSlider.minValue = activeObjectScript.minSpeed;
                speedSlider.maxValue = activeObjectScript.maxSpeed;
            }
            if(scaleSpeedSlider != null)
            {
                scaleSpeedSlider.minValue = activeObjectScript.minScaleSpeed;
                scaleSpeedSlider.maxValue = activeObjectScript.maxScaleSpeed;
            }
            if(rotationSlider != null)
            {
                rotationSlider.minValue = activeObjectScript.minRotationSpeed;
                rotationSlider.maxValue = activeObjectScript.maxRotationSpeed;
            }
            // --------------------------------------------------------------------------

            UpdateSlidersAndText();
        }
    }

    private void UpdateSlidersAndText()
    {
        if (activeObjectScript == null) return;

        // Kode ini sekarang akan secara otomatis menyesuaikan nilai slider
        // dalam rentang min/max yang baru ditetapkan.
        if (speedSlider != null) speedSlider.value = activeObjectScript.speed;
        if (scaleSpeedSlider != null) scaleSpeedSlider.value = activeObjectScript.scaleSpeed;
        if (rotationSlider != null) rotationSlider.value = activeObjectScript.rotationSpeed;
        
        if (speedValueText != null) speedValueText.text = activeObjectScript.speed.ToString("F2");
        if (scaleSpeedValueText != null) scaleSpeedValueText.text = activeObjectScript.scaleSpeed.ToString("F2");
        if (rotationValueText != null) rotationValueText.text = activeObjectScript.rotationSpeed.ToString("F2");
    }

    // Metode lain (SelectColor, SetSpeed, PlayGame, dll) tetap sama...
    public void SelectColor(int colorIndex)
    {
        if (activeObjectScript != null && colorIndex < colorPalette.Length)
        {
            activeObjectScript.SetColor(colorPalette[colorIndex]);
            isColorSelected = true;
        }
    }

    public void SetSpeed(float value)
    {
        if (activeObjectScript != null) activeObjectScript.speed = value;
        if (speedValueText != null) speedValueText.text = value.ToString("F2");
    }

    public void SetScaleSpeed(float value)
    {
        if (activeObjectScript != null) activeObjectScript.scaleSpeed = value;
        if (scaleSpeedValueText != null) scaleSpeedValueText.text = value.ToString("F2");
    }

    public void SetRotationSpeed(float value)
    {
        if (activeObjectScript != null) activeObjectScript.rotationSpeed = value;
        if (rotationValueText != null) rotationValueText.text = value.ToString("F2");
    }

    public void PlayGame()
    {
        if (activeObjectScript != null)
        {
            if (!isColorSelected)
            {
                activeObjectScript.SetColor(Color.white);
            }
            activeObjectScript.isPlaying = true;
        }

        playButton.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void PauseGame()
    {
        if (activeObjectScript != null)
        {
            activeObjectScript.isPlaying = false;
        }

        playButton.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
    }
}