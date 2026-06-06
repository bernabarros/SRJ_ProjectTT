using System.Collections.Generic;
using UnityEngine;

public class PlayerHand
{
    private List<Card> cards = new();

    public void AddCard(Card card)
    {
        cards.Add(card);
    }

    public void RemoveCard(Card card)
    {
        cards.Remove(card);
    }
    public int Count()
    {
        return cards.Count;
    }

    public List<Card> GetCards()
    {
        return cards;
    }
}
