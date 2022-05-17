using DTO.General.Base.Api.Output;

namespace DTO.Intra.Loan.Output
{
    public class IntraLoanUpsertOutput : BaseApiOutput
    {
        public IntraLoanUpsertOutput(string msg) : base(msg) { }
        public IntraLoanUpsertOutput(bool scs, string id) : base(scs) => Id = id;
        public string Id { get; set; }
    }
}
