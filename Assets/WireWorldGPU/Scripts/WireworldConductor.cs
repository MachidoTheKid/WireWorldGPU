using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using System.IO;
using System;

public class WireworldConductor : MonoBehaviour
{
    const int N = 8;

    public Material renderbuffer;

    public ComputeBuffer WireWorldStatePrevious, WireWorldStateNext;
    //int[] TempBuffer = new int[N * N];

    public ComputeShader WireWorldRules;

    public List<ComputeBuffer> Buffers = new List<ComputeBuffer>();
    int curstate = 1;

    public bool update;


    void Start()
    {

   
        WireWorldStatePrevious = new ComputeBuffer(N * N, sizeof(int));
        Buffers.Add(WireWorldStatePrevious);
        WireWorldStateNext = new ComputeBuffer(N * N, sizeof(int));
        Buffers.Add(WireWorldStateNext);              

        //Preload the Start State with some Data
        Buffers[0].SetData(CreateClock8x8WithClock());
        DebugBufferWrite(Buffers[0], @"C:\Users\Nathan Duker\Documents\Debug.txt");
        renderbuffer.SetInt("_Size", N);
        WireWorldRules.SetInt("_Size", N);

        //Display preloaded Data
        renderbuffer.SetBuffer("_Cells", Buffers[0]);

    }

    void AutomataStep()
    {
        //Previous state = Next State
        //WireWorldStateNext.GetData(TempBuffer);
        //WireWorldStatePrevious.SetData(TempBuffer);
        int Next = curstate;
        curstate = Mathf.Abs(Next - 1);

        //Load & Trigger the Compute shader
        //WireWorldRules.SetBuffer(0, "_Input", WireWorldStatePrevious);
        //WireWorldRules.SetBuffer(0, "_Result", WireWorldStateNext);
        WireWorldRules.SetBuffer(0, "_Input", Buffers[curstate]); //Now look the theory is now we are no longer pulling stuff from the GPU every step
        WireWorldRules.SetBuffer(0, "_Result", Buffers[Next]);    //But instead flipflopping the two buffers, so we skip the write and read from buffer thing
        WireWorldRules.Dispatch(0, N/8, N/8, 1);                  //Someone do a profiler check and confirm this bussiness.

        //Set the Render shader's Content to the Next State
        Shader.SetGlobalBuffer("_Cells", Buffers[Next]);
    }

    int AutomataStepDelay = 0;
    void Update()
    {
        if (!update)
        {
            if (Input.GetMouseButtonDown(2))
            {
                AutomataStep();
            }
        }
        else
        {
            if (AutomataStepDelay >60)
            {
                AutomataStep();
                AutomataStepDelay = 0;
            }
            AutomataStepDelay++;
        }
        


    }

    void OnDestroy()
    {
        WireWorldStatePrevious.Release();
        WireWorldStateNext.Release();
    }

    void DebugBufferWrite(ComputeBuffer buffer, string filename = @"D:\Debug.txt")
    {
        int[] Debugbuffer = new int[N * N];
        buffer.GetData(Debugbuffer);
        using (var outf = new StreamWriter(filename))
            for (int i = 0; i < Debugbuffer.Length; i++)
                outf.WriteLine(Debugbuffer[i]);
    }

    private static int[] MapUniform2DArrayTo1DArray(int[,] array2D)
    {
        int[] array1d = new int[array2D.GetLength(0) * array2D.GetLength(1)];
        for (int x = 0; x < array2D.GetLength(0); x++)
        {
            for (int y = 0; y < array2D.GetLength(1); y++)
            {
                array1d[x * array2D.GetLength(0) + y] = array2D[x, y];
            }
        }
        return array1d;
    }

    private int CalculateIndex(int x, int y)
    {

        return x* N + y;      

    }

    public void setcelltovalue(float x, float y, int value)
    {
        int[] TempBuffer = new int[N * N];
        Buffers[Mathf.Abs(curstate - 1)].GetData(TempBuffer);
        TempBuffer[CalculateIndex(Mathf.FloorToInt(x*(N)), Mathf.FloorToInt(y*(N)))] = value;
        Buffers[Mathf.Abs(curstate - 1)].SetData(TempBuffer);
    }
    int[] CreateClock7x7WithClock()
    {
        int[,] array2D = new int[7, 7] {{ 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 1, 1, 1, 0, 0 },
                                        { 0, 1, 0, 0, 0, 1, 0 },
                                        { 0, 0, 3, 2, 1, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 }};
        return MapUniform2DArrayTo1DArray(array2D);
    }
    int[] CreateClock8x8WithClock()
    {
        int[,] array2D = new int[8, 8] {{ 0, 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 1, 1, 1, 1, 0, 0 },
                                        { 0, 1, 0, 0, 0, 0, 1, 0 },
                                        { 0, 1, 0, 0, 0, 0, 1, 0 },
                                        { 0, 0, 3, 2, 1, 1, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0, 0 }};
        return MapUniform2DArrayTo1DArray(array2D);
    }
}
