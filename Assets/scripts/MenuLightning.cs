using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class MenuLightning : MonoBehaviour
{
    [Header("Settings")]
    public float minTime = 2f;
    public float maxTime = 8f;
    public float strikeDuration = 0.2f;

    [Header("Shape Settings")]
    public int segments = 20;        // More segments = smoother/more detail
    public float randomness = 2.0f;  // How wide the zig-zags are
    public float boltLength = 15f;   // How far down it goes

    [Header("Components")]
    public Light thunderLight;       // Optional: Drag a Light source here
    public AudioClip thunderSound;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;
    private float timer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Hide bolt initially
        lineRenderer.positionCount = 0;

        // Start the first timer
        timer = Random.Range(minTime, maxTime);
        
        audioSource = GetComponent<AudioSource>();

        // DEBUG TEST: Play immediately on start
        audioSource.PlayOneShot(thunderSound);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            StartCoroutine(Strike());
            timer = Random.Range(minTime, maxTime);
        }
    }

    IEnumerator Strike()
    {
        // 1. GENERATE THE GEOMETRY
        // We calculate random points from top to bottom
        lineRenderer.positionCount = segments;

        Vector3 currentPos = transform.position;
        // Start point
        lineRenderer.SetPosition(0, currentPos);

        float segmentLength = boltLength / segments;

        for (int i = 1; i < segments; i++)
        {
            // Move down
            currentPos.y -= segmentLength;

            // Move random Left/Right (Zig Zag)
            // We use center-biased random so it doesn't drift too far off screen
            float offset = Random.Range(-randomness, randomness);
            currentPos.x += offset;

            // Correction: Pull x back towards center slightly so it looks vertical overall
            currentPos.x = Mathf.Lerp(currentPos.x, transform.position.x, 0.3f);

            lineRenderer.SetPosition(i, currentPos);
        }

        // 2. VISUAL FLASH
        // Enable Light
        if (thunderLight != null) thunderLight.enabled = true;

        // Play Sound
        if (audioSource && thunderSound) audioSource.PlayOneShot(thunderSound);

        // 3. FADE OUT
        float t = 0;
        while (t < strikeDuration)
        {
            t += Time.deltaTime;

            // Jitter the width to make it look unstable
            lineRenderer.widthMultiplier = Mathf.Lerp(1f, 0f, t / strikeDuration);

            yield return null;
        }

        // Cleanup
        lineRenderer.positionCount = 0;
        if (thunderLight != null) thunderLight.enabled = false;
    }
}
