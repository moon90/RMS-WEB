using AutoMapper;
using RMS.Application.DTOs.AlertDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IMapper _mapper;

        public AlertService(IAlertRepository alertRepository, IMapper mapper)
        {
            _alertRepository = alertRepository;
            _mapper = mapper;
        }

        public async Task<AlertDto> CreateAlertAsync(CreateAlertDto dto)
        {
            var alert = _mapper.Map<Alert>(dto);
            alert.AlertDate = DateTime.UtcNow;
            var createdAlert = await _alertRepository.AddAsync(alert);
            return _mapper.Map<AlertDto>(createdAlert);
        }

        public async Task<IEnumerable<AlertDto>> GetAlertsAsync()
        {
            var alerts = await _alertRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AlertDto>>(alerts);
        }

        public async Task MarkAsAcknowledgedAsync(int id)
        {
            var alert = await _alertRepository.GetByIdAsync(id);
            if (alert != null)
            {
                alert.IsAcknowledged = true;
                await _alertRepository.UpdateAsync(alert);
            }
        }
    }
}
