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
        characterThumb.sprite = AppCore.Instance.resourceConfig.characterThumbnails[Random.Range(0, AppCore.Instance.resourceConfig.characterThumbnails.Length)];
    }
    
}
