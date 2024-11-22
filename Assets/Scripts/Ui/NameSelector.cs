using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NameSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button ConectButton;
    [SerializeField] private int MinLenght = 2, MaxLenght = 12;
    [SerializeField] public const string PlayerName = "PlayerName";
    private void Start()
    {
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }
        nameField.text = PlayerPrefs.GetString(PlayerName, string.Empty);
        HandleNameChanged();
    }
    public void HandleNameChanged()
    {
    ConectButton.interactable =  nameField.text.Length >= MinLenght && nameField.text.Length <= MaxLenght;
    
    }
    public void Conect()
    {
        PlayerPrefs.SetString(PlayerName, nameField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
