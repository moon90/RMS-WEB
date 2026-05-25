using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Application.DTOs;
using RMS.Application.Interfaces;
using RMS.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RMS.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchesController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _branchService.GetAllBranchesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _branchService.GetBranchByIdAsync(id);
            if (!result.IsSuccess) return NotFound(result);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "ROLE_CREATE")] // Only high-level admins can create branches
        public async Task<IActionResult> Create([FromBody] CreateBranchDto branchDto)
        {
            var result = await _branchService.CreateBranchAsync(branchDto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ROLE_DELETE")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _branchService.DeleteBranchAsync(id);
            return Ok(result);
        }
    }
}
