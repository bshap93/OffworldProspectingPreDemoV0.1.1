using Rewired;
using UnityEngine;

public class RewiredCustomInput : MonoBehaviour
{
    // The Rewired player id of this character
    public int playerId;
    private Player player; // The Rewired Player

    private void Awake()
    {
        // Get the Rewired Player
        player = ReInput.players.GetPlayer(playerId);
    }


    private void Update()

    {
        GetInput();
        ProcessInput();
    }

    private void GetInput()
    {
    }

    private void ProcessInput()
    {
    }
}