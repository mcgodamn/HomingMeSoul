using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngerStudio.HomingMeSoul.Core;
using BA_Studio.UnityLib.GameObjectPool;

public class CSPanelController : MonoBehaviour
{
    public GameObject row1,row2;

    public GameObject cardPrefab;

    public List<CCardController> activeCards;

    Dictionary<KeyCode, CCardController> cardMap;

    public GameObjectPool<CCardController> cardPool;

    AudioSource audio;

    void Awake ()
    {
        cardPool = new GameObjectPool<CCardController>(cardPrefab);
        cardMap = new Dictionary<KeyCode, CCardController>();
        this.audio = this.gameObject.AddComponent<AudioSource>();
    }

    public void AddCard (KeyCode control)
    {
        if (activeCards.Count >= 8) return;
        CCardController g = cardPool.GetObjectFromPool(this.transform);
        if (row1.transform.childCount < 4) g.transform.SetParent(row1.transform);
        else g.transform.SetParent(row2.transform);

        g.transform.localPosition = Vector3.zero;

        g.Set(control);
        activeCards.Add(g);
        cardMap.Add(control, g);
        this.audio.PlayOneShot(AppCore.Instance.activePlayers[control].assignedActionAudio);
        
    }

    public void RemoveCard (KeyCode control)
    {
        if (!cardMap.ContainsKey(control)) return;
        activeCards.Remove(cardMap[control]);
        cardPool.ReturnToPool(cardMap[control]);
        cardMap.Remove(control);
    }
}
