using UnityEngine;

public class JumpscareTrigger : MonoBehaviour
{
    [Header("Assign These in Inspector")]
    public GameObject monster;       // Your monster GameObject
    public AudioSource scareSound;  // A loud scream/noise
    public Light redEyeLight;       // The illumination we talked about
    public Animator monsterAnim;    // The animation component

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only trigger if the object is tagged "Player"
        if (other.CompareTag("Player"))
        {
            TriggerTheScare();
        }
    }

    void TriggerTheScare()
    {
        // 1. Make the monster appear
        monster.SetActive(true);

        // 2. Play the sound
        if (scareSound != null) scareSound.Play();

        // 3. Turn on the red illumination
        if (redEyeLight != null) redEyeLight.enabled = true;

        // 4. Play the "Strike" or "Scream" animation
        if (monsterAnim != null) monsterAnim.SetTrigger("Jump");

        // 5. Delete this trigger so it only happens once
        Destroy(gameObject, 0.5f);
    }
}
