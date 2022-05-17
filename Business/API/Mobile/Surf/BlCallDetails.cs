using DAO.DBConnection;
using DTO.Integration.Surf.Call.Input;
using DTO.Integration.Surf.Call.Output;
using DTO.Surf.Enum;
using DTO.Surf.Output.Charts;
using Services.Integration.Surf.Register.Customer;
using System;
using System.Linq;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Surf
{
    public class BlCallDetails
    {
        private SurfCallService SurfCallService;
        public BlCallDetails(XDataDatabaseSettings settings)
        {
            SurfCallService = new(settings);
        }

        public async Task<AppHistoryChartOutput> CallHistoryChartCurrentMonth(string msisdn)
        {
            if (string.IsNullOrEmpty(msisdn))
                return new AppHistoryChartOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "MSISDN não informado!"
                };

            var date = DateTime.Now;
            var year = date.Year;
            var month = date.Month;
            var history = await CallHistory(new SurfCallHistoryInput
            {
                Month = month.ToString(),
                Year = year.ToString(),
                MSISDN = msisdn
            }).ConfigureAwait(false);

            if (history == null)
                return new AppHistoryChartOutput
                {
                    CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                    Msg = "Informações do histórico de consumo não encontrados!"
                };

            if (!(history.Details?.Any() ?? false))
                return null;


            // Feito isso para mostrar os dados de todos os dias, mesmo que não tenha nenhum histórico para aquele dia
            var result = new AppHistoryChartOutput();
            for (var i = 1; i <= DateTime.DaysInMonth(date.Year, date.Month); i++)
            {
                result.Internet.Add(new BaseHistoryChart
                {
                    FullDate = new DateTime(date.Year, date.Month, i).ToString("dd/MM/yyyy"),
                    MinDate = i.ToString()
                });

                result.Call.Add(new BaseHistoryChart
                {
                    FullDate = new DateTime(date.Year, date.Month, i).ToString("dd/MM/yyyy"),
                    MinDate = i.ToString()
                });

                result.Sms.Add(new BaseHistoryChart
                {
                    FullDate = new DateTime(date.Year, date.Month, i).ToString("dd/MM/yyyy"),
                    MinDate = i.ToString()
                });
            }

            foreach (var item in history.Details)
            {
                _ = double.TryParse(item.TotalUsedBytes, out var bytes);
                _ = TimeSpan.TryParse(item.Duration, out var seconds);
                var fullDate = DateTimeExtension.StringToDate(item.Date);

                if (item.CallType == AppCallDataTypeEnum.Data)
                {
                    var dataInternet = result.Internet.FirstOrDefault(x => x.MinDate == fullDate.Day.ToString());
                    if (dataInternet == null)
                        continue;

                    dataInternet.Data = bytes.ConvertSize();
                }

                if (item.CallType == AppCallDataTypeEnum.Sms)
                {
                    var dataSms = result.Sms.FirstOrDefault(x => x.MinDate == fullDate.Day.ToString());
                    if (dataSms == null)
                        continue;

                    dataSms.Data++;
                }

                if (item.CallType == AppCallDataTypeEnum.Voice)
                {
                    var dataCall = result.Call.FirstOrDefault(x => x.MinDate == fullDate.Day.ToString());
                    if (dataCall == null)
                        continue;

                    dataCall.Data = seconds.TotalSeconds;
                }
            }

            return result;
        }

        public async Task<SurfCallHistoryOutput> CallHistory(SurfCallHistoryInput input) => input == null ?
            new SurfCallHistoryOutput
            {
                CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                Msg = "Requisição mal formada!"
            } : await SurfCallService.GetCallHistory(input).ConfigureAwait(false);
    }
}
