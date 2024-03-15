using UnityEngine;

public class UIInput : MonoBehaviour
{
	[SerializeField] GameObject forceBattleGameObject;
	[SerializeField] GameObject partyManagerGameObject;
	[SerializeField] GameObject infoPanelGameObject;
	[SerializeField] GameObject summonInfoPanelGameObject;
	[SerializeField] GameObject inGameMenuGameObject;
    [SerializeField] KeyCode[] toggleForceBattleKeys;
	[SerializeField] KeyCode[] togglePartyManagerKeys;
    [SerializeField] KeyCode[] toggleHeroInfoKeys;
	[SerializeField] KeyCode[] toggleSummonInfoKeys;
	[SerializeField] KeyCode[] toggleMainMenuKeys;


    private void Start()
    {
		GameManager.instance.Chat.AddToChatOutput("Press " + toggleHeroInfoKeys[0] + " to open the Character panel;");
		GameManager.instance.Chat.AddToChatOutput("Press " + toggleSummonInfoKeys[0] + " to open the Summon information panel.");
		GameManager.instance.Chat.AddToChatOutput("Press " + toggleForceBattleKeys[0] + " to open the region attack menu.");
		GameManager.instance.Chat.AddToChatOutput("Press " + togglePartyManagerKeys[0] + " to open the party manager panel.");
		GameManager.instance.Chat.AddToChatOutput("Press " + toggleMainMenuKeys[0] + " to open the main menu.");
	}

    void Update()
	{
		if (Input.GetKeyDown(toggleForceBattleKeys[0]))
			ToggleAttackMenu();
		if (Input.GetKeyDown(togglePartyManagerKeys[0]))
			TogglePartyManagerMenu();
		if (Input.GetKeyDown(toggleHeroInfoKeys[0]))
			ToggleHeroInfo();
		if (Input.GetKeyDown(toggleMainMenuKeys[0]))
			ToggleInGameMenu();
		if (Input.GetKeyDown(toggleSummonInfoKeys[0]))
			ToggleSummonInfo();
	}

	public void ToggleAttackMenu()
	{
		forceBattleGameObject.SetActive(!forceBattleGameObject.activeSelf);
	}

	public void TogglePartyManagerMenu()
	{
		partyManagerGameObject.SetActive(!partyManagerGameObject.activeSelf);
	}

	public void ToggleHeroInfo()
	{
		infoPanelGameObject.SetActive(!infoPanelGameObject.activeSelf);
	}

	public void ToggleSummonInfo()
	{
		summonInfoPanelGameObject.SetActive(!summonInfoPanelGameObject.activeSelf);
	}

	public void ToggleInGameMenu()
	{
		inGameMenuGameObject.SetActive(!inGameMenuGameObject.activeSelf);
	}

}
