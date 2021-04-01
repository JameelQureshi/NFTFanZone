using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateGrid : MonoBehaviour
{


    public GameObject prefab; // This is our prefab object that will be exposed in the inspector
    public GameObject scroll;

    private void Start()
    {
        Populate();
    }

    public void Populate()
    {
        GameObject newObj; // Create GameObject instance

        for (int i = 0; i < GalleryHandler.TotalImageInGallery ; i++)
        {
            // Create new instances of our prefab until we've created as many as we specified
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<GalleryItem>().LoadItem(i);
        }

        float width = scroll.GetComponent<RectTransform>().rect.width;
       
      
        Vector2 newSize = new Vector2((width/3)-10,(width/3)-10) ;
        GetComponent<GridLayoutGroup>().cellSize= newSize;
        
        GalleryHandler.ImagesInGallery  = GalleryHandler.GetTotalImageSaved();
    }

   
}