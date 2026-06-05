using UnityEngine;
/// <summary>
/// Class that controls Cards, stores values and the current player owner
/// </summary>
public class Card
{
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
    //private Player owner;
    public Card(int north, int south, int west, int east/*Player player*/)
    {
        this.north = north;
        this.south = south;
        this.west = west;
        this.east = east;
    }

    public int GetNorth()
    {
        return north;
    }
    public int GetSouth()
    {
        return south;
    }
    public int GetWest()
    {
        return west;
    }
    public int GetEast()
    {
        return east;
    }
}
