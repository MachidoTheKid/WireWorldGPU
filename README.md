# WireWorld Cellular Automata GPU Implementation

## Project Description:
This project is an implementation of the WireWorld Cellular Automata. For more information on what Cellular Automata is and why it's so cool, feel free to check out the [WireWorld Repository Wiki](https://github.com/GollyGang/ruletablerepository/wiki") written by Tim Hutton. It details various important pieces of information, namely the rules and how to use them. This project was put together with the use of Unity 3D and various existing assets/packages.  

<div align="center">
<img src=../WireWorld.gif" >
</div>

## Project Motivation:
This exists as an example of how to run a Cellular Automata simulation on a grid, in Unity, using the GPU. Specifically, setting up with the use of a fragment shader and compute shader. The rules can be adjusted to match other types of existing Cellular Automata rules that currently exist.

## Core Files:
Wireworldrules.compute: This Compute Shader file contains all the logic that establishes the rules for WireWorld to run on the grid. 


WireworldConductor.cs : C# script that runs the WireWorld Automata on the grid. 


WireworldInteraction.cs : C# script that allows the user to change the values of each cell on the grid.


BufferRender.shader: Shader file that generates a grid on which the WireWorld Cellular Automata will run from. 

## The Rules
1) Empty Cells Stay Empty.
2) Electron Heads always become Electron Tails.
3) Electron Tail Becomes Wire.
4) Wire stays Wire unless it has 2 or more neighbours that are Electron Heads. It becomes an Electron Head in that case.
