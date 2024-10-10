using System.Runtime.InteropServices;

namespace MonacoEditor.WPF.HostedObjects;

[ClassInterface(ClassInterfaceType.AutoDual)]
[ComVisible(true)]
public class TextHostedObject
{
    public event Action<string>? TextChanged;

    public void SetText(string text)
    {
        TextChanged?.Invoke(text);  
    }
}
