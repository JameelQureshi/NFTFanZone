using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject recordButton;
    public GameObject scroll;

    public void Switch()
    {
        recordButton.SetActive(!recordButton.activeSelf);
        scroll.SetActive(!scroll.activeSelf);
    }

}


