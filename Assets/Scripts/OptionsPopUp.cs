using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPopUp : BasePopUp
{
    [SerializeField] BasePopUp hitPopup;
    [SerializeField] BasePopUp parPopUp;
    // Start is called before the first frame update
    private void Start()
    {
        Time.timeScale = 0;
    }
    public void OnExitGameButton()
    {
        //Debug.Log("exit game");
        Application.Quit();
    }
    public void OnReturnToGameButton()
    {
        // Debug.Log("return to game");
        Time.timeScale = 1f;
        Close();
        hitPopup.Open();
        parPopUp.Open();
    }
}
