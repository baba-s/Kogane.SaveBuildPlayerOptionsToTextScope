using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Kogane
{
    /// <summary>
    /// ビルドに使用した BuildPlayerOptions の情報を Resources フォルダのテキストファイルに書き込むエディタ拡張
    /// </summary>
    public sealed class SaveBuildPlayerOptionsToTextScope : IDisposable
    {
        //================================================================================
        // 変数(static readonly)
        //================================================================================
        private static readonly string DIRECTORY_NAME = $"Assets/{nameof( SaveBuildPlayerOptionsToTextScope )}/Resources";
        private static readonly string FILE_NAME      = "build_player_options.json";

        //================================================================================
        // 関数
        //================================================================================
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SaveBuildPlayerOptionsToTextScope
        (
            bool               isReleaseBuild,
            BuildPlayerOptions options
        )
        {
            // リリースビルドにテキストファイルが含まれないように
            // ビルド開始時に削除しています
            Refresh();

            if ( isReleaseBuild ) return;

            var jsonOptions = new JsonBuildPlayerOptions( options );
            var json        = JsonUtility.ToJson( jsonOptions, true );

            Directory.CreateDirectory( DIRECTORY_NAME );
            var path = $"{DIRECTORY_NAME}/{FILE_NAME}";
            File.WriteAllText( path, json, Encoding.UTF8 );
            AssetDatabase.ImportAsset( path );
        }

        /// <summary>
        /// ビルド終了時に呼び出されます
        /// </summary>
        public void Dispose()
        {
            Refresh();
        }

        /// <summary>
        /// 作成したテキストファイルを削除します
        /// </summary>
        private static void Refresh()
        {
            var directoryName = Path.GetDirectoryName( DIRECTORY_NAME );
            if ( !AssetDatabase.IsValidFolder( directoryName ) ) return;
            AssetDatabase.DeleteAsset( directoryName );
        }
    }
}