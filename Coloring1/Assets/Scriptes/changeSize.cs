using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeSize : MonoBehaviour
{
    public Texture2D pureImage;
    public Image img;
    void Start()
    {
        print(1f* Camera.main.pixelWidth/GetComponent<SpriteRenderer>().sprite.texture.width);
        transform.localScale =  new Vector3(1f*GetComponent<SpriteRenderer>().sprite.texture.width/ Camera.main.pixelHeight, 1f * GetComponent<SpriteRenderer>().sprite.texture.height/Camera.main.pixelWidth, 1);
        //gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(pureImage, img.GetComponent<RectTransform>().rect, Vector2.zero);

    }

}
