using System.Collections;
using UnityEngine;

public class InfoPopupController : MonoBehaviour
{
    [Header("Pengaturan UI")]
    [Tooltip("Panel utama dari popup yang akan ditampilkan/disembunyikan.")]
    public GameObject popupPanel;

    [Tooltip("Referensi ke GameManager untuk memberitahu kapan harus melanjutkan.")]
    public GameManager gameManager;

    [Header("Pengaturan Waktu")]
    [Tooltip("Berapa lama (dalam detik) popup akan ditampilkan sebelum hilang.")]
    public float displayDuration = 3.0f;

    void Start()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
            StartCoroutine(HidePopupAfterDelay());
        }
        else
        {
            Debug.LogError("Popup Panel atau GameManager belum di-set di Inspector pada InfoPopupController!");
        }
    }

    private IEnumerator HidePopupAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        popupPanel.SetActive(false);

        if (gameManager != null)
            gameManager.ShowCustomizationUI();
    }
}