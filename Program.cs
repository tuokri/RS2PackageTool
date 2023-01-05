using System;
using System.Collections.Generic;
using System.IO;
using UELib;
using log4net;
using UELib.Core;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace RS2PackageTool
{
    internal static class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        /*
        private static readonly List<string> CNames =
            new List<string> { "StaticMeshActor", "StaticMeshComponent" };
        */

        private static void Main(string[] args)
        {
            var commonPackages = LoadCommonPackages();

            var filePath = args[0];
            Log.InfoFormat("reading           {0}", filePath);

            var fileInfo = new FileInfo(filePath);
            Log.InfoFormat("Length:           {0}", fileInfo.Length);
            Log.InfoFormat("CreationTimeUtc:  {0:o}", fileInfo.CreationTimeUtc);
            Log.InfoFormat("LastWriteTimeUtc: {0:o}", fileInfo.LastWriteTimeUtc);

            var package = UnrealLoader.LoadCachedPackage(filePath);

            commonPackages.ForEach(p => RegisterTypes(p, package));

            package.InitializePackage();

            Log.InfoFormat("FullPackageName:            {0}", package.FullPackageName);
            Log.InfoFormat("PackageName:                {0}", package.PackageName);
            Log.InfoFormat("PackageDirectory:           {0}", package.PackageDirectory);
            Log.InfoFormat("GetBufferSize():            {0}", package.GetBufferSize());
            Log.InfoFormat("GetBufferPosition():        {0}", package.GetBufferPosition());
            Log.InfoFormat("HeaderSize:                 {0}", package.HeaderSize);
            Log.InfoFormat("Version:                    {0}", package.Version);
            Log.InfoFormat("LicenseeVersion:            {0}", package.LicenseeVersion);
            Log.InfoFormat("PackageFlags:               {0}", package.PackageFlags);
            Log.InfoFormat("EngineVersion:              {0}", package.EngineVersion);
            Log.InfoFormat("GUID:                       {0}", package.GUID);
            Log.InfoFormat("CookerVersion:              {0}", package.CookerVersion);
            Log.InfoFormat("CompressionFlags:           {0}", package.CompressionFlags);
            Log.InfoFormat("Summary.NamesCount:         {0}", package.Summary.NamesCount);
            Log.InfoFormat("Summary.NamesOffset:        {0}", package.Summary.NamesOffset);
            Log.InfoFormat("Summary.ExportCount:        {0}", package.Summary.ExportsCount);
            Log.InfoFormat("Summary.ExportOffset:       {0}", package.Summary.ExportsOffset);
            Log.InfoFormat("Summary.ImportCount:        {0}", package.Summary.ImportsCount);
            Log.InfoFormat("Summary.ImportOffset:       {0}", package.Summary.ImportsOffset);
            Log.InfoFormat("Summary.DependsOffset:      {0}", package.Summary.DependsOffset);

            package.InitializeImportObjects();
            var imports = package.Imports;
            Log.Info("*** IMPORTS: ***");
            Log.InfoFormat("imports.Count: {0}", imports.Count);
            imports.ForEach(PrintImportTableItem);
            imports.ForEach(LoadImportPackages);

            package.InitializeExportObjects();
            var exports = package.Exports;
            Log.Info("*** EXPORTS: ***");
            Log.InfoFormat("exports.Count: {0}", exports.Count);
            // exports.ForEach(PrintExportTableItem);

            var outStream = new UPackageStream("VNTE-IwoJimaOut.roe", FileMode.Create, FileAccess.Write);
            outStream.PostInit(package);
            package.Serialize(outStream);
        }

        private static void RegisterTypes(UnrealPackage package, UnrealPackage targetPackage)
        {
            // foreach (var export in package.Exports)
            foreach (var obj in package.Objects)
            {
                var type1 = obj.GetType();
                var type2 = obj.GetFriendlyType();

                if (obj.GetType() != typeof(UClass))
                {
                    continue;
                }

                var className = obj.GetFriendlyType();
                if (targetPackage.HasClassType(className)) continue;

                // var classType = package.GetClassType(className);

                var classType = package.GetClassType(className);
                Log.InfoFormat("adding class type {0} {1}", className, classType);
                targetPackage.AddClassType(className, classType);
            }
        }

        private static List<UnrealPackage> LoadCommonPackages()
        {
            var packages = new List<UnrealPackage>
            {
                UnrealLoader.LoadCachedPackage(
                    "O:\\SteamLibrary\\steamapps\\common\\Rising Storm 2\\ROGame\\BrewedPC\\AkAudio.u")
            };

            foreach (var package in packages)
            {
                try
                {
                    package.InitializePackage();
                    package.InitializeImportObjects();
                    package.InitializeExportObjects();
                }
                catch (Exception e)
                {
                    Log.WarnFormat("failed to initialize package {0}: {1}", package, e.Message);
                }
            }

            return packages;
        }

        private static void PrintImportTableItem(UImportTableItem importTableItem)
        {
            Log.Info("... ... ... ... ... ... ... ...");

            Log.InfoFormat("importTableItem : {0}", importTableItem);
            Log.InfoFormat("PackageName     : {0}", importTableItem.PackageName);
            Log.InfoFormat("ClassName       : {0}", importTableItem.ClassName);
            Log.InfoFormat("Index           : {0}", importTableItem.Index);
            Log.InfoFormat("Offset          : {0}", importTableItem.Offset);
            Log.InfoFormat("Size            : {0}", importTableItem.Size);
            Log.InfoFormat("OuterName       : {0}", importTableItem.OuterName);
            Log.InfoFormat("OuterIndex      : {0}", importTableItem.OuterIndex);
            Log.InfoFormat("OuterTable      : {0}", importTableItem.OuterTable);
            Log.InfoFormat("Owner           : {0}", importTableItem.Owner);
            Log.InfoFormat("ObjectName      : {0}", importTableItem.ObjectName);

            var o = importTableItem.Object;
            if (o == null)
            {
                return;
            }

            // o.BeginDeserializing();
            Log.InfoFormat("Object (O)      : {0}", o);
            Log.InfoFormat("O.Package       : {0}", o.Package);
            Log.InfoFormat("O.P.PackageName : {0}", o.Package.PackageName);
            Log.InfoFormat("O.P.Pkg.Dir.    : {0}", o.Package.PackageDirectory);
            Log.InfoFormat("O.Name          : {0}", o.Name);
            Log.InfoFormat("O.GetOuterNa.() : {0}", o.GetOuterName());
            Log.InfoFormat("O.Outer         : {0}", o.Outer);
        }

        private static void PrintExportTableItem(UExportTableItem exportTableItem)
        {
            var obj = exportTableItem.Object;

            Log.Info("... ... ... ... ... ... ... ...");

            Log.InfoFormat("exportTableItem : {0}", exportTableItem);
            Log.InfoFormat("ClassName       : {0}", exportTableItem.ClassName);
            Log.InfoFormat("Index           : {0}", exportTableItem.Index);
            Log.InfoFormat("Offset          : {0}", exportTableItem.Offset);
            Log.InfoFormat("Size            : {0}", exportTableItem.Size);
            Log.InfoFormat("OuterName       : {0}", exportTableItem.OuterName);
            Log.InfoFormat("OuterIndex      : {0}", exportTableItem.OuterIndex);
            Log.InfoFormat("OuterTable      : {0}", exportTableItem.OuterTable);
            Log.InfoFormat("Owner           : {0}", exportTableItem.Owner);
            Log.InfoFormat("ObjectName      : {0}", exportTableItem.ObjectName);
            Log.InfoFormat("SuperName       : {0}", exportTableItem.SuperName);
            Log.InfoFormat("GetBufferPos.() : {0}", exportTableItem.GetBufferPosition());
            Log.InfoFormat("GetBufferId()   : {0}", exportTableItem.GetBufferId());
            Log.InfoFormat("GetBufferSize() : {0}", exportTableItem.GetBufferSize());

            // obj.BeginDeserializing();
            Log.InfoFormat("Object          : {0}", obj);
            Log.InfoFormat("Class           : {0}", obj.Class);
            Log.InfoFormat("Name            : {0}", obj.Name);
            Log.InfoFormat("Outer           : {0}", obj.Outer);
            Log.InfoFormat("Properties      : {0}", obj.Properties);
            Log.InfoFormat("GetOuterName()  : {0}", obj.GetOuterName());
            Log.InfoFormat("GetBufferId()   : {0}", obj.GetBufferId());
            Log.InfoFormat("GetBufferPos.() : {0}", obj.GetBufferPosition());
            Log.InfoFormat("GetBufferSize() : {0}", obj.GetBufferSize());
            Log.InfoFormat("GetClassName()  : {0}", obj.GetClassName());
            Log.InfoFormat("GetType()       : {0}", obj.GetType());
            Log.InfoFormat("Decompile()     : {0}", obj.Decompile());
        }

        private static void LoadImportPackages(UImportTableItem importTableItem)
        {
            var outer = importTableItem.Object.Outer;
            var packageDir = importTableItem.Object.Package.PackageDirectory;
            var pgkToLoad = "";

            Log.InfoFormat("loading import packages for: {0}", importTableItem);

            try
            {
                while (outer != null)
                {
                    Log.InfoFormat("outer: {0}", outer);

                    if (outer.Outer == null)
                    {
                        pgkToLoad = packageDir + "\\" + outer.Name + ".u";
                        var loadedPackage = UnrealLoader.LoadCachedPackage(pgkToLoad);

                        try
                        {
                            loadedPackage.InitializePackage();
                            loadedPackage.InitializeImportObjects();
                            loadedPackage.InitializeExportObjects();
                        }
                        catch (Exception e)
                        {
                            Log.WarnFormat("failed to initialize package {0}: {1}", loadedPackage, e.Message);
                        }

                        Log.InfoFormat("loaded package: {0}", pgkToLoad);

                        // var o = -importTableItem.OuterIndex - 1;
                        // var outerIndex = importTableItem.OuterIndex;
                        // Log.InfoFormat("loading object: {0}[{1}]", loadedPackage.PackageName, outerIndex);
                        Log.InfoFormat("loaded object:  {0}",
                            loadedPackage.FindObject<UObject>(importTableItem.ObjectName, checkForSubclass: true));

                        break;
                    }

                    outer = outer.Outer;
                }
            }
            catch (IOException)
            {
                Log.ErrorFormat("error loading {0}", pgkToLoad);
            }
        }
    }
}
