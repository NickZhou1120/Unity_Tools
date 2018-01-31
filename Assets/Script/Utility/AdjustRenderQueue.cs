using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustRenderQueue : MonoBehaviour
{

    public int RenderQueue = 3000;
    private int renderQueue
    {
        get
        {
            if (RenderQueue < 1) 
                RenderQueue = 1;
            return RenderQueue;
        }
    }



    void Update()
    {
        Renderer[] renders = transform.GetComponentsInChildren<Renderer>();
        if (renders != null)
        {
            for (int i = 0; i < renders.Length; ++i)
            {
                if (renders[i].material != null)
                    renders[i].sharedMaterial.renderQueue = renderQueue;
            }
        }

    }

}
