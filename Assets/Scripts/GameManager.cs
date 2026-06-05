using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Player CurrentPlayer;
    private Board board = new Board();

    public void PlayCard(Card card, int row, int col)
    {
        bool success = board.PlaceCard(card, row , col);

        if(success)
        {
            SwitchPlayer();
        }
    }
    /// <summary>
    /// Method for switching Player turn
    /// </summary>
    private void SwitchPlayer()
    {
        CurrentPlayer = 
        CurrentPlayer == Player.Blue 
        ? Player.Red 
        : Player.Blue;
    }
    /// <summary>
    /// Method for deciding which player goes first by comparing randomly generated numbers 
    /// </summary>
    private void FirstTurnPlayer()
    {
        int bluePlayer = 0;
        int redPlayer = 0;
        while(bluePlayer == redPlayer)
        {
            bluePlayer = Random.Range(0,10);
            redPlayer = Random.Range(0,10);

            if(bluePlayer > redPlayer)
            {
                CurrentPlayer = Player.Blue;
            }
            else
            {
                CurrentPlayer = Player.Red;
            }
        }
    }
    public bool IsBoardFull()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if(board.Grid[row,col] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
