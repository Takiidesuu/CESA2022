using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{

    bool zoom_bool = false; // zoom’†‚©‚Ç‚¤‚©
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if (!zoom_bool)
            {
                zoom_bool = true;
                StartCoroutine("ZoomInMove");
            }
            else
            {
                zoom_bool = false;
                StartCoroutine("ZoomOutMove");
            }
        }

    }
    IEnumerator ZoomInMove()
    {
        for (int zoom = 0; zoom < 20; zoom++)
        {
            this.transform.Translate(0, 0, 1);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator ZoomOutMove()
    {
        for (int zoom = 0; zoom < 20; zoom++)
        {
            this.transform.Translate(0, 0, -1);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
