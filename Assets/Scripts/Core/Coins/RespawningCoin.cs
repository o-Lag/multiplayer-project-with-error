using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningCoin : Coin
{
    public event Action<RespawningCoin> OnCollected;
    private Vector3 previousePos;
    private void Update()
    {
       if(previousePos != transform.position)
        {
            show(true);
        } 
       previousePos = transform.position;
    }
    public override int Collect()
    {
        if (!IsServer) { show(false); return 0; }
        if (alreadyCollected == true) { return 0; }
        alreadyCollected = true;
        OnCollected?.Invoke(this);
        return coinValue; 
    }
    public void Reset()
    {
        alreadyCollected = false;
    }
}
