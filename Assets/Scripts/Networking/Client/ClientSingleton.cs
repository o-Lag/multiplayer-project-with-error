using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientSingleton : MonoBehaviour
{
    // Start is called before the first frame update
    private static ClientSingleton instance;
    private const string MenuSceneName = "Menu";
    public ClientGameManager GameManager {get; private set; }
    public static ClientSingleton Instance
    { 
      get{
                if (instance != null) { return instance; }

                instance = FindObjectOfType<ClientSingleton>();

                if (instance == null)
                {
                    Debug.LogError("No HostSingleton in the scene!");
                    return null;
                }

                return instance;
            }

        
    
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

  public async Task<bool> CreateClient()
    {
      GameManager =  new ClientGameManager();
      return await GameManager.InitAsync();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
    private void OnDestroy() { 
    
    GameManager?.Dispose();
    }
}
