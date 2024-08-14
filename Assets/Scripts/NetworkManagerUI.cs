using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject networkButtons;

    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [SerializeField] private GameObject viewControls;

    [SerializeField] private GameObject controlsPanel;

    private void Awake()
    {
        serverButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartServer();
            networkButtons.SetActive(false);
            viewControls.SetActive(true);
        });
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            networkButtons.SetActive(false);
            viewControls.SetActive(true);
        });
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            networkButtons.SetActive(false);
            viewControls.SetActive(true);
        });

        controlsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            controlsPanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        { 
            controlsPanel.SetActive(false);
        }
    }
}
