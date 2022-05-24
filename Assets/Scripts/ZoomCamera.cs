using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ZoomCamera : MonoBehaviour
{
    public Camera m_Camera;
    bool zoom_bool = false; // zoom’†‚©‚Ç‚¤‚©
    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
        //    if (!zoom_bool)
        //    {
        //        zoom_bool = true;
        //        StartCoroutine("ZoomInMove");
        //    }
        //    else
        //    {
        //        zoom_bool = false;
        //        StartCoroutine("ZoomOutMove");
        //    }
        //}

    }
    IEnumerator ZoomInMove()
    {
        for (int zoom = 0; zoom < 3; zoom++)
        {
            this.transform.Translate(-0.03f,0.07f, 0);
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

    public void ZoomFov(Camera camera, float zoom, float duration)
    {
        float defaultFov = camera.fieldOfView;
        DOTween.To(() => camera.fieldOfView, fov => camera.fieldOfView = fov,zoom, duration);
    }
}
