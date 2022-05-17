using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Mobile.Visao.Output
{
    public class AppCitiesOutput : BaseApiOutput
    {
        public AppCitiesOutput(string msg) : base(msg) { }
        public AppCitiesOutput(List<string> cities) : base(true) => Cities = cities;
        public List<string> Cities { get; set; }
    }
}
