using ELittoral.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Services
{
    public class AnalysisModelService
    {
        public async Task<IEnumerable<AnalysisModel>> GetDataAsync()
        {
            await Task.CompletedTask;
            var data = new List<AnalysisModel>();

            var analysis = new AnalysisModel
            {
                CreatedAt = "05/06/2017",
                State = "complete",
                Message = null,
                Total = "6",
                Current = "6",
                Result = "0.9876"
            };

            analysis.Results = new List<AnalysisResultModel>();
            analysis.Results.Add(new AnalysisResultModel
            {
                Result = "0.0254",
                Image = new Uri("http://vps361908.ovh.net/dev/elittoral/api/results/8/content")
            });
            analysis.Results.Add(new AnalysisResultModel
            {
                Result = "0.001",
                Image = new Uri("http://vps361908.ovh.net/dev/elittoral/api/results/9/content")
            });
            analysis.Results.Add(new AnalysisResultModel
            {
                Result = "0.33",
                Image = new Uri("http://vps361908.ovh.net/dev/elittoral/api/results/10/content")
            });
            data.Add(analysis);

            data.Add(new AnalysisModel
            {
                CreatedAt = "05/06/2017",
                State = "error",
                Message = "Une erreur est survenue",
                Total = "6",
                Current = "3",
                Result = "0.99"
            });

            return data;
        }
    }
}
