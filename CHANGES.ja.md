# ExapisSOP - 更新履歴
Copyright (C) 2020 Takym.

## 概略
| # |バージョン|開発コード名|日付      |説明                                                            |
|--:|:--------:|:-----------|:---------|:---------------------------------------------------------------|
|  5|v0.1.0.0  |xsop01a0    |2020/00/00|ツール「ExapisSOP.Tools.EncodingFixer」作成。                   |
|  4|v0.0.1.2  |xsop00b2    |2020/10/26|接続可能なオブジェクトを実装。                                  |
|  3|v0.0.1.1  |xsop00b1    |2020/10/21|ツール「ExapisSOP.Tools.PackageDependencyFixer」の不具合修正。  |
|  2|v0.0.1.0  |xsop00b0    |2020/10/21|ツール「ExapisSOP.Tools.PackageDependencyFixer」作成。          |
|  1|v0.0.0.1  |xsop00a1    |2020/10/21|設定ファイルの互換性に関する不具合とパッケージの依存関係を修正。|
|  0|v0.0.0.0  |xsop00a0    |2020/10/20|最初のリリースです。                                            |

## [master]
 * 仕様書：[https://takym.github.io/ExapisSOP/latest/index.html](https://takym.github.io/ExapisSOP/latest/index.html)
 * 比較：[https://github.com/Takym/ExapisSOP/compare/xsop01a0...master](https://github.com/Takym/ExapisSOP/compare/xsop01a0...master)

## [v0.1.0.0] - xsop01a0
 * **更新日：** 2020年10月28日
 * ツール「ExapisSOP.Tools.EncodingFixer」作成。
 * 仕様書：[https://takym.github.io/ExapisSOP/v0.1.0.0/index.html](https://takym.github.io/ExapisSOP/v0.1.0.0/index.html)
 * 比較：[https://github.com/Takym/ExapisSOP/compare/xsop00b2...xsop01a0](https://github.com/Takym/ExapisSOP/compare/xsop00b2...xsop01a0)
 * 「Shift-JIS」「UTF-8 with BOM」を「UTF-8」へ一括変換してくれるツールを作成。
 * パス文字列に様々な機能を追加。
 * コンソールロガーの色に関する不具合を修正。

## [v0.0.1.2] - xsop00b2
 * **更新日：** 2020年10月26日
 * 接続可能なオブジェクトを実装。
 * 仕様書：[https://takym.github.io/ExapisSOP/v0.0.1.2/index.html](https://takym.github.io/ExapisSOP/v0.0.1.2/index.html)
 * 比較：[https://github.com/Takym/ExapisSOP/compare/xsop00b1...xsop00b2](https://github.com/Takym/ExapisSOP/compare/xsop00b1...xsop00b2)
 * インターフェース「ExapisSOP.ConnectionModel.IConnector」を作成。
 * インターフェース「ExapisSOP.ConnectionModel.IConnector&lt;in TIn, out TOut&gt;」を作成。
 * インターフェース「ExapisSOP.ConnectionModel.IConnectable」を作成。
 * インターフェース「ExapisSOP.ConnectionModel.IConnectable&lt;in TIn, out TOut&gt;」を作成。
 * 静的クラス「ExapisSOP.ConnectionModel.ConnectorExtensions」を作成。
 * 静的クラス「ExapisSOP.ConnectionModel.ConnectableExtensions」を作成。
 * 「ExapisSOP.Tools.PackageDependencyFixer」を微修正。
 * 更新履歴の書式を変更。

## [v0.0.1.1] - xsop00b1
 * **更新日：** 2020年10月21日
 * ツール「ExapisSOP.Tools.PackageDependencyFixer」の不具合修正。
 * 仕様書：[https://takym.github.io/ExapisSOP/v0.0.1.1/index.html](https://takym.github.io/ExapisSOP/v0.0.1.1/index.html)
 * 比較：[https://github.com/Takym/ExapisSOP/compare/xsop00b0...xsop00b1](https://github.com/Takym/ExapisSOP/compare/xsop00b0...xsop00b1)

## [v0.0.1.0] - xsop00b0
 * **更新日：** 2020年10月21日
 * ツール「ExapisSOP.Tools.PackageDependencyFixer」作成。
 * 仕様書：[https://takym.github.io/ExapisSOP/v0.0.1.0/index.html](https://takym.github.io/ExapisSOP/v0.0.1.0/index.html)
 * 比較：[https://github.com/Takym/ExapisSOP/compare/xsop00a1...xsop00b0](https://github.com/Takym/ExapisSOP/compare/xsop00a1...xsop00b0)
 * NuGetパッケージの依存関係の循環参照対策用ツールの作成。
 * EnglishErrorReportBuilder の書式を修正。

## [v0.0.0.1] - xsop00a1
 * **更新日：** 2020年10月21日
 * 設定ファイルの互換性に関する不具合とパッケージの依存関係を修正。
 * 仕様書：[https://takym.github.io/ExapisSOP/v0.0.0.1/index.html](https://takym.github.io/ExapisSOP/v0.0.0.1/index.html)
 * 比較：[https://github.com/Takym/ExapisSOP/compare/xsop00a0...xsop00a1](https://github.com/Takym/ExapisSOP/compare/xsop00a0...xsop00a1)
 * 設定ファイルの互換性の有無をログ出力。
 * 設定ファイルの互換性の判定方法を修正。
 * NuGetパッケージの依存関係を修正。

## [v0.0.0.0] - xsop00a0
 * **更新日：** 2020年10月20日
 * 最初のリリースです。
 * 仕様書：[https://takym.github.io/ExapisSOP/v0.0.0.0/index.html](https://takym.github.io/ExapisSOP/v0.0.0.0/index.html)
 * 比較：[https://github.com/Takym/ExapisSOP/compare/450d395...xsop00a0](https://github.com/Takym/ExapisSOP/compare/450d395...xsop00a0)


[master]: https://github.com/Takym/ExapisSOP/tree/master
[v0.1.0.0]: https://github.com/Takym/ExapisSOP/releases/tag/xsop01a0
[v0.0.1.2]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00b2
[v0.0.1.1]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00b1
[v0.0.1.0]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00b0
[v0.0.0.1]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00a1
[v0.0.0.0]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00a0
