using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    public CardUI SelectedCard;

    private void Awake()
    {
        Instance = this;
    }
}
