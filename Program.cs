using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Metadata;
using UELib;
using UELib.Core;
using log4net;
using static UELib.BinaryMetaData;

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
            var package = UnrealLoader.LoadPackage(args[0]);

            package.InitializePackage();
            package.InitializeExportObjects();
            package.InitializeImportObjects();

            Log.InfoFormat("FullPackageName:            {0}", package.FullPackageName);
            Log.InfoFormat("PackageName:                {0}", package.PackageName);
            Log.InfoFormat("PackageDirectory:           {0}", package.PackageDirectory);
            Log.InfoFormat("GetBufferSize():            {0}", package.GetBufferSize());
            Log.InfoFormat("GetBufferPosition()         {0}", package.GetBufferPosition());
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

            var imports = package.Imports;
            Log.InfoFormat("imports.Count: {0}", imports.Count);
            imports.ForEach(PrintImportTableItem);

            var exports = package.Exports;
            Log.InfoFormat("exports.Count: {0}", exports.Count);
            exports.ForEach(PrintExportTableItem);

            /*
            var objects = package.Objects;
            log.InfoFormat("objects.Count: {0}", objects.Count);
            objects.ForEach(PrintObject);
            */

            // Console.ReadLine();
        }

        private static void PrintImportTableItem(UImportTableItem importTableItem)
        {
            // if (!CNames.Contains(importTableItem.ClassName))
            // {
            //     return;
            // }

            // var obj = importTableItem.Object;

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

            /*
            obj.BeginDeserializing();
            log.InfoFormat("Object          : {0}", obj);
            log.InfoFormat("Class           : {0}", obj.Class);
            log.InfoFormat("Properties      : {0}", obj.Properties);
            log.InfoFormat("GetClassName()  : {0}", obj.GetClassName());
            log.InfoFormat("Decompile()     : {0}", obj.Decompile());
            */
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
            Log.InfoFormat("GetBufferPos.() : {0}", exportTableItem.GetBufferPosition());
            Log.InfoFormat("GetBufferId()   : {0}", exportTableItem.GetBufferId());
            Log.InfoFormat("GetBufferSize() : {0}", exportTableItem.GetBufferSize());

            obj.BeginDeserializing();
            Log.InfoFormat("Object          : {0}", obj);
            Log.InfoFormat("Class           : {0}", obj.Class);
            Log.InfoFormat("Properties      : {0}", obj.Properties);
            Log.InfoFormat("GetBufferPos.() : {0}", obj.GetBufferPosition());
            Log.InfoFormat("GetClassName()  : {0}", obj.GetClassName());
            Log.InfoFormat("Decompile()     : {0}", obj.Decompile());
        }

        private static void PrintObject(UObject uObject)
        {
            Log.Info("... ... ... ... ... ... ... ...");

            Log.InfoFormat("uObject         : {0}", uObject);
            Log.InfoFormat("GetType()       : {0}", uObject.GetType());
            uObject.BeginDeserializing();
            Log.InfoFormat("Decompile()     : {0}", uObject.Decompile());
        }
    }
}
