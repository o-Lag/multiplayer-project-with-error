using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DealDamageOnContact : MonoBehaviour
{
    [SerializeField] private int damage = 5;
    private ulong OwnerClientID;
    public void setOwner(ulong ownerClientID)
    {
        this.OwnerClientID = ownerClientID;
    }

    private void OnTriggerEnter(Collider col)
    {
        
        if (col.attachedRigidbody == null) { return; }
        if (col.attachedRigidbody.TryGetComponent<NetworkObject>(out NetworkObject netObj))
        {
            if (OwnerClientID == netObj.OwnerClientId){return;}
        
        }
        if (col.attachedRigidbody.TryGetComponent<Health>(out Health health))
        {
        health.TakeDamage(damage);
        }
        
    }
}

