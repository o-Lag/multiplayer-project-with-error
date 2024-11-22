using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [Header("References")]

    [SerializeField] private InputReader inputreader;
    [SerializeField] private Transform bodytransform;
    [SerializeField] private Rigidbody rb;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float turningRate = 270;
    [SerializeField] private Vector2 previousMovimentInput;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            return;
        }
       inputreader.MoveEvent += HandleMovment;
      
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner)
        {
            return;
        }
        inputreader.MoveEvent -= HandleMovment;
    }
    private void Update()
    {
        if (!IsOwner) { return; }
        float zRotation = previousMovimentInput.x * turningRate * Time.deltaTime;
        bodytransform.Rotate(0f, zRotation, 0f);
    }
    private void FixedUpdate()
    {
        if (!IsOwner) {return; }
        rb.velocity = (Vector3)bodytransform.forward * previousMovimentInput.y  * moveSpeed;
    }
    public void HandleMovment(Vector2 movmentInput)
    {
        previousMovimentInput = movmentInput;
    }
}
