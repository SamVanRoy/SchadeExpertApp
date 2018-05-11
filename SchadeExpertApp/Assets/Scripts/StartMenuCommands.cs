using HoloToolkit.UI.Keyboard;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuCommands : MonoBehaviour {
    public GameObject speechManager;
    public GameObject photoManager;
    public GameObject videoManager;
    public GameObject mainMenu;
    public GameObject newProjectInformationScreen;

    public GameObject projectListScreen;

    private GameObject startMenu;

    public void OpenProjectlistScreen(bool isStartmenuActive)
    {
        startMenu = GameObject.Find("StartMenu");
        startMenu.SetActive(isStartmenuActive);
        projectListScreen.SetActive(!isStartmenuActive);
    }

    public void OpenNewProjectInfoScreen(bool isStartmenuActive)
    {
        startMenu = GameObject.Find("StartMenu");
        startMenu.SetActive(isStartmenuActive);

        newProjectInformationScreen.SetActive(!isStartmenuActive);
    }

    public void InitWorldNewProject(bool isStartmenuActive)
    {
        //startMenu = GameObject.Find("StartMenu");
        startMenu.SetActive(isStartmenuActive);
        newProjectInformationScreen.SetActive(isStartmenuActive);
        //speechManager.SetActive(!isStartmenuActive);
        photoManager.SetActive(!isStartmenuActive);
        videoManager.SetActive(!isStartmenuActive);
        mainMenu.SetActive(!isStartmenuActive);
    }

    public void GoBackToStartMenu()
    {
        startMenu.SetActive(true);
        photoManager.SetActive(false);
        videoManager.SetActive(false);
        mainMenu.SetActive(false);
        newProjectInformationScreen.SetActive(false);
        projectListScreen.SetActive(false);
    }

    public void ClearTextInputfield(KeyboardInputField inputfield)
    {
        inputfield.text = "";
    }
}
