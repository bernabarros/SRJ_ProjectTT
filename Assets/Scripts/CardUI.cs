using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TMP_Text idText;
    [SerializeField] private TMP_Text northText;
    [SerializeField] private TMP_Text southText;
    [SerializeField] private TMP_Text westText;
    [SerializeField] private TMP_Text eastText;
    [SerializeField] private Image background;
    public Card Card {get; private set;}

    private static List<CardUI> allCards = new List<CardUI>();

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        allCards.Add(this);
    }

    public void CardSetup(Card card)
    {
        Card = card;

        idText.text = card.GetCardID();

        northText.text = card.GetNorth().ToString();
        southText.text = card.GetSouth().ToString();
        westText.text = card.GetWest().ToString();
        eastText.text = card.GetEast().ToString();

        background.color =
            card.GetOwner() == Player.Blue
            ? Color.blue
            : Color.red;
    }
    public void SelectCard()
    {
        if(Card.GetOwner() != gameManager.currentPlayer.Value)
        {
            Debug.Log("Not your turn");
            return;
        }

        SelectionManager.Instance.SelectedCard = this;

        Debug.Log("Selected " + Card.GetCardID());
    }

    public void RefreshVisual()
    {
        if (Card == null) return;

        if(Card.GetOwner() == Player.Blue)
        {
            background.color = Color.blue;
        }
        else
        {
            background.color = Color.red;
        }
    }

    public static void RefreshAll()
    {
        foreach (var c in allCards)
            c.RefreshVisual();
    }
}
