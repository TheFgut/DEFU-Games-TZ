using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GameObject potionInstance;
    public static Map instance;
    public GameObject playerInstance;

    private void Awake()
    {
        //initialization
        instance = this;
        ColorsModule.Init();
        RandomMapModule.Init();
    }

    GameObject[] mapObjects;

    public void ClearMap()
    {
        if (playerObj != null)
        {
            Destroy(playerObj);
        }
        if (mapObjects != null)
        {
            for (int i = 0; i < mapObjects.Length; i++)
            {
                Destroy(mapObjects[i]);
            }
        }
    }
    public static GameObject playerObj;
    public void GenMap(int elementsCount)
    {

        //destroying old
        ClearMap();
        //generating new
        playerObj = Instantiate(playerInstance);
        playerObj.transform.position = new Vector3(0, 1, 0);

        int counter = elementsCount;
        Vector3 offset = transform.position;
        List<GameObject> GeneratedObjects = new List<GameObject>();
        int c = 0;
        do
        {
            c++;
            Vector3 connectorPos;
            GameObject element = null;
            if (c == 3)
            {
                c = 0;
                element = Instantiate(RandomMapModule.colorChangerModules);
                connectorPos = element.GetComponent<mapModule>().connector.position;
            }
            else
            {
                element = Instantiate(RandomMapModule.GetRandom(out connectorPos));
            }
            element.transform.position = offset;
            element.transform.parent = transform;
            offset += connectorPos;
            GeneratedObjects.Add(element);
            counter--;
        }while(counter > 0);
        mapObjects = GeneratedObjects.ToArray();
    }


    [SerializeField]
    private RandomMapModuleGen RandomMapModule = new RandomMapModuleGen();
    [System.Serializable]
    class RandomMapModuleGen
    {
        public GameObject[] mapModulesObjects;
        mapModule[] modulesScrs;
        float maxChance;
        public GameObject colorChangerModules;
        mapModule ColorChngModuleScr;

        public void Init()
        {
            modulesScrs = new mapModule[mapModulesObjects.Length];
            for (int i = 0; i < mapModulesObjects.Length; i++)
            {
                modulesScrs[i] = mapModulesObjects[i].GetComponent<mapModule>();
                maxChance += modulesScrs[i].appearChance;
            }
            ColorChngModuleScr = colorChangerModules.GetComponent<mapModule>();
        }

        float prevMin = float.MaxValue;
        float prevMax;
        public GameObject GetRandom(out Vector3 targetPos)
        {
            float RandomNum;
            do
            {
                RandomNum = Random.Range(0, maxChance);
            } while (RandomNum > prevMin && RandomNum <= prevMax);

            float num = 0;
            for (int i = 0; i < mapModulesObjects.Length;i++)
            {
                float newMin = num;
                num += modulesScrs[i].appearChance;
                if (RandomNum <= num)
                {
                    prevMin = newMin;
                    prevMax = num;
                    targetPos = modulesScrs[i].connector.position;
                    return mapModulesObjects[i];
                }

            }
            targetPos = new Vector3();
            return null;
        }
    }

    public ColorsModuleClass ColorsModule = new ColorsModuleClass();
    [System.Serializable]
    public class ColorsModuleClass
    {
        public GameColor[] awailableColors;

        public void Init()
        {
            for (int i = 0; i < awailableColors.Length;i++)
            {
                awailableColors[i].ID = i;
            }
        }

        public GameColor GetRandom()
        {
            return awailableColors[Random.Range(0, awailableColors.Length)];
        }
    }
}

[System.Serializable]
public class GameColor
{
    public Color color;
    internal int ID;
}
