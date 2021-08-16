using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class chooseColor : MonoBehaviour, IPointerClickHandler
{
    public List<GameObject> prefabs;
    public void OnPointerClick(PointerEventData eventData)//при клике на изображение
    {
        if (!detectClicks.letColoring)
            return;
        GetComponent<AudioSource>().Play();//проигриваем звук
        if (tag=="color")
        {
            detectClicks.color = GetComponent<Image>().color;//запрминаем цвет изображения
            FreeDraw.Drawable.Pen_Colour = GetComponent<Image>().color;
            ContollActiveColor.active.GetComponent<Outline>().effectDistance = Vector2.zero;//убираем тени с прошлого изображения
            ContollActiveColor.active = gameObject;//запрминаем вибранное изображение
            ContollActiveColor.active.GetComponent<Outline>().effectDistance = new Vector2(5, 5);//создаем тени для вибраного изображения
            return;
        }
        detectClicks.prefabs = prefabs;
    }
}