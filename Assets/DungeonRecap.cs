using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonRecap : MonoBehaviour
{
    [SerializeField]
    PlayerControls player;
    public void LeaveDungeon()
    {
        player.location = new Vector3Int(0, 0, 0);
        player.transform.position = new Vector3(0f, 0.5f, 0f);
        player.dest = player.transform.position;
        GameManager.instance.nextHeroPosition = player.transform.position;
        GameManager.instance.lastHeroPosition = GameManager.instance.nextHeroPosition;
        DungeonManager.instance.DeleteDungeonProgressFile();
        SceneManager.LoadScene("Village");
    }
}
