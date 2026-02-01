using UnityEngine;

public class LinearHallwayGen : MonoBehaviour
{
    [Header("Level Settings")]
    public int levelLength = 20;     // How many hallway segments to spawn
    public float segmentWidth = 10f; // The width of your sprites (X axis)

    [Header("Prefabs")]
    public GameObject startRoom;     // The safe starting area
    public GameObject endRoom;       // The elevator/door at the end
    public GameObject[] hallwayVariants; // Different scary hallways (Empty, Blood, Windows, etc.)

    void Start()
    {
        GenerateHallway();
    }

    void GenerateHallway()
    {
        Vector2 currentPos = transform.position;

        // 1. Spawn Start Room
        Instantiate(startRoom, currentPos, Quaternion.identity, transform);

        // Move forward for the next piece
        currentPos.x += segmentWidth;

        // 2. Spawn the Middle Sections
        for (int i = 0; i < levelLength; i++)
        {
            // Pick a random hallway style (e.g., Bloody, Dark, Flashing Light)
            GameObject segment = hallwayVariants[Random.Range(0, hallwayVariants.Length)];

            Instantiate(segment, currentPos, Quaternion.identity, transform);

            // Move the cursor to the right
            currentPos.x += segmentWidth;
        }

        // 3. Spawn End Room
        Instantiate(endRoom, currentPos, Quaternion.identity, transform);
    }
}