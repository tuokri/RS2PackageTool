using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UELib;

namespace RS2PackageTool
{
    internal static class Program
    {
        private static List<string> _cNames = new List<string> { "StaticMeshActor", "StaticMeshComponent" };

        private static void Main(string[] args)
        {
            var package = UnrealLoader.LoadPackage(args[0], System.IO.FileAccess.Read);
            Console.WriteLine($"Version: {package.Summary.Version}\n");

            package.InitializePackage();
            package.InitializeExportObjects();
            package.InitializeImportObjects();

            var imports = package.Imports;
            var someImports = imports.GetRange(0, 25);

            imports.ForEach(PrintImportTableItem);

            /*
            // var someObjects = package.Objects.GetRange(0, 5);
            foreach (var obj in package.Objects)
            {
                try
                {
                    obj.BeginDeserializing();

                    if (obj.Class.Name != "StaticMeshActor")
                    {
                        continue;
                    }

                    Console.WriteLine($"Name: {obj.Name}");
                    Console.WriteLine($"Class: {obj.Class?.Name}");
                    Console.WriteLine($"Outer: {obj.Outer}");

                    var output = obj.Decompile();
                    Console.WriteLine(output);

                    Console.WriteLine("");
                }
                catch (Exception ex)
                {
                    // Console.WriteLine(ex.GetType());
                }
            }
            // someObjects.ForEach(i => Console.WriteLine("{0.}{1}"));
            */
        }

        private static void PrintImportTableItem(UImportTableItem importTableItem)
        {
            if (!_cNames.Contains(importTableItem.ClassName))
            {
                return;
            }

            Console.WriteLine(importTableItem);
            Console.WriteLine("PackageName: {0}", importTableItem.PackageName);
            Console.WriteLine("ClassName: {0}", importTableItem.ClassName);
            Console.WriteLine("Index: {0}", importTableItem.Index);
            Console.WriteLine("Offset: {0}", importTableItem.Offset);
            Console.WriteLine("Size: {0}", importTableItem.Size);
            Console.WriteLine("Outer: {0}", importTableItem.Outer);
            Console.WriteLine("Object: {0}", importTableItem.Object);
            Console.WriteLine("Owner: {0}", importTableItem.Owner);
            Console.WriteLine("... ... ... ... ... ... ... ...");
        }
    }
}
