using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorChangerZone : MonoBehaviour
{
    GameColor color;
    public ParticleSystem particles;

    public void Start()
    {
        color = Map.instance.ColorsModule.GetRandom();
        ParticleSystem.MainModule module = particles.main;
        module.startColor = color.color;

    }

    public GameColor GetColor()
    {
        return color;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player playerScr = other.GetComponent<Player>();
            playerScr.playerColor.SetColor(color);
        }
    }
}
