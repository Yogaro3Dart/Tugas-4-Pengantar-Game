using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private MeshRenderer _renderer;
    private Material _material;
    public bool isPlaying = false;

    [Header("Variabel Gerakan")]
    private Vector3 targetPosition;
    public float speed = 2.0f;

    [Header("Variabel Ukuran")]
    private Vector3 targetScale;
    public float scaleSpeed = 2.0f;

    [Header("Variabel Rotasi")]
    private Quaternion targetRotation;
    public float rotationSpeed = 10.0f;

    [Header("Variabel Warna")]
    private Color targetColor;
    public float colorChangeSpeed = 1.0f;
    public float opacitySpeed = 0.5f;
    
    // --- BARU: Batas nilai untuk dihubungkan ke UI Slider ---
    [Header("Batas Nilai Slider")]
    public float minSpeed = 1f;
    public float maxSpeed = 10f;
    public float minScaleSpeed = 1f;
    public float maxScaleSpeed = 5f;
    public float minRotationSpeed = 10f;
    public float maxRotationSpeed = 100f;
    // ---------------------------------------------------------

    void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
        _material = _renderer.material;
    }

    void Start()
    {
        targetPosition = transform.position;
        targetScale = transform.localScale;
        targetRotation = transform.rotation;
        targetColor = _material.color;
    }

    void Update()
    {
        if (!isPlaying) return;

        // Logika gerakan, rotasi, dan skala tetap sama...
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1.0f)
        {
            float randomX = Random.Range(0f, 360f);
            float randomY = Random.Range(0f, 360f);
            float randomZ = Random.Range(0f, 360f);
            targetRotation = Quaternion.Euler(randomX, randomY, randomZ);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            float randomX = Random.Range(-5f, 5f);
            float randomY = Random.Range(-5f, 8f);
            float randomZ = Random.Range(-10f, 15f);
            targetPosition = new Vector3(randomX, randomY, randomZ);
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.localScale, targetScale) < 0.1f)
        {
            float randomScale = Random.Range(0.5f, 2.5f);
            targetScale = Vector3.one * randomScale;
        }
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, scaleSpeed * Time.deltaTime);

        if (Vector4.Distance(_material.color, targetColor) < 0.1f)
        {
            targetColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        }

        Color nextColor = Color.Lerp(_material.color, targetColor, colorChangeSpeed * Time.deltaTime);
        float oscillatingAlpha = Mathf.Lerp(0.1f, 1.0f, (Mathf.Sin(Time.time * opacitySpeed) + 1.0f) / 2.0f);
        nextColor.a = oscillatingAlpha;
        _material.color = nextColor;
    }
    
    public void SetColor(Color newColor)
    {
        _material.color = newColor;
        targetColor = newColor;
    }
}