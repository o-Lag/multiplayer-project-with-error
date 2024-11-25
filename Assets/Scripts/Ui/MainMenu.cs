using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeField;
   public async void StartHost()
    {
        await HostSingleton.Instance.GameManager.StartHostAsync();
    }
    public async void StarClient()
    {
        await ClientSingleton.Instance.GameManager.StartClientAsync(joinCodeField.text);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
