using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;

    public Task InitializationTask { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        InitializationTask = InitializeServices();
    }

    private async Task InitializeServices()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();

        Debug.Log($"Authenticated: {AuthenticationService.Instance.PlayerId}");
    }

    public async Task<string> CreateRelay()
    {
        Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

        string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "dtls"));

        Debug.Log($"Join Code: {joinCode}");

        return joinCode;
    }

    public async Task JoinRelay(string joinCode)
    {
        JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();

        transport.SetRelayServerData(AllocationUtils.ToRelayServerData(allocation,"dtls"));
    }
}
