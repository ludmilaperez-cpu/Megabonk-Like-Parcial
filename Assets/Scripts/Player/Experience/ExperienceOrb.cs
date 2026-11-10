using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    private int experienceValue;

    public void SetExperience(int value)
    {
        experienceValue = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerLevel playerLevel =
            other.GetComponent<PlayerLevel>();

        if (playerLevel != null)
        {
            playerLevel.AddExperience(experienceValue);

            Destroy(gameObject);
        }
    }
}
