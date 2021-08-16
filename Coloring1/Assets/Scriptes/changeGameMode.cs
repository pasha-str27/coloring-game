using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeGameMode : MonoBehaviour
{
    public Image[] placesForSprites;
    public Sprite[] icons;
    public Sprite[] iconsForRightMenu;
    Color[] colors;
    public Sprite knob;

    private void Start()
    {
        colors = new Color[placesForSprites.Length];
        for (int i = 0; i < colors.Length; ++i)
            colors[i] = placesForSprites[i].color;
    }

    public void loadPictures()
    {
        print(detectClicks.letColoring);
        if (!detectClicks.letColoring)
            return;
        if(tag!="fill")
        {
            tag = "fill";
            detectClicks.showImage = true;
            GetComponent<Image>().sprite = icons[1];
            GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 0);
            GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 4f);
            for (int i = 0; i < placesForSprites.Length; ++i)
            {
                placesForSprites[i].color = Color.white;
                placesForSprites[i].GetComponent<Outline>().effectDistance = Vector2.zero;
                placesForSprites[i].sprite = iconsForRightMenu[i];
                placesForSprites[i].gameObject.tag = "image";
            }
            return;
        }
        detectClicks.letColoring = true;
        detectClicks.showImage = false;
        detectClicks.color = Color.white;
        tag = "palochka";
        GetComponent<Image>().sprite = icons[0];
        GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -12);
        GetComponent<RectTransform>().localScale = new Vector3(0.7f, 0.7f, 1f);
        for (int i = 0; i < placesForSprites.Length; ++i)
        {
            placesForSprites[i].color = colors[i];
            placesForSprites[i].sprite = knob;
            placesForSprites[i].gameObject.tag = "color";
        }

    }
}
