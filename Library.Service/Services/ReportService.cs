using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Models.Report;
using Library.Service.Dtos.Report;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;

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

        public async Task<IEnumerable<AnnualReportRow>> GetAnnualReport(AnnualReportDto annualReportDto)
        {
            var result = await _genericRepository.GetAnnualReport(annualReportDto.ModelName, annualReportDto.Year);
            return result;
        }
    }
}
