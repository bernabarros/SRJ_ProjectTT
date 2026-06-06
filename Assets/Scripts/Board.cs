using Unity.Netcode;

public class Board
{
    public Card [,] Grid = new Card[3,3];
    /// <summary>
    /// Places card on selected position on Board, receives a selected card and the position on the Board
    /// </summary>
    /// <param name="card">The card to be placed</param>
    /// <param name="row">The row to place the card</param>
    /// <param name="col">The column to place the card</param>
    /// <returns>Returns true if the card placement succeeded and false if it failed</returns>
    public bool PlaceCard(Card card, int row, int col)
    {
        bool placement;

        if(Grid[row,col] != null)
        {
            placement = false;
        }
        else
        {
            Grid[row,col] = card;
            ResolveCaptures(row,col);
            CardUI.RefreshAll();

            placement = true;
        }

        return placement;
    }
    /// <summary>
    /// Checks the positions surrounding the placed card for possible Card Captures
    /// </summary>
    /// <param name="row">Receives the card's row</param>
    /// <param name="col">Receives the card's column</param>
    private void ResolveCaptures(int row, int col)
    {
        Card placedCard = Grid[row,col];

        CheckNorth(placedCard, row, col);
        CheckSouth(placedCard, row, col);
        CheckWest(placedCard, row, col);
        CheckEast(placedCard, row, col);
    }

    //Methods for checking surrounding positions's cards, receive the placed card and its row and column

    private void CheckNorth(Card placedCard, int row, int col)
    {
        if(row <= 0)
        {
            return;
        }

        Card neighbor = Grid[row - 1, col];

        if(neighbor == null || neighbor.GetOwner() == placedCard.GetOwner())
        {
            return;
        }

        if(placedCard.GetNorth() > neighbor.GetSouth())
        {
            neighbor.SetOwner(placedCard.GetOwner());
        }
    }

    private void CheckSouth(Card placedCard, int row, int col)
    {
        if(row >= 2)
        {
            return;
        }

        Card neighbor = Grid[row + 1, col];

        if(neighbor == null || neighbor.GetOwner() == placedCard.GetOwner())
        {
            return;
        }

        if(placedCard.GetSouth() > neighbor.GetNorth())
        {
            neighbor.SetOwner(placedCard.GetOwner());
        }
    }

    private void CheckWest(Card placedCard, int row, int col)
    {
        if(col <= 0)
        {
            return;
        }

        Card neighbor = Grid[row, col - 1];

        if(neighbor == null || neighbor.GetOwner() == placedCard.GetOwner())
        {
            return;
        }

        if(placedCard.GetWest() > neighbor.GetEast())
        {
            neighbor.SetOwner(placedCard.GetOwner());
        }
    }

    private void CheckEast(Card placedCard, int row, int col)
    {
        if(col >= 2)
        {
            return;
        }

        Card neighbor = Grid[row, col + 1];

        if(neighbor == null || neighbor.GetOwner() == placedCard.GetOwner())
        {
            return;
        }

        if(placedCard.GetEast() > neighbor.GetWest())
        {
            neighbor.SetOwner(placedCard.GetOwner());
        }
    }
}
