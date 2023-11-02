using UnityEngine;

public class GameOver : MonoBehaviour
{
    public Transform player;
    public Transform gameOver;
    public AnimStates state;

    private void Start()
    {
        PlayerDead();
    }

    private void PlayerDead()
    {
        if (state.isDead)
        {
            player.gameObject.SetActive(false);
            gameOver.gameObject.SetActive(true);
        }
    }
}
