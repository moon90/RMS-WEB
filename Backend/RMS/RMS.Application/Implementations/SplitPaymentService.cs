using AutoMapper;
using RMS.Application.DTOs.SplitPaymentDTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Entities;
using RMS.Infrastructure.IRepositories;
using System.Threading.Tasks;

namespace RMS.Application.Implementations
{
    public class SplitPaymentService : ISplitPaymentService
    {
        private readonly ISplitPaymentRepository _splitPaymentRepository;
        private readonly IMapper _mapper;

        public SplitPaymentService(ISplitPaymentRepository splitPaymentRepository, IMapper mapper)
        {
            _splitPaymentRepository = splitPaymentRepository;
            _mapper = mapper;
        }

        public async Task<SplitPaymentDto> CreateSplitPaymentAsync(CreateSplitPaymentDto dto)
        {
            var splitPayment = _mapper.Map<SplitPayment>(dto);
            splitPayment.CreatedOn = DateTime.UtcNow;
            var createdSplitPayment = await _splitPaymentRepository.AddAsync(splitPayment);
            return _mapper.Map<SplitPaymentDto>(createdSplitPayment);
        }
    }
}
