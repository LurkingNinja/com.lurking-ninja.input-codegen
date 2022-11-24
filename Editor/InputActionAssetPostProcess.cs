#if UNITY_INPUTSYSTEM_ENABLED
using System;
using System.Text;
using UnityEngine.InputSystem;

namespace LurkingNinja.Input.Editor
{
    internal static class InputActionAssetPostProcess
    {
	    internal static void DeleteFile(string filename) =>
		    AssetPostProcessorHelper
			    .DeleteFile(filename, InputCodegenSettings.Get.path);

	    internal static void GenerateFile(InputActionAsset inputActionAsset, string filename) =>
		    AssetPostProcessorHelper
			    .WriteFile(filename, InputCodegenSettings.Get.path, GenerateFileContent(inputActionAsset));

	    private static string GenerateFileContent(InputActionAsset inputActionAsset)
		{
			var definitions = new StringBuilder();
			var variables = new StringBuilder();
			var classes = new StringBuilder();

			const string oneDefinition = "\t\t\t{0} = new {0}Actions(asset);";
			const string oneVariable = "\t\tpublic {0}Actions {0} {{ get; }}";
			const string oneAction = "public InputAction {0} => {2}Map.FindAction(\"{1}\");";
			const string oneActionDefinition = "\t\t\tpublic InputAction {0};";
			const string oneActionLet = "\t\t\t\t{0} = _actionMap.FindAction(\"{1}\");";

			foreach(var actionMap in inputActionAsset.actionMaps)
			{
				definitions.Append(string.Format(oneDefinition,
					/*{0}*/AssetPostProcessorHelper.KeyToCSharp(actionMap.name)));
				definitions.Append(Environment.NewLine);
				variables.Append(string.Format(oneVariable,
					/*{0}*/AssetPostProcessorHelper.KeyToCSharp(actionMap.name)));
				variables.Append(Environment.NewLine);

				var actions = new StringBuilder();
				var actionsDefinitions = new StringBuilder();
				var actionsLets = new StringBuilder();
				
				foreach(var inputAction in actionMap.actions)
				{
					actions.Append(string.Format(oneAction,
						/*{0}*/AssetPostProcessorHelper.KeyToCSharp(inputAction.name),
						/*{1}*/AssetPostProcessorHelper.KeyToCSharpWithoutAt(inputAction.name),
						/*{2}*/AssetPostProcessorHelper.KeyToCSharp(actionMap.name)));
					actions.Append(Environment.NewLine);
					actionsDefinitions.Append(string.Format(oneActionDefinition,
						/*{0}*/AssetPostProcessorHelper.KeyToCSharp(inputAction.name)));
					actionsDefinitions.Append(Environment.NewLine);
					actionsLets.Append(string.Format(oneActionLet,
						/*{0}*/AssetPostProcessorHelper.KeyToCSharp(inputAction.name),
						/*{1}*/AssetPostProcessorHelper.KeyToCSharpWithoutAt(inputAction.name)));
					actionsLets.Append(Environment.NewLine);
				}

				classes.Append(string.Format(InputCodegenSettings.Get.classTemplate,
					/*{0}*/AssetPostProcessorHelper.KeyToCSharp(actionMap.name),
					/*{1}*/AssetPostProcessorHelper.KeyToCSharpWithoutAt(actionMap.name),
					/*{2}*/actionsDefinitions,
					/*{3}*/actionsLets));
				classes.Append(Environment.NewLine);
			}

			return string.Format(InputCodegenSettings.Get.template,
				/*{0}*/DateTime.Now,
				/*{1}*/AssetPostProcessorHelper.KeyToCSharp(inputActionAsset.name),
				/*{2}*/definitions,
				/*{3}*/variables,
				/*{4}*/classes,
				/*{5}*/InputCodegenSettings.Get.inputNamespace);
		}
    }
}
#endif