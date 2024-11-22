using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private InputReader inputReader;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private Vector2 previousAimPos;

    private void Update()
    {
        if (!IsOwner){return;}

        Vector3 AimWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(inputReader.AimPosition.x, inputReader.AimPosition.y, Camera.main.transform.position.y));

        // Calcular a direção entre o mouse e a torreta, no plano XZ (horizontal)
        Vector3 direction = new Vector3(AimWorldPos.x - turretTransform.position.x, 0, AimWorldPos.z - turretTransform.position.z);

        // Definir o forward da torreta para a direção calculada
        turretTransform.forward = direction.normalized;
    }
}
