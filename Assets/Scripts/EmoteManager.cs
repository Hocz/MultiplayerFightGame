using Unity.Netcode;
using UnityEngine;

public class EmoteManager : MonoBehaviour
{
    [SerializeField] private GameObject emotePanel;

    private void Update()
    {        
        ToggleEmotePanel();
    }


    private void ToggleEmotePanel()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Active");
            emotePanel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.T)) 
        {
            Debug.Log("Not Active");
            emotePanel.SetActive(false);
        }
    }
}
