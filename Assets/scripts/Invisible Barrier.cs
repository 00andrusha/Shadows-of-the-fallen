using UnityEngine;
using TMPro; // Use UnityEngine.UI if you are not using TextMeshPro

public class InvisibleBarrier : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask enemyLayer; // Set this to "Attackable" in Inspector

    [Header("The Barrier")]
    public TextMeshProUGUI messageText;
    private BoxCollider2D wallCollider;

    private bool battleActive = false;

    void Start()
    {
        wallCollider = GetComponent<BoxCollider2D>();
        // Wait 2 seconds for Spawner to finish
        Invoke("StartChecking", 2.0f);
    }

    void StartChecking()
    {
        battleActive = true;
    }

    void Update()
    {
        // 1. Don't check until the battle actually starts
        if (!battleActive) return;

        // 2. THE FIX: Use the new "FindObjectsByType" command
        // This gets every active object in the scene
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        int enemiesAlive = 0;

        // 3. Loop through them and count only the ones on your specific Layer
        foreach (GameObject obj in allObjects)
        {
            // This math checks if the object's layer matches the one you picked
            if (((1 << obj.layer) & enemyLayer) != 0)
            {
                enemiesAlive++;
            }
        }

        // 4. If count is 0, open the wall
        if (enemiesAlive == 0)
        {
            wallCollider.enabled = false;

            if (messageText != null)
            {
                messageText.gameObject.SetActive(true);
                messageText.text = "Go Right";
            }

            this.enabled = false; // Stop checking
        }
    }
}