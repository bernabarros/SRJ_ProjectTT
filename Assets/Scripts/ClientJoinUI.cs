using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ClientJoinUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] public GameObject joinPanel;
    [SerializeField] public GameObject gamePanel;

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDestroy()
    {
        if(NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    public async void Connect()
    {
        string code = joinCodeInput.text;

        await RelayManager.Instance.JoinRelay(code);

        NetworkManager.Singleton.StartClient();
    }

    private void OnClientConnected(ulong clientId)
    {
        if(clientId != NetworkManager.Singleton.LocalClientId)
        {
            return;
        }

        joinPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
}
