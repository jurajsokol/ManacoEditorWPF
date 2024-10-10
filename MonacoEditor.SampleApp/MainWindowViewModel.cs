using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;

namespace MonacoEditor.SampleApp;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string text = "test";

    public async Task OpenFromFile(string path)
    {
        Text = await File.ReadAllTextAsync(path);
    }

    public async Task SaveToFile(string path)
    { 
        await File.WriteAllTextAsync(path, Text);
    }
}
