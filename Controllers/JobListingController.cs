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
    public class JobListingController : ControllerBase
    {
        private readonly IJobListingRepository<JobListing> _jobRepo;

        public JobListingController(IJobListingRepository<JobListing> jobRepo)
        {
            _jobRepo = jobRepo;
        }


        [HttpGet("GetAll")]
        public IActionResult GetAllJobs()
        {
            var jobs = _jobRepo.GetAllJobs();

            var jobDtos = jobs.Select(job => new JobListingDTO
            {
                EmployerId = job.EmployerId,
                Title = job.Title,
                Description = job.Description,
                Qualifications = job.Qualifications,
                Location = job.Location,
                PostedDate = job.PostedDate
            }).ToList();

            return Ok(jobDtos);
        }



        [Authorize(Roles = "User")]
        [HttpGet("GetById/{id}")]

        public IActionResult GetJobById(int id)
        {
            var job = _jobRepo.GetJobById(id);
            if (job == null) return NotFound();
            return Ok(job);
        }

        [HttpPost("Add")]

        public IActionResult AddJob([FromBody] JobListing job)
        {
            return Ok(_jobRepo.AddJob(job));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("AddDTO")]
        public IActionResult Add([FromBody] JobListingDTO dto)
        {
            var job = new JobListing
            {
                EmployerId = dto.EmployerId,
                Title = dto.Title,
                Description = dto.Description,
                Qualifications = dto.Qualifications,
                Location = dto.Location,
                PostedDate = dto.PostedDate
            };

            return Ok(_jobRepo.AddJob(job));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]

        public IActionResult UpdateJob([FromBody] JobListing job)
        {
            return Ok(_jobRepo.UpdateJob(job));
        }

        [HttpDelete("Delete")]

        public IActionResult DeleteJob([FromBody] JobListing job)
        {
            return Ok(_jobRepo.DeleteJob(job));
        }
    }
}
