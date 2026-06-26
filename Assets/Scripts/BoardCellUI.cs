using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardCellUI : MonoBehaviour
{
    [SerializeField] private int row;
    [SerializeField] private int col;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform cardAnchor;
    [SerializeField] private Image backgroundImage;

    private static Dictionary<(int,int), BoardCellUI> cells = new();

    private void Awake()
    {
        cells[(row,col)] = this;
    }

    public static BoardCellUI Find(int row,int col)
    {
        return cells[(row,col)];
    }

    public void PlaceCardVisual(CardUI cardUI)
    {
        cardUI.transform.SetParent(
            cardAnchor,
            false
        );

        cardUI.transform.SetAsLastSibling();

        RectTransform rt =
            cardUI.GetComponent<RectTransform>();

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;

        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        rt.localScale = Vector3.one;
        rt.anchoredPosition = Vector2.zero;

        Button cardButton =
            cardUI.GetComponent<Button>();

        cardButton.enabled = false;

        Button cellButton =
            GetComponent<Button>();

        cellButton.interactable = false;

        backgroundImage.enabled = false;
    }

    
    public void SelectCell()
    {
        CardUI selected =
            SelectionManager.Instance.SelectedCard;

        if(selected == null)
        {
            Debug.Log("No card selected");
            return;
        }
        gameManager.RequestPlayCard(selected.Card,row,col);
    }
    public static void ResetAllCells()
    {
        foreach(var cell in cells.Values)
        {
            Button button = cell.GetComponent<Button>();

            button.interactable = true;

            cell.backgroundImage.enabled = true;
        }
    }
}
