using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public CharacterController parent;
    CharacterController player;
    public List<CharacterController> playerDetect = new List<CharacterController>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<CharacterController>();
            //Debug.Log("OnTriggerEnter: " + other.name);
            if (player != null)
            {
                //Debug.Log("OnTriggerEnter: " + player.transform.parent.name);
                if (player != parent && !playerDetect.Contains(player))
                {
                    playerDetect.Add(player);

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.LogError("OnTriggerExit : " + other.name);
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
