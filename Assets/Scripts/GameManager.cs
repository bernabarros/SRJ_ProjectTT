using UnityEngine;
using TMPro;
using Unity.Netcode;
using System.Collections.Generic;

public class GameManager : NetworkBehaviour
{
    /// <summary>
    /// Game UI text variable
    /// </summary>
    [SerializeField] private TMP_Text turnText;

    /// <summary>
    /// NetworkVariable that indicates the turn of the currently connected client player
    /// </summary>
    public NetworkVariable<Player> CurrentPlayer = new NetworkVariable<Player>();

    /// <summary>
    /// Local Client variable that determines what player the local client is
    /// </summary>
    public Player LocalPlayer {get; private set;}


    private Board board = new Board();
    private PlayerHand blueHand = new();
    private PlayerHand redHand = new();

    [SerializeField] private Transform blueHandPosition;
    [SerializeField] private Transform redHandPosition;
    [SerializeField] private GameObject cardPrefab;

    private Dictionary<ulong, Player> clientPlayers = new();
    private Dictionary<string, Card> allCards = new Dictionary<string, Card>();

    public NetworkVariable<bool> MatchStarted = new NetworkVariable<bool>(false);

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += AssignPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandlePlayerDisconnected;
        }

        CurrentPlayer.OnValueChanged += OnTurnChanged;
        MatchStarted.OnValueChanged += OnMatchStartedChanged;

        UpdateTurnUI();
    }

    public override void OnNetworkDespawn()
    {
        CurrentPlayer.OnValueChanged -= OnTurnChanged;
        MatchStarted.OnValueChanged -= OnMatchStartedChanged;

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= AssignPlayer;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandlePlayerDisconnected;
        }
    }

    private void HandlePlayerDisconnected(ulong clientId)
    {
        if (!IsServer)
            return;

        Debug.Log($"Player disconnected: {clientId}");

        clientPlayers.Remove(clientId);

        ResetMatch();

        MatchCancelledClientRpc();
    }

    [ClientRpc]
    private void MatchCancelledClientRpc()
    {
        turnText.text = "Waiting for another player...";
    }
    private void OnMatchStartedChanged(bool oldValue, bool newValue)
    {
        UpdateTurnUI();
    }

    private void OnTurnChanged(Player oldPlayer, Player newPlayer)
    {
        UpdateTurnUI();
    }

    private void AssignPlayer(ulong clientID)
    {
        if(!IsServer)
        {
            return;
        }

        Player assigned;

        if (!clientPlayers.ContainsValue(Player.Blue))
        {
            assigned = Player.Blue;
        }
        else
        {
            assigned = Player.Red;
        }

        clientPlayers[clientID] = assigned;

        SendPlayerAssignmentClientRpc(assigned, new ClientRpcParams{Send = new ClientRpcSendParams{TargetClientIds = new[] {clientID}}});

        if(clientPlayers.Count == 2)
        {
            StartMatch();
        }
    }

    private void StartMatch()
    {
        ResetMatch();

        MatchStarted.Value = true;

        GenerateCards();

        FirstTurnPlayer();

        UpdateTurnUI();
    }

    private void ResetMatch()
    {
        MatchStarted.Value = false;

        board = new Board();

        blueHand = new PlayerHand();
        redHand = new PlayerHand();

        allCards.Clear();

        ResetBoardClientRpc();
    }

    [ClientRpc]
    private void ResetBoardClientRpc()
    {
        CardUI.DestroyAllCards();

        BoardCellUI.ResetAllCells();

        SelectionManager.Instance.SelectedCard = null;
    }
    private void GenerateCards()
    {
        GeneratePlayerCards(Player.Blue);
        GeneratePlayerCards(Player.Red);
    }
    
    private void GeneratePlayerCards(Player owner)
    {
        string prefix = owner == Player.Blue ? "B" : "R";

        Card card = null;

        for(int i = 1; i <= 5; i++)
        {
            card = new Card(
                prefix + i,
                Random.Range(1,10),
                Random.Range(1,10),
                Random.Range(1,10),
                Random.Range(1,10),
                owner
            );

            RegisterCard(card);

            AddCardToHand(card);

            CreateCardClientRpc(
                card.GetCardID(),
                card.GetNorth(),
                card.GetSouth(),
                card.GetWest(),
                card.GetEast(),
                (int)owner
            );
        }
    }

    public void RequestPlayCard(Card card, int row, int col)
    {
        RequestPlayCardRpc(card.GetCardID(), row, col);
    }


    [Rpc(SendTo.Server)]
    public void RequestPlayCardRpc(string cardId, int row, int col, RpcParams rpcParams = default)
    {
        ulong sender = rpcParams.Receive.SenderClientId;

        Debug.Log($"Server received move request for {cardId} at {row},{col}");

        ValidateandPlayMove(sender,cardId,row,col);
    }

    public bool PlayCard(Card card, int row, int col)
    {
        Debug.Log($"Playing {card.GetCardID()} at {row},{col}");

        bool success = board.PlaceCard(card, row , col);

        if(success)
        {

            PlaceCardClientRpc(card.GetCardID(), row, col);

            SyncCardOwners();
        }

        else
        {
            return false;
        }

        RemoveCardFromHand(card);

        if(IsBoardFull())
        {
            EndGame();
        }
        else
        {
            SwitchPlayer();
        }

        return success;
    }
    /// <summary>
    /// Method for switching Player turn
    /// </summary>
    private void SwitchPlayer()
    {
        CurrentPlayer.Value = CurrentPlayer.Value == Player.Blue 
        ? Player.Red 
        : Player.Blue;
    }
    /// <summary>
    /// Method for deciding which player goes first by comparing randomly generated numbers 
    /// </summary>
    private void FirstTurnPlayer()
    {
        CurrentPlayer.Value = Random.Range(0,2) == 0 ? Player.Blue : Player.Red;
    }
    private bool IsBoardFull()
    {
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 3; col++)
            {
                if(board.Grid[row,col] == null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    [ClientRpc]
    private void SendPlayerAssignmentClientRpc(Player player, ClientRpcParams rpcParams = default)
    {
        LocalPlayer = player;
    }
    
    public void AddCardToHand(Card card)
    {
        if(card.GetOwner() == Player.Blue)
        {
            blueHand.AddCard(card);
        }
        else
        {
            redHand.AddCard(card);
        }
    }

    public void RemoveCardFromHand(Card card)
    {
        if(card.GetOwner() == Player.Blue)
        {
            blueHand.RemoveCard(card);
        }
        else
        {
            redHand.RemoveCard(card);
        }
    }
    private int CountCards(Player player)
    {
        int count = 0;

        for(int row = 0; row < 3; row++)
        {
            for(int col = 0; col < 3; col++)
            {
                Card card = board.Grid[row,col];

                if(card != null && card.GetOwner() == player)
                {
                    count++;
                }
            }
        }

        if(player == Player.Blue)
        {
            count += blueHand.Count();
        }
        else
        {
            count += redHand.Count();
        }

        return count;
    }

    private void EndGame()
    {
        int blueScore = CountCards(Player.Blue);
        int redScore = CountCards(Player.Red);

        Debug.Log($"Blue Score: {blueScore}");
        Debug.Log($"Red Score: {redScore}");

        string result;

        if(blueScore > redScore)
        {
            result = "Blue Wins!";
        }
        else if(redScore > blueScore)
        {
            result = "Red Wins!";
        }
        else
        {
            result = "Draw!";
        }
        ShowEndGameClientRpc(result);
    }

    private void UpdateTurnUI()
    {
        if(!IsClient)
        {
            return;
        }
        if(!MatchStarted.Value)
        {
            turnText.text = "Waiting for other player...";
            return;
        }
        if(CurrentPlayer.Value == LocalPlayer)
        {
            turnText.text = $"Your Turn ({CurrentPlayer.Value})";
        }
        else
        {
            turnText.text = "Opponent Turn";
        }
    }

    private void ValidateandPlayMove(ulong sender, string cardID, int row, int col)
    {
        
        if(!clientPlayers.ContainsKey(sender))
        {
            return;
        }

        Player player = clientPlayers[sender];

        if(player != CurrentPlayer.Value)
        {
            Debug.Log(
                "Not your turn"
            );
            return;
        }

        if(!allCards.ContainsKey(cardID))
        {
            return;
        }

        Card card = allCards[cardID];

        if(card.GetOwner() != player)
        {
            Debug.Log(
                "You do not own this card"
            );
            return;
        }

        if(player == Player.Blue)
        {
            if(!blueHand.GetCards().Contains(card))
            {
                return;
            }
        }
        else
        {
            if(!redHand.GetCards().Contains(card))
            {
                return;
            }
        }

        if(row < 0 || row > 2)
        {
            return;
        }

        if(col < 0 || col > 2)
        {
            return;
        }

        if(board.Grid[row,col] != null)
        {
            return;
        }

        bool success = PlayCard(card,row,col);
    }

    public void RegisterCard(Card card)
    {
        allCards[card.GetCardID()] = card;
    }

    [ClientRpc] 
    private void CreateCardClientRpc(string id, int north, int south, int west, int east, int owner)
    {
        Card card = new Card(id, north, south, west, east, (Player)owner);

        Transform parent = (owner == (int)Player.Blue) ? blueHandPosition : redHandPosition;

        GameObject obj = Instantiate(cardPrefab, parent);
        obj.GetComponent<CardUI>().CardSetup(card);
    }
    [ClientRpc]
    private void PlaceCardClientRpc(string cardID, int row, int col)
    {
        CardUI cardUI = CardUI.Find(cardID);

        if(cardUI == null)
        {
            return;
        }

        BoardCellUI cell = BoardCellUI.Find(row,col);

        cell.PlaceCardVisual(cardUI);
    }
    private void SyncCardOwners()
    {
        foreach(Card card in allCards.Values)
        {
            UpdateCardOwnerClientRpc(
                card.GetCardID(),
                (int)card.GetOwner()
            );
        }
    }

    [ClientRpc]
    private void UpdateCardOwnerClientRpc(string cardID, int owner)
    {
        CardUI cardUI = CardUI.Find(cardID);

        if(cardUI == null)
        {
            return;
        }

        cardUI.Card.SetOwner((Player)owner);

        cardUI.RefreshVisual();
    }
    [ClientRpc]
    private void ShowEndGameClientRpc(string result)
    {
        turnText.text = result;
    }
}
