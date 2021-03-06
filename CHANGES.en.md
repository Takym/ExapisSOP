# ExapisSOP - Change Logs
Copyright (C) 2020 Takym.

## Overview
| # |Version |Code Name|Date      |Description                                                            |
|--:|:------:|:--------|:---------|:----------------------------------------------------------------------|
|  5|v0.1.0.0|xsop01a0 |2020/00/00|Created the tool "ExapisSOP.Tools.EncodingFixer".                      |
|  4|v0.0.1.2|xsop00b2 |2020/10/26|Implemented the connection model interfaces and classes.               |
|  3|v0.0.1.1|xsop00b1 |2020/10/21|Fixed a bug in the tool "ExapisSOP.Tools.PackageDependencyFixer".      |
|  2|v0.0.1.0|xsop00b0 |2020/10/21|Created the tool "ExapisSOP.Tools.PackageDependencyFixer".             |
|  1|v0.0.0.1|xsop00a1 |2020/10/21|Fixed a bug about settings file compatibility and package dependencies.|
|  0|v0.0.0.0|xsop00a0 |2020/10/20|The first release.                                                     |

## [master]
 * The specification (Japanese): [https://takym.github.io/ExapisSOP/latest/index.html](https://takym.github.io/ExapisSOP/latest/index.html)
 * The comparing: [https://github.com/Takym/ExapisSOP/compare/xsop01a0...master](https://github.com/Takym/ExapisSOP/compare/xsop01a0...master)

## [v0.0.1.2] - xsop00b2
 * **Date:** October 28th, 2020
 * Created the tool "ExapisSOP.Tools.EncodingFixer".
 * The specification (Japanese): [https://takym.github.io/ExapisSOP/v0.1.0.0/index.html](https://takym.github.io/ExapisSOP/v0.1.0.0/index.html)
 * The comparing: [https://github.com/Takym/ExapisSOP/compare/xsop00b2...xsop01a0](https://github.com/Takym/ExapisSOP/compare/xsop00b2...xsop01a0)
 * Added the strategy tool to convert encoding of the solution files from Shift-JIS and UTF-8 with BOM to UTF-8.
 * Added various features to the path string class.
 * Fixed a bug about the color of a console logger.

## [v0.0.1.2] - xsop00b2
 * **Date:** October 26th, 2020
 * Implemented the connection model interfaces and classes.
 * The specification (Japanese): [https://takym.github.io/ExapisSOP/v0.0.1.2/index.html](https://takym.github.io/ExapisSOP/v0.0.1.2/index.html)
 * The comparing: [https://github.com/Takym/ExapisSOP/compare/xsop00b1...xsop00b2](https://github.com/Takym/ExapisSOP/compare/xsop00b1...xsop00b2)
 * Added the interface "ExapisSOP.ConnectionModel.IConnector".
 * Added the interface "ExapisSOP.ConnectionModel.IConnector&lt;in TIn, out TOut&gt;".
 * Added the interface "ExapisSOP.ConnectionModel.IConnectable".
 * Added the interface "ExapisSOP.ConnectionModel.IConnectable&lt;in TIn, out TOut&gt;".
 * Added the static class "ExapisSOP.ConnectionModel.ConnectorExtensions".
 * Added the static class "ExapisSOP.ConnectionModel.ConnectableExtensions".
 * Fixed small of "ExapisSOP.Tools.PackageDependencyFixer".
 * Changed the format of this change logs file.

## [v0.0.1.1] - xsop00b1
 * **Date:** October 21th, 2020
 * Fixed a bug in the tool "ExapisSOP.Tools.PackageDependencyFixer".
 * The specification (Japanese): [https://takym.github.io/ExapisSOP/v0.0.1.1/index.html](https://takym.github.io/ExapisSOP/v0.0.1.1/index.html)
 * The comparing: [https://github.com/Takym/ExapisSOP/compare/xsop00b0...xsop00b1](https://github.com/Takym/ExapisSOP/compare/xsop00b0...xsop00b1)

## [v0.0.1.0] - xsop00b0
 * **Date:** October 21th, 2020
 * Created the tool "ExapisSOP.Tools.PackageDependencyFixer".
 * The specification (Japanese): [https://takym.github.io/ExapisSOP/v0.0.1.0/index.html](https://takym.github.io/ExapisSOP/v0.0.1.0/index.html)
 * The comparing: [https://github.com/Takym/ExapisSOP/compare/xsop00a1...xsop00b0](https://github.com/Takym/ExapisSOP/compare/xsop00a1...xsop00b0)
 * Added the strategy tool to remove a circular reference of the NuGet packages dependencies.
 * Fixed the text format in the EnglishErrorReportBuilder class.

## [v0.0.0.1] - xsop00a1
 * **Date:** October 21th, 2020
 * Fixed a bug about settings file compatibility and package dependencies.
 * The specification (Japanese): [https://takym.github.io/ExapisSOP/v0.0.0.1/index.html](https://takym.github.io/ExapisSOP/v0.0.0.1/index.html)
 * The comparing: [https://github.com/Takym/ExapisSOP/compare/xsop00a0...xsop00a1](https://github.com/Takym/ExapisSOP/compare/xsop00a0...xsop00a1)
 * Added a log output about settings files compatibility.
 * Fixed the determination of settings files compatibility.
 * Fixed dependencies of the NuGet packages.

## [v0.0.0.0] - xsop00a0
 * **Date:** October 20th, 2020
 * The first release.
 * The specification (Japanese): [https://takym.github.io/ExapisSOP/v0.0.0.0/index.html](https://takym.github.io/ExapisSOP/v0.0.0.0/index.html)
 * The comparing: [https://github.com/Takym/ExapisSOP/compare/450d395...xsop00a0](https://github.com/Takym/ExapisSOP/compare/450d395...xsop00a0)


[master]: https://github.com/Takym/ExapisSOP/tree/master
[v0.1.0.0]: https://github.com/Takym/ExapisSOP/releases/tag/xsop01a0
[v0.0.1.2]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00b2
[v0.0.1.1]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00b1
[v0.0.1.0]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00b0
[v0.0.0.1]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00a1
[v0.0.0.0]: https://github.com/Takym/ExapisSOP/releases/tag/xsop00a0
