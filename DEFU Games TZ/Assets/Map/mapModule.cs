using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapModule : MonoBehaviour
{
    public Transform connector;
    public float appearChance;

    public Transform[] zeroPotions;
    public Transform[] onePotions;
    public Transform[] TwoPotions;

    public void Start()
    {

        List<GameColor> colors = new List<GameColor>(Map.instance.ColorsModule.awailableColors);
        int num = Random.Range(0, colors.Count);
        GenPotions(zeroPotions, colors[num]);
        colors.RemoveAt(num);
        num = Random.Range(0, colors.Count);
        GenPotions(onePotions, colors[num]);
        colors.RemoveAt(num);
        GenPotions(TwoPotions, colors[0]);

    }

    public void GenPotions(Transform[] positions,GameColor color)
    {
        GameObject potion = Map.instance.potionInstance;
        for (int i = 0; i < positions.Length;i++)
        {
            GameObject gened = Instantiate(potion);
            gened.GetComponent<Potion>().Init(color);
            gened.transform.position = positions[i].position;
            gened.transform.parent = transform;
        }
    }
}
