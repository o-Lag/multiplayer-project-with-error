using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;
using System;
public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerObj playerObj;
    [SerializeField]
    private TMP_Text DisplayNameText;
   private void Start()
    {
        HandlePlayerName(string.Empty,playerObj.PlayerName.Value);
        playerObj.PlayerName.OnValueChanged += HandlePlayerName;
     
    }

    private void HandlePlayerName(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
     DisplayNameText.text = newValue.ToString();
  
    }

    private void OnDestroy()
    {
        playerObj.PlayerName.OnValueChanged -= HandlePlayerName;
    }
}
