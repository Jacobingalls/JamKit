using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using UnityEditor.Build.Reporting;
using System.Linq;

public class WebGLBuilder
{
    public static void BuildGame()
    {

      string buildPath = System.Environment.GetCommandLineArgs()
          .SkipWhile(arg => arg != "-buildPath")
          .Skip(1)
          .First();

      string[] scenes = EditorBuildSettings.scenes
              .Where(scene => scene.enabled)
              .Select(scene => scene.path)
              .ToArray();

      BuildPipeline.BuildPlayer(scenes, buildPath, BuildTarget.WebGL, BuildOptions.None); 
    }  
}