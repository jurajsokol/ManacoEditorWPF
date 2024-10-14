# Monaco editor control for WPF

This is Monaco editor source wrapped as WPF control (editor is used in Visual Studio Code). With this wrapper it can be used directly in XAML as any other control.

## Sample

``` xml
<Window xmlns:editor="clr-namespace:MonacoEditor.WPF;assembly=MonacoEditor.WPF">
     <editor:MonacoEditor />
</Window>
```

## Features

* Text binding
* Theme binding
* Debug window can be opened by property

## ToDo

* Diff control
* Features from Monaco playground https://microsoft.github.io/monaco-editor/playground.html

## About implemetnation

Copy of source code is placed in a folder and loaded in WebView. That is wrapped and on top of that is a WPF control.

Monaco version: 0.52.0
