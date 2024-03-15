using UnityEngine;
using UnityEngine.SceneManagement;

public class ForceBattle : MonoBehaviour
{
    public int region;
    public GameObject button;
    public Transform buttonPanel;
    //public GameManager gManager;

    void Start()
    {
        //gManager = GameManager.instance;
        if (buttonPanel != null)
        for (int i = 0; i < GameManager.instance.Regions.Count; i++)
        {
            GameObject Btn = Instantiate(button);
            Btn.GetComponent<RegionIDButton>().buttonText.text = "Region " + (i+1);
            Btn.GetComponent<RegionIDButton>().region = i;
            Btn.GetComponent<RegionIDButton>().forceBattle = this;
            Btn.transform.SetParent(buttonPanel, false);
        }

    }

    public void EnterDungeon()
    {
        SceneManager.LoadScene("Dungeon1");
    }

    public void ExitDungeon()
    {
        SceneManager.LoadScene("Village");
    }
}
