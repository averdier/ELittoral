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
                Id = 8,
                Analysis = analysis,
                CreatedAt = "25/04/2017",
                Result = "0.0254",
                Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/results/8/content"),
                MinuendRessource = new ReconRessourceModel
                {
                    CreatedAt = "27/09/2017",
                    Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/15/thumbnail"),
                    Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/15/content")
                },
                SubtrahendRessource = new ReconRessourceModel
                {
                    CreatedAt = "25/04/2017",
                    Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/9/thumbnail"),
                    Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/9/content")
                }
            });
            analysis.Results.Add(new AnalysisResultModel
            {
                Id = 9,
                Analysis = analysis,
                CreatedAt = "17/03/2015",
                Result = "0.001",
                Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/results/9/content"),
                MinuendRessource = new ReconRessourceModel
                {
                    CreatedAt = "19/10/2016",
                    Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/16/thumbnail"),
                    Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/16/content")
                },
                SubtrahendRessource = new ReconRessourceModel
                {
                    CreatedAt = "13/03/2015",
                    Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/10/thumbnail"),
                    Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/10/content")
                }
            });
            analysis.Results.Add(new AnalysisResultModel
            {
                Id = 10,
                Analysis = analysis,
                CreatedAt = "17/03/2015",
                Result = "0.33",
                Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/results/10/content"),
                MinuendRessource = new ReconRessourceModel
                {
                    CreatedAt = "17/03/2016",
                    Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/17/thumbnail"),
                    Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/17/content")
                },
                SubtrahendRessource = new ReconRessourceModel
                {
                    CreatedAt = "17/03/2015",
                    Thumbnail = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/11/thumbnail"),
                    Content = new Uri("http://vps361908.ovh.net/dev/elittoral/api/resources/11/content")
                }
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
