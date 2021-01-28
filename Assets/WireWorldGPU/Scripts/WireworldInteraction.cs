using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireworldInteraction : MonoBehaviour
{
    public Camera cam;
    public WireworldConductor conductor;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            return;

        RaycastHit hit;
        if (!Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit))
            return;
        Debug.Log(hit.transform.name);
        Renderer rend = hit.transform.GetComponent<Renderer>();

        //Texture2D tex = rend.material.mainTexture as Texture2D;
        Vector2 pixelUV = hit.textureCoord;
        Debug.Log(pixelUV);
        /*pixelUV.x *= tex.width;
        pixelUV.y *= tex.height;*/
       
        if(Input.GetMouseButtonDown(0))
            conductor.setcelltovalue( pixelUV.x, pixelUV.y, 1);

        if (Input.GetMouseButtonDown(1))
            conductor.setcelltovalue(pixelUV.x, pixelUV.y, 0);
    }
}
