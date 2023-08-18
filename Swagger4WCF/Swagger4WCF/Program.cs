﻿using Mono.Cecil;
using System;
using System.IO;
using System.Linq;
using System.Text;
using YamlDotNet.Serialization;

namespace Swagger4WCF
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length==0)
            {
                args = new string[5];
                //Console.WriteLine("Please provide path to sln");
                //args[0]= Console.ReadLine();
                //Console.WriteLine("Please provide path to project file");
                //args[1] = Console.ReadLine();
                //Console.WriteLine("Please provide configuration Release or Debug");
                //args[2] = Console.ReadLine();
                //Console.WriteLine("Please provide Solution Platform (Any CPU)");
                //args[3] = Console.ReadLine();
                //Console.WriteLine("Please provide path to dll");
                //args[4] = Console.ReadLine();

                
                args[0] = @"C:\Users\dssca\Documents\GitHub\Swagger4WCF\Swagger4WCF\Swagger4WCF.sln";
                
                args[1] = @"C:\Users\dssca\Documents\GitHub\Swagger4WCF\Swagger4WCF\Swagger4WCF.Demo\Swagger4WCF.Demo.csproj";

                args[2] = "debug";

                args[3] = "any";
                
                args[4] = @"C:\Users\dssca\Documents\GitHub\Swagger4WCF\Swagger4WCF\Swagger4WCF.Demo\bin\Debug\";
            }

            try
            {
                ExecSwaggerWCF(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static void ExecSwaggerWCF(string[] args)
        {
            var _directory = Path.GetDirectoryName(args[4]);
            var _name = Path.GetFileNameWithoutExtension(args[1]);
            var _resolver = new DefaultAssemblyResolver();
            _resolver.AddSearchDirectory(_directory);
            var _domain = Directory.EnumerateFiles(_directory, "*.dll").Select(_File =>
            {
                try
                {
                    return new
                    {
                        Assembly = AssemblyDefinition.ReadAssembly(_File,
                            new ReaderParameters()
                            {
                                AssemblyResolver = _resolver,
                                ReadSymbols = true,
                                ReadingMode = ReadingMode.Immediate
                            }),
                        Location = _File
                    };
                }
                catch { return null; }
            }).Where(_Entry => _Entry != null).ToArray();

            foreach (var _entry in _domain)
            {
                foreach (var _document in YAML.Generate(_entry.Assembly, Documentation.Load(_entry.Location, _entry.Assembly)))
                {
                    var _location = $@"{ _directory }\{ _entry.Assembly.Name.Name }.{_document.Type.Name }.yaml";
                    using (var _writer = new StreamWriter(_location, false, Encoding.UTF8))
                    {
                        _writer.Write(_document.ToString());
                        Console.WriteLine($"{ _name } -> { _location }");
                    }

                    var r = new StringReader(_document.ToString());
                    var deserializer = new Deserializer();
                    var yamlObject = deserializer.Deserialize(r);

                    var serializer = new SerializerBuilder()
                                            .JsonCompatible()
                                            .Build();

                    var json = serializer.Serialize(yamlObject);

                    var _locationJson = $@"{ _directory }\{ _entry.Assembly.Name.Name }.{_document.Type.Name }.json";
                    using (var _writer = new StreamWriter(_locationJson, false, Encoding.UTF8))
                    {
                        _writer.Write(json);
                        Console.WriteLine($"{ _name } -> { _locationJson }");
                    }
                }
            }
        }
    }
}
