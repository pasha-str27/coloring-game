using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class loadImg : MonoBehaviour, IPointerClickHandler
{
    //public Image img;//силка на изображение на которм происходит раскраска
    // public GameObject secondScene;//силка на следующую канву
    public static Texture2D pureImage;//чистое изображение
    public Texture2D pureImageForDrawing;
    //public static bool reload;//нужно ли перегзагружать картинки
    //public static string nameImage;//название картинки, которую нужно перезагрузить
    public int number;

    private void Start()//при старте
    {
        if (gameObject.name == "paint")
        {

            gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("drawing/" + "paint_" + number);//иначе загружаем новий спрайт, тоесть чистую раскраску

            if (PlayerPrefs.HasKey("paint"+number.ToString()))
            {
                gameObject.GetComponent<Image>().sprite.texture.LoadImage(System.Convert.FromBase64String(PlayerPrefs.GetString("paint" + number.ToString())));//то загружаем ее
            }

            return;
        }
        else
        {
            if (pureImage == null)
                pureImage = Resources.Load<Sprite>("purebackground").texture;
            if (gameObject.name == "background")//если скрипт лежит на заднем фоне то
            {
                //то создаем для задего фона новий спрайт
                gameObject.GetComponent<Image>().sprite = Sprite.Create(pureImage, new Rect(0.0f, 0.0f, pureImage.width, pureImage.height), Vector2.zero);
                string f = gameObject.transform.parent.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite.name;//запоминаем название раскраски

                if (PlayerPrefs.HasKey("background_color " + f))//если цвет заднего фона бил сохранен ранее
                {
                    Color a;
                    //то считиваем значение цвета
                    ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("background_color " + f), out a);
                    gameObject.GetComponent<Image>().color = a;//и присваиваем для заднего фона считаной цвет
                }
            }
            else
            {
                string f = gameObject.GetComponent<Image>().sprite.texture.name;//запоминаем название раскраски
                if (PlayerPrefs.HasKey(f))//если ета картинка била сохранена ранее
                    gameObject.GetComponent<Image>().sprite.texture.LoadImage(System.Convert.FromBase64String(PlayerPrefs.GetString(f)));//то загружаем ее
                else
                    gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("origine_pictures/" + f);//иначе загружаем новий спрайт, тоесть чистую раскраску
            }
        }
        
        gameObject.GetComponent<Image>().sprite.texture.Apply();//сохраняем изменение
    }

    public void OnPointerClick(PointerEventData pointerEventData)//при клике на картинку или задний фон
    {
        detectClicks.isPainting = false;
        if (gameObject.name == "paint")
        {
            detectClicks.isPainting = true;
            detectClicks.number = number;
            detectClicks.img = GetComponent<Image>().sprite;
            // detectClicks.background = gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;//устанавиливаем спрайт заднего фона раскраски на сцене для раскраски
            //img.GetComponent<detectClicks>()._start();//запускаем загрузку игри
            SceneManager.LoadScene(2);
            return;
        }
        if(gameObject.name== "background")//если кликнули на задний фон
        {
            //то визиваем функцию клика на саму картинку
            gameObject.transform.parent.GetChild(1).gameObject.GetComponent<loadImg>().OnPointerClick(pointerEventData);
            return;
        }
        //меняем местами очередь прорисовки игрових сцен
        //int tmp = secondScene.transform.GetChild(0).gameObject.GetComponent<Canvas>().sortingOrder;
        //secondScene.transform.GetChild(0).gameObject.GetComponent<Canvas>().sortingOrder = transform.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<Canvas>().sortingOrder;
        //transform.parent.parent.parent.parent.parent.parent.gameObject.GetComponent<Canvas>().sortingOrder = tmp;

        detectClicks.img = GetComponent<Image>().sprite;//устанавиливаем изображение для раскраски на сцене для раскраски
        detectClicks.col = gameObject.transform.parent.GetChild(0).GetComponent<Image>().color;//устанавиливаем цвет заднего фона раскраски на сцене для раскраски
        detectClicks.background = gameObject.transform.parent.gameObject.transform.GetChild(0).GetComponent<Image>().sprite;//устанавиливаем спрайт заднего фона раскраски на сцене для раскраски
        //img.GetComponent<detectClicks>()._start();//запускаем загрузку игри
        SceneManager.LoadScene(1);
        ////разрешаем движение сцен
        //transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<moveScene>().goal = -18f;
        //transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<moveScene>().speed = 9f;
        //transform.parent.parent.parent.parent.parent.parent.parent.GetComponent<moveScene>().move = true;
            

        //secondScene.GetComponent<moveScene>().goal = 0f;
        //secondScene.GetComponent<moveScene>().move = true;
        //secondScene.GetComponent<moveScene>().speed = 15;
    }
}