using UnityEngine;

public class HandManager : MonoBehaviour
{
    
    [SerializeField] private Transform blueHand;
    [SerializeField] private Transform redHand;
    [SerializeField] private GameObject cardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateTestCards();
    }

    private void CreateTestCards()
    {
        Card[] cardsBlue =
        {
            new Card("A",8,2,3,5,Player.Blue),
            new Card("B",7,4,6,1,Player.Blue),
            new Card("C",5,5,5,5,Player.Blue),
            new Card("D",9,2,8,3,Player.Blue),
            new Card("E",1,9,2,8,Player.Blue)
        };

        Card[] cardsRed =
        {
            new Card("A",8,2,3,5,Player.Red),
            new Card("B",7,4,6,1,Player.Red),
            new Card("C",5,5,5,5,Player.Red),
            new Card("D",9,2,8,3,Player.Red),
            new Card("E",1,9,2,8,Player.Red)
        };

        foreach(Card card in cardsBlue)
        {
            GameObject obj =
                Instantiate(cardPrefab, blueHand);

            obj.GetComponent<CardUI>()
            .CardSetup(card);
        }
        foreach(Card card in cardsRed)
        {
            GameObject obj =
                Instantiate(cardPrefab, redHand);

            obj.GetComponent<CardUI>()
            .CardSetup(card);
        }
        //Card cardA = new Card ("A", 8, 2, 3, 5, Player.Blue);

        //GameObject obj = Instantiate(cardPrefab, blueHand);

        //obj.GetComponent<CardUI>().CardSetup(cardA);
    }
}
