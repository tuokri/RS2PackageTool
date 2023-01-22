using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using UELib;
using log4net;
using log4net.Appender;
using UELib.Core;
using UELib.Types;

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

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (!Debugger.IsAttached)
            {
                //     AppDomain newDomain = AppDomain.CreateDomain("My Error Reporter");
                //     newDomain.ExecuteAssembly(
                //         "ErrorReporter.exe",
                //         new Evidence(),
                //         new String[]
                //         {
                //             Assembly.GetExecutingAssembly().GetName().Version.ToString(4),
                //             e.ExceptionObject.ToString()
                //         });
                //
            }

            Log.ErrorFormat("error: {0}: {1}", sender, e);
        }

        private static void ProcessPackages(string[] args)
        {
            if (UnrealConfig.VariableTypes == null)
            {
                UnrealConfig.VariableTypes = new Dictionary<string, Tuple<string, PropertyType>>();
            }

            UnrealConfig.VariableTypes.Add(
                "Materials",
                new Tuple<string, PropertyType>("Engine.StaticMeshComponent", PropertyType.ObjectProperty));

            UnrealConfig.VariableTypes.Add(
                "SphereElems",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.StructProperty));
            UnrealConfig.VariableTypes.Add(
                "BoxElems",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.StructProperty));
            UnrealConfig.VariableTypes.Add(
                "SphylElems",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.StructProperty));
            UnrealConfig.VariableTypes.Add(
                "ConvexElems",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.StructProperty));

            UnrealConfig.VariableTypes.Add(
                "VertexData",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.Vector));
            UnrealConfig.VariableTypes.Add(
                "PermutedVertexData",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.Plane));
            UnrealConfig.VariableTypes.Add(
                "FaceTriData",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.IntProperty));
            UnrealConfig.VariableTypes.Add(
                "EdgeDirections",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.Vector));
            UnrealConfig.VariableTypes.Add(
                "FaceNormalDirections",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.Vector));
            UnrealConfig.VariableTypes.Add(
                "FacePlaneData",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.Plane));
            UnrealConfig.VariableTypes.Add(
                "ElemBox",
                new Tuple<string, PropertyType>("Engine.KMeshProps", PropertyType.Box));

            UnrealConfig.VariableTypes.Add(
                "Layers",
                new Tuple<string, PropertyType>("Engine.Terrain", PropertyType.StringProperty));
            UnrealConfig.VariableTypes.Add(
                "TerrainComponents",
                new Tuple<string, PropertyType>("Engine.Terrain", PropertyType.ObjectProperty));

            // var commonPackages = LoadCommonPackages();

            var filePath = args[0];
            Log.InfoFormat("reading           {0}", filePath);

            var fileInfo = new FileInfo(filePath);
            Log.InfoFormat("Length:           {0}", fileInfo.Length);
            Log.InfoFormat("CreationTimeUtc:  {0:o}", fileInfo.CreationTimeUtc);
            Log.InfoFormat("LastWriteTimeUtc: {0:o}", fileInfo.LastWriteTimeUtc);

            var package = UnrealLoader.LoadPackage(filePath);

            // commonPackages.ForEach(p => RegisterTypes(p, package));

            package.InitializePackage();

            Log.InfoFormat("FullPackageName:            {0}", package.FullPackageName);
            Log.InfoFormat("PackageName:                {0}", package.PackageName);
            Log.InfoFormat("PackageDirectory:           {0}", package.PackageDirectory);
            Log.InfoFormat("GetBufferSize():            {0}", package.GetBufferSize());
            Log.InfoFormat("GetBufferPosition():        {0}", package.GetBufferPosition());
            Log.InfoFormat("Version:                    {0}", package.Version);
            Log.InfoFormat("LicenseeVersion:            {0}", package.LicenseeVersion);
            Log.InfoFormat("GUID.Summary:               {0}", package.Summary.Guid);
            Log.InfoFormat("CookerVersion.Summary:      {0}", package.Summary.CookerVersion);
            Log.InfoFormat("CompressionFlags.Summary:   {0}", package.Summary.CompressionFlags);
            Log.InfoFormat("EngineVersion.Summary:      {0}", package.Summary.EngineVersion);
            Log.InfoFormat("PackageFlags.Summary:       {0}", package.Summary.PackageFlags);
            Log.InfoFormat("HeaderSize.Summary:         {0}", package.Summary.HeaderSize);
            Log.InfoFormat("Summary.NameCount:          {0}", package.Summary.NameCount);
            Log.InfoFormat("Summary.NameOffset:         {0}", package.Summary.NameOffset);
            Log.InfoFormat("Summary.ImportCount:        {0}", package.Summary.ImportCount);
            Log.InfoFormat("Summary.ImportOffset:       {0}", package.Summary.ImportOffset);
            Log.InfoFormat("Summary.ExportCount:        {0}", package.Summary.ExportCount);
            Log.InfoFormat("Summary.ExportOffset:       {0}", package.Summary.ExportOffset);
            Log.InfoFormat("Summary.DependsOffset:      {0}", package.Summary.DependsOffset);
            Log.InfoFormat("S.Imp.Exp.GuidsOffset:      {0}", package.Summary.ImportExportGuidsOffset);
            Log.InfoFormat("Summary.ImportGuidsCount:   {0}", package.Summary.ImportGuidsCount);
            Log.InfoFormat("Summary.ExportGuidsCount:   {0}", package.Summary.ExportGuidsCount);
            Log.InfoFormat("S.ThumbnailTableOffset:     {0}", package.Summary.ThumbnailTableOffset);
            Log.InfoFormat("S.GatherableTextDataCount:  {0}", package.Summary.GatherableTextDataCount);
            Log.InfoFormat("S.GatherableTextDataOffset: {0}", package.Summary.GatherableTextDataOffset);
            Log.InfoFormat("S.StringAssetRef.Count:     {0}", package.Summary.StringAssetReferencesCount);
            Log.InfoFormat("S.StringAssetRef.Offset:    {0}", package.Summary.StringAssetReferencesOffset);
            Log.InfoFormat("S.SearchableNamesOffset:    {0}", package.Summary.SearchableNamesOffset);

            package.InitializeImportObjects();
            var imports = package.Imports;
            Log.Info("*** IMPORTS: ***");
            Log.InfoFormat("imports.Count: {0}", imports.Count);
            imports.ForEach(PrintImportTableItem);
            // imports.ForEach(LoadImportPackages);

            package.InitializeExportObjects();
            var exports = package.Exports;
            Log.Info("*** EXPORTS: ***");
            Log.InfoFormat("exports.Count: {0}", exports.Count);
            exports.ForEach(PrintExportTableItem);

            foreach (var bmd in package.BinaryMetaData.Fields)
            {
                if (bmd.Field == "dependsMap")
                {
                    Log.InfoFormat("*** dependsMap: ***");
                    Log.InfoFormat("{0}", bmd.Decompile());
                }
            }

            var outStream = new UPackageStream("VNTE-IwoJimaOut.roe", FileMode.Create, FileAccess.Write);
            outStream.PostInit(package);
            package.Serialize(outStream);
        }

        private static void Main(string[] args)
        {
            // var logStream = new LogStream(Log);
            // Console.SetOut(logStream.Writer);

            // AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ProcessPackages(args);
            // try
            // {
            //     ProcessPackages(args);
            // }
            // catch (Exception e)
            // {
            //     Log.ErrorFormat("error: {0}", e);
            //     throw;
            // }
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
                if (classType == null) continue;

                Log.InfoFormat("adding class type {0} {1}", className, classType);
                targetPackage.AddClassType(className, classType);
            }
        }

        private static List<UnrealPackage> LoadCommonPackages()
        {
            var packages = new List<UnrealPackage>
            {
                UnrealLoader.LoadPackage(
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
            Log.InfoFormat("Outer           : {0}", importTableItem.Outer);
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
            Log.InfoFormat("O.Outer?.Name   : {0}", o.Outer?.Name);
            Log.InfoFormat("O.Outer         : {0}", o.Outer);
        }

        private static void PrintExportTableItem(UExportTableItem exportTableItem)
        {
            var obj = exportTableItem.Object;

            Log.Info("... ... ... ... ... ... ... ...");

            Log.InfoFormat("exportTableItem : {0}", exportTableItem);
            Log.InfoFormat("ClassName       : {0}", exportTableItem.Class);
            Log.InfoFormat("Index           : {0}", exportTableItem.Index);
            Log.InfoFormat("Offset          : {0}", exportTableItem.Offset);
            Log.InfoFormat("Size            : {0}", exportTableItem.Size);
            // Log.InfoFormat("OuterName       : {0}", exportTableItem.OuterName);
            Log.InfoFormat("Outer           : {0}", exportTableItem.Outer);
            Log.InfoFormat("OuterIndex      : {0}", exportTableItem.OuterIndex);
            Log.InfoFormat("OuterTable      : {0}", exportTableItem.OuterTable);
            Log.InfoFormat("Owner           : {0}", exportTableItem.Owner);
            Log.InfoFormat("ObjectName      : {0}", exportTableItem.ObjectName);
            // Log.InfoFormat("SuperName       : {0}", exportTableItem.SuperName);
            Log.InfoFormat("Super           : {0}", exportTableItem.Super);
            Log.InfoFormat("GetBufferPos.() : {0}", exportTableItem.GetBufferPosition());
            Log.InfoFormat("GetBufferId()   : {0}", exportTableItem.GetBufferId());
            Log.InfoFormat("GetBufferSize() : {0}", exportTableItem.GetBufferSize());

            // obj.BeginDeserializing();
            Log.InfoFormat("Object          : {0}", obj);
            Log.InfoFormat("Class           : {0}", obj.Class);
            Log.InfoFormat("Name            : {0}", obj.Name);
            Log.InfoFormat("Outer           : {0}", obj.Outer);
            Log.InfoFormat("Properties      : {0}", obj.Properties);
            Log.InfoFormat("Outer.?Name     : {0}", obj.Outer?.Name);
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
                        var loadedPackage = UnrealLoader.LoadPackage(pgkToLoad);

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
