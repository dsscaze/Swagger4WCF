using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Swagger4WCF.Demo.Models
{
    /// <summary>
    /// Person
    /// </summary>
    [DataContract]
    public class Person
    {
        /// <summary>
        /// First name
        /// </summary>
        [DataMember]
        public string Firstname { get; set; }

        /// <summary>
        /// Last name
        /// </summary>
        [DataMember]
        public string Lastname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<Enderecos> Enderecos { get; set; }

        /// <summary>
        /// Principal
        /// </summary>
        public Enderecos EnderecoPrincipal { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        [DataMember]
        public Gender Gender { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Enderecos
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Rua { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Bairro { get; set; }
    }

}
