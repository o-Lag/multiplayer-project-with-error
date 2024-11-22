using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class ProjectileLauncher : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GameObject ServerProjectilePrefab;
    [SerializeField] private GameObject ClientProjectilePrefab;
    [SerializeField] private Transform ProjectileSpawnPoint;
    [SerializeField] private GameObject MuzzleFlash;
    [SerializeField] private BoxCollider playerCollider;
    [SerializeField] private CoinWallet Wallet;

    [Header("Settings")]

    [SerializeField] private float projectileSpeed = 30f;
    [SerializeField] private bool shouldFire;
    [SerializeField] private float fireRate = 0.75f;
    [SerializeField] private float MuzzleFlashDuration = 0.075f;
    [SerializeField] private float MuzzleFlashTimer;
    [SerializeField] private int costToFire;
    private float timer;
    
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        _inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) { return; }
        _inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }
    private void Update()
    {
        if(MuzzleFlashTimer > 0f)
        {
            MuzzleFlashTimer -= Time.deltaTime;
            if(MuzzleFlashTimer <= 0f)
            {
                MuzzleFlash.SetActive(false);
            }
        }
        if(!IsOwner) { return; }

        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
      
        if (!shouldFire) { return; }
        if (timer > 0) { return; }
        if(Wallet.TotalCoins.Value < costToFire) {return; }

        PrimaryServerRPC(ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.forward);
        spawnDummyProjectile(ProjectileSpawnPoint.transform.position, ProjectileSpawnPoint.forward);
        timer = 1 / fireRate;
    }
    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }
    [ServerRpc]
    private void PrimaryServerRPC(Vector3 SpawnPos, Vector3 direction)
    {
        if (Wallet.TotalCoins.Value < costToFire) { return; }
        Wallet.spendCoins(costToFire);

        GameObject projectileInstance = Instantiate(ServerProjectilePrefab, SpawnPos, Quaternion.identity);
        projectileInstance.transform.forward = direction;
        Physics.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider>());

        if(projectileInstance.TryGetComponent<DealDamageOnContact>(out DealDamageOnContact dealDamage))
        {
            dealDamage.setOwner(OwnerClientId);
        }

        if (projectileInstance.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = rb.transform.forward * projectileSpeed;
        }
        spawnDummyProjectileClientRpc(SpawnPos, direction);

  
    }
    [ClientRpc]
    private void spawnDummyProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {
        if(IsOwner) { return;}
        spawnDummyProjectile(spawnPos, direction);
    }
    private void spawnDummyProjectile(Vector3 spawnPos, Vector3 direction)
    {
        MuzzleFlash.SetActive(true);
        MuzzleFlashTimer = MuzzleFlashDuration;
       GameObject projectileInstance = Instantiate(ClientProjectilePrefab, spawnPos, Quaternion.identity);

        projectileInstance.transform.forward = direction;
        Physics.IgnoreCollision(playerCollider, projectileInstance.GetComponent<SphereCollider>());
        if(projectileInstance.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = rb.transform.forward * projectileSpeed;
        }
    }
}
