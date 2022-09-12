using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    GameColor potionColor;
    public SpriteRenderer potionContent;
    // Start is called before the first frame update
    public void Init(GameColor color)
    {
        potionColor = color;
        potionContent.color = potionColor.color;
    }
    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject);
            Player.instance.GotPotion(potionColor);
        }
    }
}
