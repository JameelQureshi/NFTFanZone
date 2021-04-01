using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class GalleryItem : MonoBehaviour
{

    public  void LoadItem(int index)
    {
        //gameObject.transform.GetChild(0).GetComponent<Image>().sprite = LoadSprite(GalleryHandler.GetImagePath(""+index));
        //gameObject.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;


        Button button = gameObject.GetComponent<Button>();
        button.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = LoadSprite(Path.Combine(Application.persistentDataPath, "NFT_" + index + ".png"));

        button.onClick.AddListener(() => OpenImage(button.transform.GetChild(0).GetComponent<Image>().sprite));
    }

    public void OpenImage(Sprite sprite)
    {
        // Open Image In fullSize
        GalleryHandler.instance.OpenFullImage(sprite);
    }
    public static Sprite LoadSprite(string path)
    {
        if (string.IsNullOrEmpty(path)) return null;
        if (System.IO.File.Exists(path))
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        return null;
    }

}
