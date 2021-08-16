using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnClick : MonoBehaviour
{
    public float speed;
    public GameObject effect;

    private void Update()
    {
        gameObject.transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    private void OnMouseDown()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Destroy(Instantiate(effect, mousePos, Quaternion.identity), 5);
        Destroy(gameObject);
    }
}
