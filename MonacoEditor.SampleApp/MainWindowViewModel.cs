using CommunityToolkit.Mvvm.ComponentModel;
using MonacoEditor.WPF.Enums;
using System.IO;

namespace MonacoEditor.SampleApp;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string text = "test";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MonacoTheme))]
    private bool isDarkTheme;

    public MonacoTheme MonacoTheme => IsDarkTheme ? MonacoTheme.Dark : MonacoTheme.Light;

    public async Task OpenFromFile(string path)
    {
        Text = await File.ReadAllTextAsync(path);
    }

    public async Task SaveToFile(string path)
    { 
        await File.WriteAllTextAsync(path, Text);
    }
}
