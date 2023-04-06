using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPopUp : BasePopUp
{
    [SerializeField] BasePopUp hitPopup;
    [SerializeField] BasePopUp parPopUp;
    // Start is called before the first frame update
    public void OnExitGameButton()
    {
        //Debug.Log("exit game");
        Application.Quit();
    }
    public void OnReturnToGameButton()
    {
       // Debug.Log("return to game");
        Close();
        hitPopup.Open();
        parPopUp.Open();
    }
}
