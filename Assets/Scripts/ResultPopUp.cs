using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    public void Open()
    {
        this.gameObject.SetActive(true);
    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
