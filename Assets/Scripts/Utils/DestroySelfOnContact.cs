using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelfOnContact : MonoBehaviour
{
 
    private void OnTriggerEnter(Collider other)
    {
       
            Debug.Log("colidiu com: " + other.gameObject.name);
            Destroy(gameObject);
       
        
    }
}
