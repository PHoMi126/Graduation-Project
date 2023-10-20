using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public CharacterController player;
    public List<CharacterController> playerDetect = new List<CharacterController>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<CharacterController>();
            if (player != null && !playerDetect.Contains(player))
            {
                playerDetect.Add(player);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CharacterController enemy = other.GetComponent<CharacterController>();
        if (enemy != null && playerDetect.Contains(enemy))
        {
            playerDetect.Remove(enemy);
        }
    }

    public GameObject FindTheTarget()
    {
        if (playerDetect.Count == 0 && playerDetect != null)
        {
            return null;
        }
        else
        {
            return playerDetect[0].gameObject;
        }
    }
}
