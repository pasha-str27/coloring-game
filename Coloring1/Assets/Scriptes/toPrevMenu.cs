using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class toPrevMenu : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        //меняем местами очередь прорисовки сцен
        //int tmp = secondScene.transform.GetChild(0).gameObject.GetComponent<Canvas>().sortingOrder;
        //secondScene.transform.GetChild(0).gameObject.GetComponent<Canvas>().sortingOrder = transform.parent.parent.parent.gameObject.GetComponent<Canvas>().sortingOrder;
        //transform.parent.parent.parent.gameObject.GetComponent<Canvas>().sortingOrder = tmp;

        if (!detectClicks.letColoring)
            return;
        detectClicks.exit = true;//виходим из сцени с раскраской
        StartCoroutine(wait5Seconds());
        //SceneManager.LoadScene(0);
        //разорешаем движение сцен
        //secondScene.GetComponent<moveScene>().goal = 0f;
        //secondScene.GetComponent<moveScene>().speed = -15f;
        //transform.parent.parent.parent.parent.GetComponent<moveScene>().goal = 18f;
        //transform.parent.parent.parent.parent.GetComponent<moveScene>().speed = -9f;
        //transform.parent.parent.parent.parent.GetComponent<moveScene>().move = true;
        //secondScene.GetComponent<moveScene>().move = true;
    }

    IEnumerator wait5Seconds()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        SceneManager.LoadScene(0);
    }
}