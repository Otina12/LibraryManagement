using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Report;
using Library.Service.Dtos.Report;
using Library.Service.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository _genericRepository;

        public ReportService(IGenericRepository genericRepository)
        {
            _genericRepository = genericRepository;
        }

        public async Task<IEnumerable<PopularityReportRow>> GetPopularityReport(PopularityReportDto popularityReportDto)
        {
            var result = await _genericRepository.GetPopularityReport(popularityReportDto.ModelName, popularityReportDto.StartDate, popularityReportDto.EndDate);
            return result;
        }
    }
}
