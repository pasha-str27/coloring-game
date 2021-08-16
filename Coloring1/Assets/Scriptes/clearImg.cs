using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearImg : MonoBehaviour
{
    public GameObject waterDrops;
   public void startClearImg()
    {
        if (!detectClicks.letColoring)
            return;
        Destroy(Instantiate(waterDrops), 7);
        detectClicks.letColoring = false;
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3f);
        detectClicks.toClear = true;
        yield return new WaitForSeconds(3f);
        detectClicks.wasClick= true;
        detectClicks.letColoring = true;
    }
}
