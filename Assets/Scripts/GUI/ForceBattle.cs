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
        GameManager.instance.lastHeroPosition = new Vector3(0f, 0.5f, 0f);
        GameManager.instance.nextHeroPosition = GameManager.instance.lastHeroPosition;
        SceneManager.LoadScene("DungeonNew");
    }

    public void ExitDungeon()
    {
        SceneManager.LoadScene("Village");
    }
}
