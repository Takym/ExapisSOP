# ExapisSOP
Copyright (C) 2020 Takym.

[日本語](#ja)
[English](#en)

# <a id="en"></a>English
## Summary
This library provides the service-oriented programming.

## Get Started
```csharp
using System;
using System.Threading.Tasks;
using ExapisSOP;
using ExapisSOP.Core;

namespace Example
{
	internal class Program : AppWorker // Needs to extend AppWorker class.
	{
		// This function is called when initializing.
		// Sets the update event method.
		public override async Task InitializeAsync(IContext context)
		{
			this.Update += this.Program_Update;
			await base.InitializeAsync(context);
		}

		// Update event
		private void Program_Update(object? sender, ContextEventArgs e)
		{
			// TODO: write your code in here:
			Console.WriteLine("Hello, World!!");
			Console.ReadKey(true);

			// Terminate the program:
			throw new TerminationException();
		}

		// Entry point
		[STAThread()]
		private static async Task<int> Main(string[] args)
		{
			// Configure how to run the application:
			return await HostRunner.Create(args).Configure(
				config => config.AddAppWorker<Program>() // Adds Program as an AppWorker
			).Build().RunAsync();
		}
	}
}
```

## Latest Version History
|Version |Code Name|Date      |Description       |
|:------:|:--------|:---------|:-----------------|
|v0.0.0.0|xsop00a0 |2020/00/00|The first release.|
||||For more history, please see [CHANGES.en.md](./CHANGES.en.md) file.|

## Terms of Use
This library is released and distributed under the MIT License.
Please see [LICENSE.txt](./LICENSE.txt) file.
<!-- Special thanks to all [core collaborators and contributors](./CONTRIBUTORS.md) for this project. -->

## Contribution
Feel free to submit an issue if the library have bugs, vulnerabilities, or
suggestions of new feature.
Please note below when you propose a pull request (PR):
* **One PR** can have only **one change**.
* Do not use/refer other libraries.
* Copyrights will be transferred to [@Takym](https://github.com/Takym) when the PR is merged.
* Not always merge your PR.
* Write your name and GitHub ID in [CONTRIBUTORS.md](./CONTRIBUTORS.md).

***


# <a id="ja"></a>日本語
## 概要
サービス指向プログラミングに必要な機能を提供します。

## 使い方
```csharp
using System;
using System.Threading.Tasks;
using ExapisSOP;
using ExapisSOP.Core;

namespace Example
{
	internal class Program : AppWorker // AppWorker クラスを継承する必要があります。
	{
		// この関数は初期化時に呼び出されます。
		// 更新イベントを設定しています。
		public override async Task InitializeAsync(IContext context)
		{
			this.Update += this.Program_Update;
			await base.InitializeAsync(context);
		}

		// 更新イベント
		private void Program_Update(object? sender, ContextEventArgs e)
		{
			// TODO: ここにコードを書いてください：
			Console.WriteLine("Hello, World!!");
			Console.ReadKey(true);

			// プログラムを終了させます。
			throw new TerminationException();
		}

		// 開始地点
		[STAThread()]
		private static async Task<int> Main(string[] args)
		{
			// アプリケーションの実行に関する設定を行います。
			return await HostRunner.Create(args).Configure(
				config => config.AddAppWorker<Program>() // Program を AppWorker として追加します。
			).Build().RunAsync();
		}
	}
}
```

## 最近の更新履歴
|バージョン|開発コード名|日付      |説明                |
|:--------:|:-----------|:---------|:-------------------|
|v0.0.0.0  |xsop00a0    |2020/00/00|最初のリリースです。|
||||過去の更新は[CHANGES.ja.md](./CHANGES.ja.md)をご覧ください。|

## 利用規約
このライブラリはMITライセンスの下で公開・配布されています。
詳細は[LICENSE.txt](./LICENSE.txt)から確認してください。
<!-- この場を借りてお礼を申し上げます。全ての[協力者さんと貢献者さん](./CONTRIBUTORS.md)に感謝致します。 -->

## 貢献方法
問題が見つかれば是非気軽に Issue の投稿をしてください。
新規機能の提案も受け付けています。
プル・リクエスト(PR)も受け付けていますが以下の点に注意してください。
* **一つのPR**は**一つの変更**のみにしてください。
* 他の既存のライブラリを参照しないでください。
* マージされた場合、著作権、知的財産権は[@Takym](https://github.com/Takym)に譲渡されます。
* 必ずマージされるわけではありません。
* 貴方の名前を GitHub ID を[CONTRIBUTORS.md](./CONTRIBUTORS.md)に記入してください。
