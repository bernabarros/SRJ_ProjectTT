/// <summary>
/// Class that controls Cards, stores direction values and the current player owner
/// </summary>
public class Card
{
    /// <summary>
    /// ID for the card, single letter from A-Z
    /// </summary>
    private string cardID;
    /// <summary>
    /// Value for North direction
    /// </summary>
    private int north;
    /// <summary>
    /// Value for South direction
    /// </summary>
    private int south;
    /// <summary>
    /// Value for West direction
    /// </summary>
    private int west;
    /// <summary>
    /// Value for East direction
    /// </summary>
    private int east;
    /// <summary>
    /// Current owner of the Card
    /// </summary>
    private Player owner;
    public Card(string cardID, int north, int south, int west, int east, Player player)
    {
        this.cardID = cardID;
        this.north = north;
        this.south = south;
        this.west = west;
        this.east = east;
        owner = player;
    }

    /// <summary>
    /// Return card's North value
    /// </summary>
    /// <returns></returns>
    public int GetNorth()
    {
        return north;
    }

    /// <summary>
    /// Return card's South value
    /// </summary>
    /// <returns></returns>
    public int GetSouth()
    {
        return south;
    }

    /// <summary>
    /// Return card's West value
    /// </summary>
    /// <returns></returns>
    public int GetWest()
    {
        return west;
    }
    
    /// <summary>
    /// Return card's East value
    /// </summary>
    /// <returns></returns>
    public int GetEast()
    {
        return east;
    }
    /// <summary>
    /// Returns card's owner
    /// </summary>
    /// <returns></returns>
    public Player GetOwner()
    {
        return owner;
    }
    /// <summary>
    /// Set card's owner value
    /// </summary>
    /// <param name="newowner"></param>
    public void SetOwner(Player newowner)
    {
        owner = newowner;
    }
}
