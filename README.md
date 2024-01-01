# Input System Codegen
This is a simple package to automatically generate accessor code for Input System assets.
## Installation
Install the prerequisite [Codegen](https://github.com/LurkingNinja/com.lurking-ninja.codegen) package and the [Input System](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/index.html) package.
You can choose manually installing the package or from GitHub source.
### Add package from git URL
Use the Package Manager's ```+/Add package from git URL``` function.
The URL you should use is this:
```
https://github.com/LurkingNinja/com.lurking-ninja.input-codegen.git?path=Packages/com.lurking-ninja.input-codegen
```
### Manual install
1. Download the latest ```.zip``` package from the [Release](https://github.com/LurkingNinja/com.lurking-ninja.input-codegen/releases) section.
2. Unpack the ```.zip``` file into your project's ```Packages``` folder.
3. Open your project and check if it is imported properly.
## Usage
After the package installed, all you need to do is to create your [InputActionAsset](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/ActionAssets.html) as normal.
When you create the asset or when you save it, this package generates a file in the ```Assets/Plugins/LurkingNinja/InputCodegen``` folder with the same name you named your asset with the exception of all spaces will be replaced with underscore character (_) and all special characters will be omitted. Please be careful with filenames from now on, they will serve as struct name in code as well.

Actual use of the new code if fairly simple, here is an example usage code:

```CSharp
using UnityEngine;
using UnityEngine.InputSystem;
using LurkingNinja.Input;

public class TestInputCodegen : MonoBehaviour
{

    [SerializeField]
    private InputActionAsset _actionAsset;
    
    private New_Controls input;
    
    private void Awake()
    {
        input = new New_Controls(_actionAsset);
        input.Enable();
        input.Test.TestAction.performed += TestPerformed;
    }

    private void TestPerformed(InputAction.CallbackContext ctx) =>
        Debug.Log("Input action performed.");
}
```
This example carries a couple of assumptions:
- created the new InputActionAsset with the default name of "New Controls"
- created an action map called "Test"
- renamed the example action to "TestAction"
- changed the binding something meaningful

In order to make this example work
- copy the code above and paste into a file called "TestInputCodegen.cs" in your project
- attach it to something in your scene
- drag and drop the "New Controls" asset into the field in the Inspector

When you hit play, you should be able to fire the action through the binding you chose and see the message ```Input action performed.``` in the Console. 

From here, it's up to you how you organize your code and feel free to fork this package to your heart's content.