using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPopUp : BasePopUp
{
    [SerializeField] BasePopUp hitPopup;
    [SerializeField] BasePopUp parPopUp;

    [SerializeField] private Image soundOnImage;
    [SerializeField] private Image soundOffImage;
    [SerializeField] private Image backGround;
    //[SerializeField] private AudioClip pauseMusic;
    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Start()
    {
        backGround.gameObject.SetActive(true);
        soundOnImage.gameObject.SetActive(true);
        audioSource = GetComponent<AudioSource>();
        Time.timeScale = 0;
        audioSource.Play();

    }
    public void OnExitGameButton()
    {
        //Debug.Log("exit game");
        Application.Quit();
    }
    public void OnReturnToGameButton()
    {
        backGround.gameObject.SetActive(false);
        soundOnImage.gameObject.SetActive(false);
        soundOffImage.gameObject.SetActive(false);
        audioSource.Pause();
        // Debug.Log("return to game");
        Time.timeScale = 1f;
        Close();
        hitPopup.Open();
        parPopUp.Open();
    }

    public void soundOnButton()
    {
        soundOnImage.gameObject.SetActive(false);
        soundOffImage.gameObject.SetActive(true);
        audioSource.Pause();
    }

    public void soundOffButton()
    {
        soundOffImage.gameObject.SetActive(false);
        soundOnImage.gameObject.SetActive(true);
        audioSource.UnPause();
    }

}
