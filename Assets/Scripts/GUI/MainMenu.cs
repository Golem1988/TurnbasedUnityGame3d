using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button EnterWorldButton;
    [SerializeField] TextMeshProUGUI saveFileReport;
    [SerializeField] GameObject messageObj;
    private void Start()
    {
        var scene = SceneManager.GetActiveScene();
        if (scene.name == "MainScreen")
        {
            bool canContinue = ReadFromFile();
            if (canContinue)
                EnterWorldButton.interactable = true;
            else
            {
                EnterWorldButton.interactable = false;
                saveFileReport.text = "Save file could not be found. Please start new game to enter the game.";
                saveFileReport.color = Color.red;
            }
            messageObj.SetActive(true);
        }
    }

    bool ReadFromFile()
    {
        string filePath = Application.persistentDataPath + "/CharData.json";
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("CharacterCreation");
    }

    public void EnterWorld()
    {
        SceneManager.LoadScene("Village");
    }

    public void MainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }
}
