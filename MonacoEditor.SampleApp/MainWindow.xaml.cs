using System.Windows;

namespace MonacoEditor.SampleApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindowViewModel ViewModel { get; } = new MainWindowViewModel();

    public MainWindow()
    {
        DataContext = ViewModel;
        InitializeComponent();

        Closing += (_, _) => MonacoEditor.Dispose();
    }

    private async void LoadButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.OpenFileDialog();
        if (dialog.ShowDialog() == true)
        {
            await ViewModel.OpenFromFile(dialog.FileName);
        }
    }

    private async void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new Microsoft.Win32.SaveFileDialog();

        if (dialog.ShowDialog() == true)
        {
            await ViewModel.SaveToFile(dialog.FileName);
        }
    }
}