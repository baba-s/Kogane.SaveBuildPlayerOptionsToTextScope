# Kogane Save Build Player Options To Text Scope

ビルド時に BuildPlayerOptions の情報を Resources フォルダのテキストファイルに書き込むエディタ拡張

## 使用例

```cs
using System.Linq;
using Kogane;
using UnityEditor;

public static class Example
{
    [MenuItem( "Tools/Hoge" )]
    public static void Hoge()
    {
        const bool isReleaseBuild = false;

        var options = new BuildPlayerOptions
        {
            scenes           = EditorBuildSettings.scenes.Select( x => x.path ).ToArray(),
            locationPathName = "Build/Game.exe",
            targetGroup      = BuildTargetGroup.Standalone,
            target           = BuildTarget.StandaloneWindows64,
            options          = BuildOptions.AutoRunPlayer,
        };

        using ( new SaveBuildPlayerOptionsToTextScope( isReleaseBuild, options ) )
        {
            BuildPipeline.BuildPlayer( options );
        }
    }
}
```

```cs
using UnityEngine;

public sealed class Example : MonoBehaviour
{
    private void Start()
    {
        var textAsset = Resources.Load<TextAsset>( "build_player_options" );
        Debug.Log( textAsset != null ? textAsset.text : "" );
    }
}
```