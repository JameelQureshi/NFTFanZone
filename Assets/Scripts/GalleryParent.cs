using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryParent : MonoBehaviour
{

    public static GameObject GalleryCanvas;

    public void Start()
    {
        GalleryCanvas = gameObject;
    }
}
