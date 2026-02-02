using UnityEngine;
using UnityEngine.Timeline;

// This script runs automatically once, cleans the mess, then you can delete it.
[ExecuteInEditMode]
public class FixSignals : MonoBehaviour
{
    public bool KILL_THE_RECEIVER = false;

    void Update()
    {
        if (KILL_THE_RECEIVER)
        {
            var receiver = GetComponent<SignalReceiver>();
            if (receiver != null)
            {
                Debug.Log("Broken Signal Receiver Found... DESTROYING IT.");
                DestroyImmediate(receiver);
            }
            KILL_THE_RECEIVER = false;
        }
    }
}