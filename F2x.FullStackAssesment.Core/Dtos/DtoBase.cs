using System.Runtime.Serialization;

namespace F2xFullStackAssesment.Core.Dtos
{
    [DataContract]
    public class DtoBase<TId>
        where TId : struct
    {
        [DataMember(Name = "id")]
        public TId Id { get; set; }
    }
}
