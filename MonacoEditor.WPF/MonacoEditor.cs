using Microsoft.Web.WebView2.Wpf;
using MonacoEditor.WPF.Enums;
using MonacoEditor.WPF.Wrappers;
using System.Windows;
using System.Windows.Controls;

namespace MonacoEditor.WPF;

public class MonacoEditor : ContentControl, IDisposable
{
    private readonly MonacoEditorWrapper editorWrapper;

    // holds actual value for text
    private string text;

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        name: nameof(Text),
        propertyType: typeof(string),
        ownerType: typeof(MonacoEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            propertyChangedCallback: async (o, args) =>
            {
                MonacoEditor editor = (o as MonacoEditor)!;
                string newValue = args.NewValue as string ?? string.Empty;

                if (editor.text == newValue)
                {
                    return;
                }
                await editor.editorWrapper.SetText((string)args.NewValue);
            }));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty ThemeProperty = DependencyProperty.Register(
        name: nameof(Theme),
        propertyType: typeof(MonacoTheme),
        ownerType: typeof(MonacoEditor),
        typeMetadata: new FrameworkPropertyMetadata(
            propertyChangedCallback: async (o, args) =>
            {
                MonacoEditor editor = (o as MonacoEditor)!;
                await editor.editorWrapper.SetTheme((MonacoTheme)args.NewValue);
            }));

    public MonacoTheme Theme
    {
        get => (MonacoTheme)GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public static readonly DependencyProperty OpenDevToolsProperty = DependencyProperty.Register(
       name: nameof(OpenDevTools),
       propertyType: typeof(bool),
       ownerType: typeof(MonacoEditor),
       typeMetadata: new FrameworkPropertyMetadata(
           propertyChangedCallback: async (o, args) =>
           {
               MonacoEditor editor = (o as MonacoEditor)!;

               if ((bool)args.NewValue)
               { 
                   await editor.editorWrapper.OpenDevToolsWindow();
               }
           }));

    public bool OpenDevTools
    {
        get => (bool)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public MonacoEditor()
    {
        IWebView2 webView2 = new WebView2();
        editorWrapper = new MonacoEditorWrapper(webView2);
        Content = webView2;
        editorWrapper.TextChanged += EditorWrapper_TextChanged;

        this.Loaded += async (_, _) =>
        { 
            await editorWrapper.Initialize();
            await editorWrapper.SetText(Text);
            await editorWrapper.SetTheme(Theme);
        };

        SetValue(TextProperty, string.Empty);
        SetValue(ThemeProperty, MonacoTheme.Light);
    }

    private void EditorWrapper_TextChanged(string text)
    {
        this.text = text;
        Text = text;
    }

    public void Dispose()
    {
        editorWrapper.Dispose();
        editorWrapper.TextChanged -= EditorWrapper_TextChanged;
    }
}
