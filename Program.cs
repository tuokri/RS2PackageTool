using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UELib;
using UELib.Core;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace RS2PackageTool
{
    internal static class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        private static readonly List<string> CNames =
            new List<string> { "StaticMeshActor", "StaticMeshComponent" };

        private static void Main(string[] args)
        {
            var package = UnrealLoader.LoadPackage(args[0], System.IO.FileAccess.Read);
            log.Info($"Version: {package.Summary.Version}\n");

            package.InitializePackage();
            package.InitializeExportObjects();
            package.InitializeImportObjects();

            var imports = package.Imports;
            log.InfoFormat("imports.Count: {0}\n", imports.Count);
            imports.ForEach(PrintImportTableItem);

            var exports = package.Exports;
            log.InfoFormat("exports.Count: {0}\n", exports.Count);
            exports.ForEach(PrintExportTableItem);

            // Console.ReadLine();
        }

        private static void PrintImportTableItem(UImportTableItem importTableItem)
        {
            // if (!CNames.Contains(importTableItem.ClassName))
            // {
            //     return;
            // }

            var obj = importTableItem.Object;

            log.Info("... ... ... ... ... ... ... ...");

            log.InfoFormat("importTableItem : {0}", importTableItem);
            log.InfoFormat("PackageName     : {0}", importTableItem.PackageName);
            log.InfoFormat("ClassName       : {0}", importTableItem.ClassName);
            log.InfoFormat("Index           : {0}", importTableItem.Index);
            log.InfoFormat("Offset          : {0}", importTableItem.Offset);
            log.InfoFormat("Size            : {0}", importTableItem.Size);
            log.InfoFormat("Outer           : {0}", importTableItem.Outer);
            log.InfoFormat("Owner           : {0}", importTableItem.Owner);
            log.InfoFormat("ObjectName      : {0}", importTableItem.ObjectName);

            obj.BeginDeserializing();
            log.InfoFormat("Object          : {0}", obj);
            log.InfoFormat("Class           : {0}", obj.Class);
            log.InfoFormat("Properties      : {0}", obj.Properties);
            log.InfoFormat("GetClassName()  : {0}", obj.GetClassName());
            log.InfoFormat("Decompile()     : {0}", obj.Decompile());
        }

        private static void PrintExportTableItem(UExportTableItem exportTableItem)
        {
            var obj = exportTableItem.Object;

            log.Info("... ... ... ... ... ... ... ...");

            log.InfoFormat("exportTableItem : {0}", exportTableItem);
            log.InfoFormat("PackageGuid     : {0}", exportTableItem.PackageGuid);
            log.InfoFormat("Class           : {0}", exportTableItem.Class);
            log.InfoFormat("Index           : {0}", exportTableItem.Index);
            log.InfoFormat("Offset          : {0}", exportTableItem.Offset);
            log.InfoFormat("Size            : {0}", exportTableItem.Size);
            log.InfoFormat("Outer           : {0}", exportTableItem.Outer);
            log.InfoFormat("Owner           : {0}", exportTableItem.Owner);
            log.InfoFormat("ObjectName      : {0}", exportTableItem.ObjectName);

            obj.BeginDeserializing();
            log.InfoFormat("Object          : {0}", obj);
            log.InfoFormat("Class           : {0}", obj.Class);
            log.InfoFormat("Properties      : {0}", obj.Properties);
            log.InfoFormat("GetClassName()  : {0}", obj.GetClassName());
            log.InfoFormat("Decompile()     : {0}", obj.Decompile());
        }
    }
}
