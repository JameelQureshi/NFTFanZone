using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadCustom : MonoBehaviour
{
    static bool isLoaded;

    void Awake() 
    {
        print("Back Button isLoaded=" + isLoaded);

        if (!isLoaded)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);  
        }
 
        isLoaded = true;
     }
}
