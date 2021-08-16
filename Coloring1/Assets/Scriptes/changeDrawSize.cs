using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeDrawSize : MonoBehaviour
{
    public GameObject panel;
    public int size;

    public void showPanel()
    {
        if(panel.activeSelf)
            panel.SetActive(false);
        else
        {
            panel.SetActive(true);
            detectClicks.letColoring = false;
        }
    }

    public void changeSize()
    {
        FreeDraw.Drawable.Pen_Width = this.size;
        transform.parent.gameObject.SetActive(false);
        detectClicks.letColoring = true;
    }
}
