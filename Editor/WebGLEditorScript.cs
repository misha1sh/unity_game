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
}