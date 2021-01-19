using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using System.IO;

public class WireworldConductor : MonoBehaviour

{

    const int N=7;

    public ComputeBuffer WireWorldBufferPrevious, WireWorldBufferNext;

    public ComputeShader WireWorldRules;

    


    void DebugBufferWrite(ComputeBuffer buffer, string filename = @"C:\Users\Nathan Duker\Documents\Debug.txt")
    {

        int[] Debugbuffer = new int[N * N];
        buffer.GetData(Debugbuffer);
        using (var outf = new StreamWriter(filename))
            for (int i = 0; i < Debugbuffer.Length; i++)
                outf.WriteLine(Debugbuffer[i]);

    }

    void OnDestroy()
    {

        WireWorldBufferPrevious.Release();
        WireWorldBufferNext.Release();
    }
    // Start is called before the first frame update
    void Start()
    {

        WireWorldBufferPrevious = new ComputeBuffer(N * N, sizeof(int));
        WireWorldBufferNext = new ComputeBuffer(N * N, sizeof(int));
        //int[] tempArr = new int[N * N];




        int[,] array2D = new int[7, 7] { { 0, 1, 1, 1, 1, 0, 0 },
                                             { 1, 0, 0, 0, 0, 1, 0 },
                                             { 0, 1, 3, 2, 1, 0, 0 },
                                             { 0, 0, 0, 0, 0, 0, 0 },
                                             { 0, 0, 0, 0, 0, 0, 0 },
                                             { 0, 0, 0, 0, 0, 0, 0 },
                                             { 0, 0, 0, 0, 0, 0, 0 } };
        int[] array1d = new int[7 * 7];

        for (int x = 0; x < 6; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                array1d[x * 7 + y] = array2D[x, y];

            }

        }




        /*for(int i = 0; i<(N*N)-1; i++)
        {
            tempArr[i] = Mathf.RoundToInt(i % 2);


        }*/

        WireWorldBufferPrevious.SetData(array1d);
        DebugBufferWrite(WireWorldBufferPrevious);
        Shader.SetGlobalBuffer("_Buffer", WireWorldBufferPrevious);
        WireWorldRules.SetBuffer(0,"_Input", WireWorldBufferPrevious);     
        WireWorldRules.SetBuffer(0,"_Result", WireWorldBufferNext);

        WireWorldRules.Dispatch(0, 7, 7, 1);

        DebugBufferWrite(WireWorldBufferNext, @"C:\Users\Nathan Duker\Documents\Demo2.txt");




    }

    // Update is called once per frame\
    int framecount = 0;
    void runAutomata()
    {
      
        if (framecount > 20)
        {            
            int[] bufferarray = new int[N * N];
            WireWorldBufferNext.GetData(bufferarray);
            WireWorldBufferPrevious.SetData(bufferarray);
            WireWorldRules.SetBuffer(0, "_Input", WireWorldBufferPrevious);
            WireWorldRules.SetBuffer(0, "_Result", WireWorldBufferNext);
            WireWorldRules.Dispatch(0, 7, 7, 1);
            Shader.SetGlobalBuffer("_Buffer", WireWorldBufferNext);
            framecount = 0;
        }
        framecount++;
        Debug.Log(framecount);
    }
  
    void Update()
    {
        if (Input.GetMouseButton(0)){

            //Debug.Log("Pressed primary button.");
            runAutomata();


    }


    }
}
