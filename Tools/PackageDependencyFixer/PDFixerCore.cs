/****
 * ExapisSOP
 * Copyright (C) 2020 Takym.
 * 
 * distributed under the MIT License.
****/

using System;
using System.IO;
using System.IO.Compression;
using System.Xml;
using ExapisSOP.IO;
using ExapisSOP.IO.Logging;

namespace ExapisSOP.Tools.PackageDependencyFixer
{
	internal static class PDFixerCore
	{

#if NETCOREAPP3_1 && !(ARM || x64 || x86)

		internal static void Fix(ILogger? logger, IContext context)
		{
			var path = new PathString(AppDomain.CurrentDomain.BaseDirectory ?? Environment.CurrentDirectory)
				.GetDirectoryName()?.GetDirectoryName()?.GetDirectoryName()?.GetDirectoryName();
			if (path is null) {
				logger?.Warn("Could not load the solution path.");
				return;
			}
			logger?.Info($"The solution path is: {path}");
			Fix(logger, path + "bin\\Debug",        path + ".packages\\Debug_AnyCPU");
			Fix(logger, path + "bin\\ARM\\Debug",   path + ".packages\\Debug_ARM");
			Fix(logger, path + "bin\\x64\\Debug",   path + ".packages\\Debug_x64");
			Fix(logger, path + "bin\\x86\\Debug",   path + ".packages\\Debug_x86");
			Fix(logger, path + "bin\\Release",      path + ".packages\\Release_AnyCPU");
			Fix(logger, path + "bin\\ARM\\Release", path + ".packages\\Release_ARM");
			Fix(logger, path + "bin\\x64\\Release", path + ".packages\\Release_x64");
			Fix(logger, path + "bin\\x86\\Release", path + ".packages\\Release_x86");
			logger?.Info("finish!");
		}

		private static void Fix(ILogger? logger, PathString sourcePath, PathString targetPath)
		{
			logger?.Info($"fixing {sourcePath} and copying it to {targetPath}...");
			string ver = VersionInfo.VersionString;
			if (ver.EndsWith(".0")) {
				ver = ver.Substring(0, ver.Length - 2);
			}
			RewriteAndCopyNuspecFile(logger, sourcePath, targetPath, ver, "ExapisSOP");
			RewriteAndCopyNuspecFile(logger, sourcePath, targetPath, ver, "ExapisSOP.DemoApp");
			RewriteAndCopyNuspecFile(logger, sourcePath, targetPath, ver, "ExapisSOP.NativeWrapper.Windows");
			RewriteAndCopyNuspecFile(logger, sourcePath, targetPath, ver, "ExapisSOP.Tools.PackageDependencyFixer");
			RewriteAndCopyNuspecFile(logger, sourcePath, targetPath, ver, "ExapisSOP.Utils");
		}

		private static void RewriteAndCopyNuspecFile(ILogger? logger, PathString sourcePath, PathString targetPath, string ver, string name)
		{
			var src = sourcePath + $"{name}.{ver}.nupkg";
			var dst = targetPath + $"{name}.{ver}.nupkg";
			logger?.Info($"rewriting: {src}");
			using (var nupkgf = OpenNupkgFile(src))
			using (var stream = OpenNuspecFile(nupkgf, $"{name}.nuspec")) {
				var xd = new XmlDocument();
				xd.Load(stream);
				var dep = xd["package"]["metadata"]["dependencies"];
				dep.RemoveAll();
				var group1 = CreateGroupElement(xd, dep, ".NETFramework4.8");
				var group2 = CreateGroupElement(xd, dep, ".NETCoreApp3.1");
				switch (name) {
				case "ExapisSOP":
					break;
				case "ExapisSOP.DemoApp":
					CreateDependencyElement(xd, group1, group2, ver, "ExapisSOP");
					CreateDependencyElement(xd, group1, group2, ver, "ExapisSOP.NativeWrapper.Windows");
					CreateDependencyElement(xd, group1, group2, ver, "ExapisSOP.Utils");
					break;
				case "ExapisSOP.NativeWrapper.Windows":
					CreateDependencyElement(xd, group1, group2, ver, "ExapisSOP");
					break;
				case "ExapisSOP.Tools.PackageDependencyFixer":
					CreateDependencyElement(xd, null,   group2, "4.3.0", "System.IO.Compression");
					CreateDependencyElement(xd, group1, group2, ver,     "ExapisSOP");
					CreateDependencyElement(xd, group1, group2, ver,     "ExapisSOP.NativeWrapper.Windows");
					break;
				case "ExapisSOP.Utils":
					CreateDependencyElement(xd, group1, group2, ver, "ExapisSOP");
					break;
				}
				stream.Position = 0;
				stream.SetLength(0);
				xd.Save(stream);
			}
			logger?.Info($"copying from {src} to {dst}...");
			File.Copy(src, dst, true);
		}

		private static ZipArchive OpenNupkgFile(PathString zipfile)
		{
			return ZipFile.Open(zipfile, ZipArchiveMode.Update);
		}

		private static Stream OpenNuspecFile(ZipArchive nupkgfile, string entryname)
		{
			return nupkgfile.GetEntry(entryname).Open();
		}

		private static XmlElement CreateGroupElement(XmlDocument doc, XmlNode dep, string runtime)
		{
			var group = doc.CreateElement("group");
			var attr  = doc.CreateAttribute("targetFramework");
			attr.Value = runtime;
			group.Attributes.Append(attr);
			dep.AppendChild(group);
			return group;
		}

		private static void CreateDependencyElement(XmlDocument doc, XmlElement? group1, XmlElement? group2, string ver, string name)
		{
			var depEntry = doc.CreateElement("dependency");
			var attr_id  = doc.CreateAttribute("id");
			var attr_ver = doc.CreateAttribute("version");
			var attr_x   = doc.CreateAttribute("exclude");
			attr_id .Value = name;
			attr_ver.Value = ver;
			attr_x  .Value = "Build,Analyzers";
			depEntry.Attributes.Append(attr_id);
			depEntry.Attributes.Append(attr_ver);
			depEntry.Attributes.Append(attr_x);
			group1?.AppendChild(depEntry);
			group2?.AppendChild(depEntry.Clone());
		}

#else

		internal static void Fix(ILogger? logger, IContext context)
		{
			logger?.Info($"Cannot run on this platform: {VersionInfo.SystemType}.");
		}

#endif

	}
}

