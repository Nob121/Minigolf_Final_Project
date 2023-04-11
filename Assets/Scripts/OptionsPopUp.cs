using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPopUp : BasePopUp
{
    [SerializeField] BasePopUp hitPopup;
    [SerializeField] BasePopUp parPopUp;
    //[SerializeField] private AudioClip pauseMusic;
    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Start()
    {
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
        audioSource.Pause();
        // Debug.Log("return to game");
        Time.timeScale = 1f;
        Close();
        hitPopup.Open();
        parPopUp.Open();
    }
}
