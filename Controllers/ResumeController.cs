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
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IResumeRepository<Resume> _resumeRepo;

        public ResumeController(IResumeRepository<Resume> resumeRepo)
        {
            _resumeRepo = resumeRepo;
        }


        [HttpGet("GetAll")]

        public IActionResult GetAllResumes()
        {
            return Ok(_resumeRepo.GetAllResumes());
        }

        [HttpPost("Add")]

        public IActionResult AddResume([FromBody] Resume resume)
        {
            return Ok(_resumeRepo.AddResume(resume));
        }

        [HttpPost("AddDTO")]
        public IActionResult AddResume([FromBody] ResumeDTO dto)
        {
            var resume = new Resume
            {
                UserId = dto.UserId,
                FileName = dto.FileName,
                FileType = dto.FileType,
                FileData = dto.FileData,
                UploadedDate = dto.UploadedDate
            };

            var result = _resumeRepo.AddResume(resume);
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]

        public IActionResult UpdateResume([FromBody] Resume resume)
        {
            return Ok(_resumeRepo.UpdateResume(resume));
        }

        [HttpDelete("Delete")]

        public IActionResult DeleteResume([FromBody] Resume resume)
        {
            return Ok(_resumeRepo.DeleteResume(resume));
        }
    }
}
