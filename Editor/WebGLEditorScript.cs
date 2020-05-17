using UnityEditor;
using UnityEngine;

/// <summary>
///     Класс для установки настроек сборки в webgl
/// </summary>
public class WebGLEditorScript
{
    /// <summary>
    ///     Устанавливает настройки сборки в webgl
    /// </summary>
    [MenuItem("Tools/Setup webgl settings")]
    public static void DisableErrorMessageTesting() {
        Debug.Log(PlayerSettings.WebGL.threadsSupport);
        PlayerSettings.WebGL.threadsSupport = false;
        PlayerSettings.SetIncrementalIl2CppBuild(BuildTargetGroup.WebGL, true);
        Debug.Log(PlayerSettings.WebGL.memorySize);
        PlayerSettings.WebGL.memorySize = 512;
        //PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
}