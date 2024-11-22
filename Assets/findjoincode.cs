using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
public class findjoincode : MonoBehaviour
{
   public TMP_Text texto;
    HostGameManager gameManager;
    ClientGameManager clientGameManager;
    void Start()
    {
        
            gameManager = FindAnyObjectByType<HostSingleton>().GameManager;
            texto.text = gameManager.JoinCode;
    
        //if (IsClient)
        //{
        //    gameManager = FindAnyObjectByType<ClientSingleton>().GameManager.;
        //    texto.text = gameManager.JoinCode;
        //}
    }
     

  
}
