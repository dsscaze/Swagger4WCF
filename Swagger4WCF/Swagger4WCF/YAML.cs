using Mono.Cecil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Swagger4WCF
{
    static public partial class YAML
    {
        public static List<AssemblyDefinition> _assembliesAuxiliares;

        static public IEnumerable<YAML.Document> Generate(AssemblyDefinition assembly, Documentation documentation)
        {
            foreach (var _type in assembly.MainModule.Types.Where(_Type => _Type.IsInterface && _Type.GetCustomAttribute<ServiceContractAttribute>() != null))
            {
                if (_type.FullName == _type.Module.Name.Replace(".dll", "") + "." + _type.Name)
                {
                    yield return Document.Generate(_type, documentation);
                }
            }
        }
    }
}
