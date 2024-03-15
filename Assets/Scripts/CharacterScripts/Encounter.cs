using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
public class Encounter : MonoBehaviour
{

    public GameObject player;

    private void Start()
    {
        transform.position = GameManager.instance.nextHeroPosition;
    }


    void FixedUpdate()
    {
        if(player.GetComponent<ControllableCharacter>().speed > 0)
        {
            GameManager.instance.isWalking = true;
        }
        else
        {
            GameManager.instance.isWalking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnterTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }

        if (other.tag == "LeaveTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }

        if (other.tag == "Region1")
        {
            GameManager.instance.curRegions = 0;
        }

        if (other.tag == "Region2")
        {
            GameManager.instance.curRegions = 1;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canGetEncounter = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Region1" || other.tag == "Region2")
        {
            GameManager.instance.canGetEncounter = false;
        }
    }
}
