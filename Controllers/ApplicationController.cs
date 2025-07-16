using DAL.DataAccess;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.DTO;

namespace ServiceLayer.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationRepository<Application> _appRepo;

        public ApplicationController(IApplicationRepository<Application> appRepo)
        {
            _appRepo = appRepo;
        }

        [HttpGet("GetApplicationsByUserEmail/{email}")]

        public IActionResult GetApplicationsByUserEmail(string email)
        {
            return Ok(_appRepo.GetApplicationsByUserEmail(email));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GettAll")]

        public IActionResult GetAllApplications()
        {
            return Ok(_appRepo.GetAllApplications());
        }

        [HttpPost("Add")]

        public IActionResult AddApplication([FromBody] Application application)
        {
            return Ok(_appRepo.AddApplication(application));
        }

        [HttpPost("AddDTO")]
        public IActionResult Add([FromBody] ApplicationDTO dto)
        {
            var application = new Application
            {
                JobId = dto.JobId,
                JobSeekerId = dto.JobSeekerId,
                AppliedDate = dto.AppliedDate,
                Status = dto.Status,
                Notes = dto.Notes
            };

            return Ok(_appRepo.AddApplication(application));
        }

        [Authorize(Roles = "User")]
        [HttpPut("Update")]

        public IActionResult UpdateApplication([FromBody] Application application)
        {
            return Ok(_appRepo.UpdateApplication(application));
        }

        [HttpDelete("Delete")]


        public IActionResult DeleteApplication([FromBody] Application application)
        {
            return Ok(_appRepo.DeleteApplication(application));
        }
    }
}
