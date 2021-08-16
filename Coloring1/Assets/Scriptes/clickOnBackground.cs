using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class clickOnBackground : MonoBehaviour, IPointerClickHandler
{
    public GameObject[] ballons;
    public Texture2D pureImage;//пустой спрайт
    public GameObject particleEffect;
    private void Start()
    {
        gameObject.GetComponent<Image>().sprite = Sprite.Create(pureImage, new Rect(0.0f, 0.0f, pureImage.width, pureImage.height), Vector2.zero);
    }
    public void OnPointerClick(PointerEventData eventData)//при клике на задний фон
    {
        if (!detectClicks.letColoring)
            return;
        if (detectClicks.letColoring)
            Destroy(Instantiate(particleEffect, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity), 3);
        GetComponent<AudioSource>().Play();//проигриваем звуковой ефект

        if (detectClicks.showImage)
        {
            detectClicks.instantiatePicture();
            return;
        }

        //if (detectClicks.isPainting)
        //{
        //    //узнаем координати пикселя на которий нажали
        //    Vector2 localCursor;
        //    RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localCursor);
        //    Rect r = RectTransformUtility.PixelAdjustRect(GetComponent<RectTransform>(), GetComponent<Canvas>());
        //    Vector2 ll = new Vector2(localCursor.x - r.x, localCursor.y - r.y);
        //    int x, y;
        //    x = (int)(ll.x / r.width * gameObject.GetComponent<Image>().sprite.texture.width);
        //    y = (int)(ll.y / r.height * gameObject.GetComponent<Image>().sprite.texture.width);
        //    for (int i = x - 5; i < x + 5; ++i)
        //        for (int j = y - 5; j < y + 5; ++j)
        //            gameObject.GetComponent<Image>().sprite.texture.SetPixel(i, j, Color.red);
        //    gameObject.GetComponent<Image>().sprite.texture.Apply();
        //    return;
        //}
        particleEffect.GetComponent<ParticleSystem>().startColor = detectClicks.color;
        if (detectClicks.letColoring)
        {
            GetComponent<Image>().color = detectClicks.color;//меняем цвет заднего фона
            GetComponent<Image>().sprite.texture.Apply();//обновляекм картинку
            detectClicks.wasClick = true;//разрешаем сохранение изменений
            int count = 0;
            foreach (Color pixel in transform.parent.GetChild(1).GetComponent<detectClicks>().imgOnGame.sprite.texture.GetPixels())
            {
                if (pixel == Color.white)
                    ++count;
                if (count > 200)
                    break;
            }
            if (count <= 200 && GetComponent<Image>().color != Color.white&& detectClicks.wasClear)
            {
                detectClicks.letShowPicture = false;
                detectClicks.letColoring = false;

                detectClicks.wasClear = false;
                StartCoroutine(wait());
                instantiateBallons();
            }
            return;
        }

    }

    void instantiateBallons()
    {
        for (int i = 0; i < 25; ++i)
            Destroy(Instantiate(ballons[UnityEngine.Random.Range(0, ballons.Length)],
                new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-15f, 2f)),
                Quaternion.identity),8);

    }

    IEnumerator wait()
    {
        detectClicks.exit = true;
        yield return new WaitForSeconds(8f);

        detectClicks.letColoring = true;
    }
}
