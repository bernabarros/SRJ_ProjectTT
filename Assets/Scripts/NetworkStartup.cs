using System;
using Unity.Netcode;
using UnityEngine;

public class NetworkStartup : MonoBehaviour
{
    bool startAsServer = false;
    bool startAsClient = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        string [] args = Environment.GetCommandLineArgs();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;

        foreach(string arg in args)
        {
            if(arg == "-server")
            {
                startAsServer = true;
            }

            if(arg == "-client")
            {
                startAsClient = true;
            }
        }
        if(startAsServer)
        {
            NetworkManager.Singleton.StartServer();
            Debug.Log("Starting as SERVER");
            Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer}");
            Debug.Log($"IsClient: {NetworkManager.Singleton.IsClient}");
        }
        else if(startAsClient)
        {
            NetworkManager.Singleton.StartClient();
            Debug.Log("Starting as CLIENT");
            Debug.Log($"IsServer: {NetworkManager.Singleton.IsServer}");
            Debug.Log($"IsClient: {NetworkManager.Singleton.IsClient}");
        }
        else
        {
            Debug.LogError("No network mode specified");
        }
    }
    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
    }

    private void OnClientDisconnected(ulong clientId)
    {
        Debug.Log($"Client disconnected: {clientId}");
    }
}
