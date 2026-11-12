using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    private int experienceValue;

    public void SetExperience(int value)
    {
        experienceValue = value; //cantidad de exp q dara la orb
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; //solo el player agarra exp


        PlayerLevel playerLevel =other.GetComponent<PlayerLevel>(); //busca quien maneja los niveles

        if (playerLevel != null)
        {
            Debug.Log("Player ganó " +experienceValue +" EXP");

            playerLevel.AddExperience(experienceValue); //suma exp al jugador 

            Destroy(gameObject); //orbe desaparece al ser recogida
        }
    }
}
