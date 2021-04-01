using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GalleryHandler : MonoBehaviour 
{

	public static GalleryHandler instance;


    private void Awake()
    {
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}




	public static int TotalImageInGallery
    {
        set
        {
			PlayerPrefs.SetInt("TotalImageInGallery",value);
		}

        get
        {
			return PlayerPrefs.GetInt("TotalImageInGallery");
        }
    }





	public GameObject fullImageScreen;
    private void Start()
    {
		fullImageScreen.SetActive(false);
	}


    public void OpenFullImage(Sprite sprite)
    {
		fullImageScreen.SetActive(true);
		Image image = fullImageScreen.transform.GetChild(0).GetComponent<Image>();
		image.sprite = sprite;
		image.preserveAspect = true;
	}

    public static int ImagesInGallery = 0;

	



	public static void SetImagePath(string pathToImage , string ProjectID , string ButtonLink)
	{
		PlayerPrefs.SetString("pathToImage"+GetTotalImageSaved(), pathToImage);
		PlayerPrefs.SetString("ProjectID"+GetTotalImageSaved(), ProjectID);
		PlayerPrefs.SetString("ButtonLink"+GetTotalImageSaved(), ButtonLink);

		SetTotalImageSaved();
		print(GetTotalImageSaved());
	}
	public static string GetCurrentImagePath(string projectid)
	{
		for(int i=0 ; i<GetTotalImageSaved(); i++)
		{
			if(PlayerPrefs.GetString("ProjectID"+i) == projectid)
			{

				return PlayerPrefs.GetString("pathToImage"+i);
			}
		}
		return null;
	}

	public static string GetImagePath(string index)
	{
		return PlayerPrefs.GetString("pathToImage"+index);
	}
	public static string GetImageLink(int index)
	{
		return PlayerPrefs.GetString("ButtonLink"+index);
	}
	public static void SetTotalImageSaved()
	{
		PlayerPrefs.SetInt("ToatlImagesSaved",PlayerPrefs.GetInt("ToatlImagesSaved") +1 );
	} 
	public static int GetTotalImageSaved()
	{
		return PlayerPrefs.GetInt("ToatlImagesSaved");
	}

	public static bool IsAlreadyAvailable(string projectID)
	{
		for(int i=0 ; i<GetTotalImageSaved(); i++)
		{
			if(PlayerPrefs.GetString("ProjectID"+i) == projectID)
			{
				return true;
			}
		}
		return false;
	}

}
