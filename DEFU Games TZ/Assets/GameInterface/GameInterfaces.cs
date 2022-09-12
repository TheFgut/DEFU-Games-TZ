using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterfaces : MonoBehaviour
{
    public static GameInterfaces instance;
    public CameraScr cam;

    public Canvas gameInterface;
    public Canvas menuInterface;
    public Canvas deathInterface;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        GoToMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        gameInterface.enabled = false;
        menuInterface.enabled = false;
        deathInterface.enabled = true;
    }
    public void GoToMainMenu()
    {
        gameInterface.enabled = false;
        menuInterface.enabled = true;
        deathInterface.enabled = false;
        cam.Stop();
        Map.instance.ClearMap();

    }
    public void StartGame()
    {
        gameInterface.enabled = true;
        menuInterface.enabled = false;
        deathInterface.enabled = false;
        Map.instance.GenMap(20);
        cam.FollowPlayer();
    }
}
