using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using MonacoEditor.WPF.Enums;
using MonacoEditor.WPF.HostedObjects;
using System.Drawing;
using System.IO;

namespace MonacoEditor.WPF.Wrappers;

internal class MonacoEditorWrapper : IDisposable
{
    private readonly string MONACO_PAGE_SOURCE = @"MonacoSource\index.html";

    private readonly IWebView2 webView2;
    private TaskCompletionSource editorLoaded = new TaskCompletionSource();
    private readonly TextHostedObject textHostedObject = new TextHostedObject();

    public event Action<string>? TextChanged;

    public Task EditorLoadedTask => editorLoaded.Task;

    public MonacoEditorWrapper(IWebView2 webView2)
    {
        this.webView2 = webView2;
        textHostedObject.TextChanged += text => OnTextChanged(text);
    }

    public async Task Initialize()
    {
        // set UDF for web view 2
        CoreWebView2Environment env = await CoreWebView2Environment.CreateAsync(userDataFolder: Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WebView2"));
        await webView2.EnsureCoreWebView2Async(env);

        webView2.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        webView2.Source = new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, MONACO_PAGE_SOURCE));
        webView2.CoreWebView2.AddHostObjectToScript(nameof(textHostedObject), textHostedObject);
    }

    public async Task OpenDevToolsWindow()
    {
        await EditorLoadedTask;
        webView2.CoreWebView2.OpenDevToolsWindow();
    }

    private void CoreWebView2_NavigationCompleted(object? sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
    {
        webView2.CoreWebView2.NavigationCompleted -= CoreWebView2_NavigationCompleted;
        editorLoaded.SetResult();
    }

    public async Task SetText(string text)
    {
        if (text is null)
        {
            return;
        }

        await EditorLoadedTask;
        await webView2.ExecuteScriptAsync($"model.setValue(`{text}`);");
    }

    public async Task<string> GetText()
    {
        await EditorLoadedTask;
        return await webView2.ExecuteScriptAsync("model.getValue()");
    }

    public async Task SetTheme(MonacoTheme theme)
    {
        await EditorLoadedTask;

        if (theme == MonacoTheme.Dark)
        {
            await webView2.ExecuteScriptAsync("monaco.editor.setTheme('vs-dark');");
        }
        else
        {
            await webView2.ExecuteScriptAsync("monaco.editor.setTheme('vs-light');");
        }
    }

    private void OnTextChanged(string text)
    {
        TextChanged?.Invoke(text);
    }

    public void Dispose()
    {
        webView2.Dispose();
    }
}
