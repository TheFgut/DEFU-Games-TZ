using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScr : MonoBehaviour
{
    Vector3 idlePos;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        idlePos = transform.position;
    }

    public void FollowPlayer()
    {
        player = Map.playerObj;
        followCouroutine = StartCoroutine(followPlayer());
    }

    Coroutine followCouroutine;
    IEnumerator followPlayer()
    {
        do
        {
            transform.position = Vector3.Lerp(transform.position,player.transform.position + idlePos,Time.deltaTime);
            yield return new WaitForEndOfFrame();
        } while (true);
    }

    public void Stop()
    {
        if (followCouroutine != null)
        {
            StopCoroutine(followCouroutine);
        }
        transform.position = idlePos;
    }
}
