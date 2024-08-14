using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class EmoteManager : NetworkBehaviour
{
    [SerializeField] private SpriteRenderer icon;

    [SerializeField] private Sprite laughEmote;
    [SerializeField] private Sprite sadEmote;
    [SerializeField] private Sprite angryEmote;
    [SerializeField] private Sprite deadEmote;
    [SerializeField] private Sprite poopEmote;

    private bool isEmoting = false;

    private void Awake()
    {
        icon.sprite = null;
    }

    public void SetIcon(int type)
    {
        if (!isEmoting)
        {
            isEmoting = true;

            switch (type)
            {
                case 0:
                    icon.sprite = laughEmote;
                    StartCoroutine(ResetIcon());
                    break;
                case 1:
                    icon.sprite = sadEmote;
                    StartCoroutine(ResetIcon());
                    break;
                case 2:
                    icon.sprite = angryEmote;
                    StartCoroutine(ResetIcon());
                    break;
                case 3:
                    icon.sprite = deadEmote;
                    StartCoroutine(ResetIcon());
                    break;
                case 4:
                    icon.sprite = poopEmote;
                    StartCoroutine(ResetIcon());
                    break;

                default:
                    icon.sprite = null;
                    break;
            }
        }
        
    }

    private IEnumerator ResetIcon()
    {
        yield return new WaitForSeconds(2f);
        icon.sprite = null;
        isEmoting = false;
    }


}
