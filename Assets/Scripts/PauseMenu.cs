using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public static bool modeChanged = false;
    public GameObject pauseOverlay;
    public AudioMixer mixer;

    //mixer
    //public GUIText currentMode;
    GameManager manager;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                //Debug.Log("Pause");
                Pause();
            }
            else
            {
                //Debug.Log("Unpause");
                Resume();
            }
        }
    }

    public void Resume()
    {
        pauseOverlay.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    private void Pause()
    {
        pauseOverlay.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game"); 
    }

    public void ChangeGameMode()
    {
        //destroys tanks and respawns in other gamemode. Also resets game stats
        GameManager.gameMode = (GameManager.gameMode + 1) % 2;
        modeChanged = true;
        manager.SwapGameMode();
        Resume();
        //Debug.Log("Changing Gamemode");
    }


    public void SetMasterVolume(Slider volume)
    {
        mixer.SetFloat("Master", volume.value);
    }

    public void SetMusicVolume(Slider volume)
    {
        mixer.SetFloat("Music", volume.value);
    }

    public void SetSFXVolume(Slider volume)
    {
        mixer.SetFloat("SFX", volume.value);
    }

    public void SetTankVolume(Slider volume)
    {
        mixer.SetFloat("Driving", volume.value);
    }
}

