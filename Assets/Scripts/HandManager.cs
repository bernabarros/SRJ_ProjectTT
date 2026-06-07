using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HandManager : NetworkBehaviour
{
    
    [SerializeField] private Transform blueHand;
    [SerializeField] private Transform redHand;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!IsServer)
        {
           return; 
        }
        CreatePlayerHands();
    }

    private void CreatePlayerHands()
    {
        
        Card[] cardsBlue =
        {
            GenerateCard("B1", Player.Blue),
            GenerateCard("B2", Player.Blue),
            GenerateCard("B3", Player.Blue),
            GenerateCard("B4", Player.Blue),
            GenerateCard("B5", Player.Blue)
        };

        Card[] cardsRed =
        {
            GenerateCard("R1", Player.Red),
            GenerateCard("R2", Player.Red),
            GenerateCard("R3", Player.Red),
            GenerateCard("R4", Player.Red),
            GenerateCard("R5", Player.Red)
        };

        foreach (var c in cardsBlue)
        {
            CreateCardClientRpc(
                c.GetCardID(),
                c.GetNorth(),
                c.GetSouth(),
                c.GetWest(),
                c.GetEast(),
                (int)Player.Blue
            );
        }

        foreach (var c in cardsRed)
        {
            CreateCardClientRpc(
                c.GetCardID(),
                c.GetNorth(),
                c.GetSouth(),
                c.GetWest(),
                c.GetEast(),
                (int)Player.Red
            );
        }
    }

    private Card GenerateCard(string id, Player owner)
    {
        int north = Random.Range(1,10);
        int south = Random.Range(1,10);
        int west = Random.Range(1,10);
        int east = Random.Range(1,10);

        Debug.Log(
            $"Generating {id} Owner={owner} N={north} S={south} W={west} E={east}"
        );

        return new Card(
            id,
            north,
            south,
            west,
            east,
            owner
        );
    }

    [ClientRpc] private void CreateCardClientRpc(string id, int north, int south, int west, int east, int owner)
    {
        Card card = new Card(id, north, south, west, east, (Player)owner);

        Transform parent = (owner == (int)Player.Blue) ? blueHand : redHand;

        GameObject obj = Instantiate(cardPrefab, parent);
        obj.GetComponent<CardUI>().CardSetup(card);

        //gameManager.AddCardToHand(card);
    }
}
