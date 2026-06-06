using UnityEngine;

public class HandManager : MonoBehaviour
{
    
    [SerializeField] private Transform blueHand;
    [SerializeField] private Transform redHand;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateTestCards();
    }

    private void CreateTestCards()
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

        foreach(Card card in cardsBlue)
        {
            gameManager.AddCardToHand(card);

            GameObject obj = Instantiate(cardPrefab, blueHand);

            obj.GetComponent<CardUI>().CardSetup(card);
        }
        foreach(Card card in cardsRed)
        {
            gameManager.AddCardToHand(card);

            GameObject obj = Instantiate(cardPrefab, redHand);

            obj.GetComponent<CardUI>().CardSetup(card);
        }
    }

    private Card GenerateCard(string id, Player owner)
    {
        int north = Random.Range(1,10);
        int south = Random.Range(1,10);
        int west = Random.Range(1,10);
        int east = Random.Range(1,10);

        return new Card(
            id,
            north,
            south,
            west,
            east,
            owner
        );
    }
}
