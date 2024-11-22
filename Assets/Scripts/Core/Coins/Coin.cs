using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class Coin : NetworkBehaviour
{
    [SerializeField] private MeshRenderer m_MeshRenderer;
    protected int coinValue  = 10;
    protected bool alreadyCollected;

    public abstract int Collect();

    public void setValue(int value)
    {
        coinValue = value;
    }

    protected void show(bool show)
    {
        m_MeshRenderer.enabled = show;
    }
}
