using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoins = new NetworkVariable<int>();
  

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Coin>(out Coin coin)){return;}
        int coinvalue = coin.Collect();
        if (!IsServer) { return; }
        TotalCoins.Value += coinvalue;
    }

    public void spendCoins(int value)
    {
        TotalCoins.Value -= value;
    }
}
