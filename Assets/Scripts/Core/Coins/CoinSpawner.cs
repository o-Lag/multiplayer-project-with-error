using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CoinSpawner : NetworkBehaviour
{
    [SerializeField] private RespawningCoin Coinprefab;
    [SerializeField] private int maxCoins = 50;
    [SerializeField] private int coinValue = 10;
    [SerializeField] private Vector2 xSpawnRange;
    [SerializeField] private Vector2 ySpawnRange;
    [SerializeField] private Vector2 zSpawnRange;
    [SerializeField] private LayerMask LayerMask;
    private float coinRadius;
    private Collider[] coinBuffer = new Collider[1];
    public override void OnNetworkSpawn()
    {
        if (!IsServer) { return; };
        coinRadius = Coinprefab.GetComponent<SphereCollider>().radius;
        for (int i = 0; i < maxCoins; i++) 
        {
            SpawnCoin();
        }
    }
    private void SpawnCoin()
    {
       RespawningCoin coinInstance = Instantiate(Coinprefab, GetSpawnPoint(), Quaternion.identity);
        coinInstance.setValue(coinValue);
        coinInstance.GetComponent<NetworkObject>().Spawn();
        coinInstance.OnCollected += HandleCoinCollected;
    }

    private void HandleCoinCollected(RespawningCoin coin)
    {
        coin.transform.position = GetSpawnPoint();
        coin.Reset();
    }

    private Vector3 GetSpawnPoint()
    {
        float x = 0;
        float z = 0;

        while (true) {
            x = Random.Range(xSpawnRange.x, xSpawnRange.y);
            z = Random.Range(zSpawnRange.x, zSpawnRange.y);
            Vector3 spawnpoint = new Vector3(x, 1, z);
           int numColliders = Physics.OverlapSphereNonAlloc(spawnpoint, coinRadius, coinBuffer,LayerMask);
            if (numColliders == 0) {

                return spawnpoint;
            }
        }
}
}
