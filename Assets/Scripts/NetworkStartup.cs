using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class NetworkStartup : MonoBehaviour
{
    [SerializeField] private bool startAsServer = false;
    [SerializeField] private bool startAsClient = false;

    [SerializeField] public GameObject joinPanel;
    [SerializeField] public GameObject gamePanel;
    [SerializeField] private GameObject serverPanel;

    [SerializeField] private TMP_Text joinCodeText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        await RelayManager.Instance.InitializationTask;

        if(startAsServer && !startAsClient)
        {
            string joinCode = await RelayManager.Instance.CreateRelay();

            joinCodeText.text = $"Join Code: {joinCode}";

            Debug.Log($"Join Code: {joinCode}");

            NetworkManager.Singleton.StartServer();

            joinPanel.SetActive(false);
            serverPanel.SetActive(true);
            gamePanel.SetActive(true);

            Debug.Log("Starting as SERVER");
            Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer}");
            Debug.Log($"IsClient: {NetworkManager.Singleton.IsClient}");

            return;
        }
        else if(startAsClient)
        {
            joinPanel.SetActive(true);
            serverPanel.SetActive(false);
            gamePanel.SetActive(false);

            Debug.Log("Starting as CLIENT");
            Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer}");
            Debug.Log($"IsClient: {NetworkManager.Singleton.IsClient}");

            return;
        }
        else
        {
            Debug.LogError("No network mode specified");

            return;
        }
    }
}
