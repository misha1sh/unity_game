using UnityEditor;
using UnityEngine;

public class WebGLEditorScript
{
    [MenuItem("Tools/Enable webgl threading")]
    public static void EnableErrorMessageTesting() {
        Debug.Log(PlayerSettings.WebGL.threadsSupport);
        PlayerSettings.WebGL.threadsSupport = true;
        PlayerSettings.SetIncrementalIl2CppBuild(BuildTargetGroup.WebGL, true);
        //PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
    
    [MenuItem("Tools/Disable webgl threading")]
    public static void DisableErrorMessageTesting() {
        Debug.Log(PlayerSettings.WebGL.threadsSupport);
        PlayerSettings.WebGL.threadsSupport = false;
        PlayerSettings.SetIncrementalIl2CppBuild(BuildTargetGroup.WebGL, true);
        //PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
    
    [MenuItem("Tools/Set WebGL memory")]
    public static void Setwebglmemory() {
        Debug.Log(PlayerSettings.WebGL.memorySize);
        PlayerSettings.WebGL.memorySize = 512;
        //PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
}