#if UNITY_INPUTSYSTEM_ENABLED
using UnityEditor;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace LurkingNinja.Input.Editor
{
    public class OnAssetPostProcessorInput : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                var obj = AssetDatabase.LoadAssetAtPath<Object>(path);
                switch (obj)
                {
                    case InputActionAsset inputActionAsset:
                        InputActionAssetPostProcess.GenerateFile(inputActionAsset, path);
                        break;
                }
            }
        }
    }
}
#endif
