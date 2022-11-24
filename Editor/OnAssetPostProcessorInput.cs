#if UNITY_INPUTSYSTEM_ENABLED
using UnityEditor;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

namespace LurkingNinja.Input.Editor
{
    // To detect creation and saving an asset. We do not care about moving.
    public class OnAssetPostProcessorInput : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!InputCodegenSettings.Get.inputCodegenEnabled) return;
            foreach (var path in importedAssets)
                if (AssetDatabase.LoadAssetAtPath<Object>(path) is InputActionAsset inputActionAsset)
                    InputActionAssetPostProcess.GenerateFile(inputActionAsset, path);
        }
    }

    // To detect asset removal.
    public class CustomAssetModificationProcessor : AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions rao)
        {
            if (AssetDatabase.LoadAssetAtPath<Object>(path) is InputActionAsset inputActionAsset)
                InputActionAssetPostProcess.DeleteFile(path);
            return AssetDeleteResult.DidNotDelete;
        }
    }
}
#endif
