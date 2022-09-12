using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour,IDragHandler
{
    Player player;
    public void OnDrag(PointerEventData eventData)
    {
        if (player == null)
        {
            player = Player.instance;
        }
        player.Manouvre(eventData.delta.x);
    }


}
