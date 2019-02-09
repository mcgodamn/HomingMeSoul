using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AngerStudio.HomingMeSoul.Core;

public class CCardController : MonoBehaviour
{
    public Image cardImage, characterThumb, wanted;

    public TMPro.TextMeshProUGUI keyCode;

    public void Set (KeyCode k)
    {
        keyCode.text = System.Enum.GetName(typeof(KeyCode), k);
        cardImage.sprite = AppCore.Instance.resourceConfig.playerSelctionCards[AppCore.Instance.activePlayers[k].UsingPlayerSlot];
        wanted.sprite = AppCore.Instance.config.usablePickupSprites[AppCore.Instance.activePlayers[k].assginedPickupType];

        int i = 0;
        switch (AppCore.Instance.activePlayers[k].assginedPickupType)
        {
            case 0: case 1: case 2:
                i = 0;
                break;
            case 3: case 4: case 5:
                i = 1;
                break;
            case 6: case 7: case 8:
                i = 2;
                break;
            default:
                break;
        }
        characterThumb.sprite = AppCore.Instance.resourceConfig.characterThumbnails[i];
        // characterThumb.sprite = AppCore.Instance.resourceConfig.characterThumbnails[Random.Range(0, AppCore.Instance.resourceConfig.characterThumbnails.Length)];
    }
    
}
