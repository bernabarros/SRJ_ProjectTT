using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text turnText;
    public Player CurrentPlayer;
    private Board board = new Board();
    private PlayerHand blueHand = new();
    private PlayerHand redHand = new();

    private void Start()
    {
        FirstTurnPlayer();
        UpdateTurnUI();
    }

    public bool PlayCard(Card card, int row, int col)
    {
        Debug.Log(
        $"Playing {card.GetCardID()} at {row},{col}"
        );
        bool success = board.PlaceCard(card, row , col);

        if(success)
        {
            RemoveCardFromHand(card);

            if(IsBoardFull())
            {
                EndGame();
            }
            else
            {
                SwitchPlayer();
            }
        }

        return success;
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

        UpdateTurnUI();
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
    private bool IsBoardFull()
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
    
    public void AddCardToHand(Card card)
    {
        if(card.GetOwner() == Player.Blue)
        {
            blueHand.AddCard(card);
        }
        else
        {
            redHand.AddCard(card);
        }
    }

    public void RemoveCardFromHand(Card card)
    {
        if(card.GetOwner() == Player.Blue)
        {
            blueHand.RemoveCard(card);
        }
        else
        {
            redHand.RemoveCard(card);
        }
    }
    private int CountCards(Player player)
    {
        int count = 0;

        for(int row = 0; row < 3; row++)
        {
            for(int col = 0; col < 3; col++)
            {
                Card card = board.Grid[row,col];

                if(card != null && card.GetOwner() == player)
                {
                    count++;
                }
            }
        }

        if(player == Player.Blue)
        {
            count += blueHand.Count();
        }
        else
        {
            count += redHand.Count();
        }

        return count;
    }

    private void EndGame()
    {
        int blueScore = CountCards(Player.Blue);
        int redScore = CountCards(Player.Red);

        Debug.Log($"Blue Score: {blueScore}");
        Debug.Log($"Red Score: {redScore}");

        if(blueScore > redScore)
        {
            turnText.text = "Blue Wins!";
        }
        else if(redScore > blueScore)
        {
            turnText.text = "Red Wins!";
        }
        else
        {
            turnText.text = "Draw!";
        }
    }

    private void UpdateTurnUI()
    {
        turnText.text = $"{CurrentPlayer} Turn";

        turnText.color =
        CurrentPlayer == Player.Blue
        ? Color.blue
        : Color.red;
    }
}
