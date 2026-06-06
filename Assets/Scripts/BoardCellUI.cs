using UnityEngine;
using UnityEngine.UI;

public class BoardCellUI : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform cardAnchor;
    [SerializeField] private Image backgroundImage;


    public void SelectCell()
    {
        CardUI selected =
            SelectionManager.Instance.SelectedCard;

        if(selected == null)
        {
            Debug.Log("No card selected");
            return;
        }

        //bool success = gameManager.PlayCard(selected.Card,row,col);

        gameManager.RequestPlayCardRpc(selected.Card.GetCardID(), row, col);

        /*
        if(success)
        {
            selected.transform.SetParent(cardAnchor, false);
            selected.transform.SetAsLastSibling();

            RectTransform rt = selected.GetComponent<RectTransform>();

            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;

            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;

            rt.localScale = Vector3.one;
            rt.anchoredPosition = Vector2.zero;

            Button cardButton = selected.GetComponent<Button>();

            cardButton.enabled = false;

            Button cellButton = GetComponent<Button>();

            cellButton.interactable = false;

            backgroundImage.enabled = false;

            SelectionManager.Instance.SelectedCard = null;
        }
        */
    /*
        Debug.Log(
            $"Place {selected.Card.GetCardID()} at {row},{col}"
        );
    */
    }
}
