using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using System.IO;
using System;

public class WireworldConductor : MonoBehaviour
{
    const int N = 8;

    public ComputeBuffer WireWorldStatePrevious, WireWorldStateNext;
    int[] TempBuffer = new int[N * N];

    public ComputeShader WireWorldRules;

    void Start()
    {
        WireWorldStatePrevious = new ComputeBuffer(N * N, sizeof(int));
        WireWorldStateNext = new ComputeBuffer(N * N, sizeof(int));

        //Preload the Start State with some Data
        WireWorldStatePrevious.SetData(CreateClock8x8WithClock());

        Shader.SetGlobalInt("_Size", N);
        WireWorldRules.SetInt("_Size", N);
    }

    void AutomataStep()
    {
        //Previous state = Next State
        WireWorldStateNext.GetData(TempBuffer);
        WireWorldStatePrevious.SetData(TempBuffer);

        //Load & Trigger the Compute shader
        WireWorldRules.SetBuffer(0, "_Input", WireWorldStatePrevious);
        WireWorldRules.SetBuffer(0, "_Result", WireWorldStateNext);
        WireWorldRules.Dispatch(0, N/8, N/8, 1);

        //Set the Render shader's Content to the Next State
        Shader.SetGlobalBuffer("_Cells", WireWorldStateNext);
    }

    int AutomataStepDelay = 0;
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)){
        //    runAutomata();
        //}

        //if (Input.GetMouseButton(0))
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

    int[] CreateClock7x7WithClock()
    {
        int[,] array2D = new int[7, 7] {{ 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 1, 1, 1, 0, 0 },
                                        { 0, 1, 0, 0, 0, 1, 0 },
                                        { 0, 0, 3, 2, 1, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 }};

        int[] array1d = new int[7 * 7];
        for (int x = 0; x < 7; x++)
        {
            for (int y = 0; y < 7; y++)
            {
                array1d[x * 7 + y] = array2D[x, y];
            }
        }
        return array1d;
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

        int[] array1d = new int[8 * 8];
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                array1d[x * 8 + y] = array2D[x, y];
            }
        }
        return array1d;
    }
}
