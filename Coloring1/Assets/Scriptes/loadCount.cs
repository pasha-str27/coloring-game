using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadCount : MonoBehaviour
{
    public int maxCount;
    void Start()//прис старте
    {
        int count = 0;
        for (int i =1; i <= maxCount; ++i)
            if (PlayerPrefs.HasKey(i.ToString()))//находим количество ранее сохраненних картинок
                ++count;
        GetComponent<Text>().text = "Раскрашено " + count.ToString() + " из "+ maxCount.ToString();//и воводим найденное количество на екран
    }
    //private void Update()
    //{
    //    if(loadImg.reload)//если ми перезагружаем главное меню
    //    {
    //        int count = 0;
    //        for (int i = 1; i <= maxCount; ++i)
    //            if (PlayerPrefs.HasKey(i.ToString()))//то находим количество ранее сохраненних картинок
    //                ++count;
    //        GetComponent<Text>().text = "Раскрашено " + count.ToString() + " из "+maxCount.ToString();//и воводим найденное количество на екран
    //    }
    //}
}
