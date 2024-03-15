using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class CreateCharacter : MonoBehaviour
{
    public List<CharacterProps> CharacterProps = new();
    [SerializeField]
    private Transform avatarPanel;
    [SerializeField]
    private Transform thePoint;
    public GameObject characterModel;
    private GameObject charInstance;
    private GameObject weapInstance;
    private Transform rightHand;
    private Transform wingHolder;
    private GameObject vfx1Instance;
    private GameObject vfx3Instance;
    private int id;
    public float DestroyTime = 3f;
    [SerializeField]
    TextMeshProUGUI charName;
    [SerializeField]
    TextMeshProUGUI charDesc;
    [SerializeField]
    Image[] skillSprites;
    [SerializeField]
    Transform starPanel;
    [SerializeField]
    Image fullStar;
    [SerializeField]
    Image emptyStar;
    [SerializeField]
    Transform schoolButtonHolder;
    private GameObject schoolButtonInstance;
    [SerializeField]
    TextMeshProUGUI charNamePreview;

    public void ShowCharacter(int charID)
    {
        //clear up stars / school button after previous character
        Extensions.DestroyAllChildren(starPanel);
        if (schoolButtonInstance != null)
            Destroy(schoolButtonInstance);
        
        //easy
        id = charID;
        charName.text = CharacterProps[charID].characterName;
        charDesc.text = CharacterProps[charID].characterDescription;
        charNamePreview.text = CharacterProps[charID].characterName;
        //spawn the character model and enable char creation animation
        characterModel = CharacterProps[charID].characterModel;
        charInstance = Instantiate(characterModel, thePoint);
        var animator = charInstance.GetComponent<Animator>();
        animator.runtimeAnimatorController = CharacterProps[charID].characterAnimator;

        //show presentation effects and destroy them after
        var vfx = CharacterProps[charID].characterVFX1;
        vfx1Instance = Instantiate(vfx, thePoint);
        Destroy(vfx1Instance, DestroyTime);

        //instantiate character weapon and enable it's particle effects
        var weapon = CharacterProps[charID].characterWeapon;
        var weapAnimator = weapon.GetComponent<Animator>();
        if (CharacterProps[charID].weaponAnimator != null)
        {
            weapAnimator.runtimeAnimatorController = CharacterProps[charID].weaponAnimator;
        }
        rightHand = charInstance.GetComponent<GameObjectContainer>().objectArray[3].gameObject.transform;
        weapInstance = Instantiate(weapon, rightHand);
        var weapEffects = weapInstance.GetComponent<GameObjectContainer>().objectArray;
        for (int i = 0; i < weapEffects.Length; i++)
        {
            if(weapEffects[i].gameObject != null)
                if (weapEffects[i].gameObject.activeSelf != true)
                    weapEffects[i].gameObject.SetActive(true);
        }
        
        //Instantiate wings
        var wings = CharacterProps[charID].characterWings;
        wingHolder = charInstance.GetComponent<GameObjectContainer>().objectArray[5].gameObject.transform;
        Instantiate(wings, wingHolder);
        
        //instantiate skill icons
        for (int i = 0; i < skillSprites.Length; i++)
        {
            skillSprites[i].sprite = CharacterProps[charID].skillIcons[i];
        }


        //Instantiating stat visualisation stars, full ones for each ability point, empty ones for aesthetics
        for (int i = 0; i < CharacterProps[charID].strengths.Count; i++)
        {
            int full = 5;
            for (int j = 0; j < CharacterProps[charID].strengths[i]; j++)
            {
                Instantiate(fullStar, starPanel);
                full--;
            }
            if (full > 0)
            {
               for (int k = 0; k < full; k++)
               {
                   Instantiate(emptyStar, starPanel);
               }
            }

        }

        //school buttons
        var schoolButton = CharacterProps[charID].schoolButton;
        schoolButtonInstance = Instantiate(schoolButton, schoolButtonHolder);
    }

    public void DestroyInstances()
    {
        Destroy(charInstance);
        if (vfx1Instance != null)
            Destroy(vfx1Instance);
        if (vfx3Instance != null)
            Destroy(vfx3Instance);
    }

    public void PlayAnim()
    {
        if (charInstance != null)
        {
            var vfx3 = CharacterProps[id].characterVFX3;
            vfx3Instance = Instantiate(vfx3, thePoint);
            charInstance.GetComponent<Animator>().Play("rolecreate3");
            Destroy(vfx3Instance, DestroyTime);
        }
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        var HeroManager = HeroDataManager.instance;
        //create a new data file
        HeroManager.CreateSaveFile();
        //load data from this file to gamemanager
        HeroManager.LoadCharData();
        //set according character as main
        string name = CharacterProps[id].characterName;
        int index = HeroManager.CharacterInfo.FindIndex(summon => summon.Name == CharacterProps[id].characterName);
        HeroManager.CharacterInfo[index].isActive = true;
        HeroManager.CharacterInfo[index].isMainCharacter = true;
        //save data
        HeroManager.SaveCharData();
        //run the game
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Village");
    }

    public void MainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }
}
