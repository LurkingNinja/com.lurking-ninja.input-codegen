#if UNITY_INPUTSYSTEM_ENABLED
using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LurkingNinja.Input.Editor
{
    internal static class InputActionAssetPostProcess
    {
	    private const string InputNamespace = "LurkingNinja.Input";
	    private const string BasePath = "Assets/Plugins/LurkingNinja/GameFoundation/_generated/";
	    private const string BaseTemplatePath = "Packages/com.lurkingninja.game-foundation/Editor/ScriptTemplates/";
        private const string Path = BasePath + "Input/";
        private const string TemplatePath = BaseTemplatePath + "Input/Input.cs.txt";
		private const string ClassTemplatePath = BaseTemplatePath + "Input/Input.Class.cs.txt";

        internal static void GenerateFile(InputActionAsset inputActionAsset, string fileName) =>
				AssetPostProcessorHelper.WriteFile(fileName, Path, GenerateFileContent(inputActionAsset));

		private static string GenerateFileContent(InputActionAsset inputActionAsset)
		{
			var template = AssetDatabase.LoadAssetAtPath<TextAsset>(TemplatePath).text;
			var classTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>(ClassTemplatePath).text;
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

				classes.Append(string.Format(classTemplate,
					/*{0}*/AssetPostProcessorHelper.KeyToCSharp(actionMap.name),
					/*{1}*/AssetPostProcessorHelper.KeyToCSharpWithoutAt(actionMap.name),
					/*{2}*/actionsDefinitions,
					/*{3}*/actionsLets));
				classes.Append(Environment.NewLine);
			}

			return string.Format(template,
				/*{0}*/DateTime.Now,
				/*{1}*/AssetPostProcessorHelper.KeyToCSharp(inputActionAsset.name),
				/*{2}*/definitions,
				/*{3}*/variables,
				/*{4}*/classes,
				/*{5}*/InputNamespace);
		}
    }
}
#endif