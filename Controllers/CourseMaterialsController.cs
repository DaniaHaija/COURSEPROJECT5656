using COURSEPROJECT.Data;
using COURSEPROJECT.Dto.Request;
using COURSEPROJECT.Dto.Response;
using COURSEPROJECT.Model;
using COURSEPROJECT.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COURSEPROJECT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{StaticData.Moderator}")]
    public class CourseMaterialsController(ApplicationDbContext context) : ControllerBase
    {
        private readonly ApplicationDbContext _context =context;

        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var coursematerial = _context.CourseMaterials.ToList();
            if (coursematerial == null)
            {
                return NotFound();
            }
            return Ok(coursematerial.Adapt<IEnumerable<CourseMaterialResponse>>());
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById([FromRoute] int id) {
            var coursematerial = _context.CourseMaterials.Find(id);
            if (coursematerial == null)
            {
                return NotFound();
            }

            return Ok(coursematerial.Adapt<CourseMaterialResponse>());

        }

        [HttpPost("")]
        public IActionResult Create([FromForm] CourseMaterialRequest coursematerialrequest )
        {
            var file = coursematerialrequest.FileUrl;
            var coursematerial = coursematerialrequest.Adapt<CourseMaterial>();
            if (file != null && file.Length>0)
            {
             var fileName=Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath=Path.Combine(Directory.GetCurrentDirectory(),"Files", fileName);

                using (var stream=System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                coursematerial.FileUrl = fileName;
                _context.CourseMaterials.Add(coursematerial);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetById), new {id= coursematerial.ID }, coursematerial);
               
            }

            return BadRequest();

        }
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] int id ,[FromForm] CourseMaterialRequest coursematerialrequest)
        {
             var coursematerialInDb=_context.CourseMaterials.AsNoTracking().FirstOrDefault(coursematerial => coursematerial.ID == id);
            var coursematerial= coursematerialrequest.Adapt<CourseMaterial>();
            var file = coursematerialrequest.FileUrl;
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    file.CopyTo(stream);
                }
                var OldfilePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", coursematerialInDb.FileUrl);
                if (System.IO.File.Exists(OldfilePath))
                {
                    System.IO.File.Delete(OldfilePath);
                }
                coursematerial.FileUrl = fileName;




            }
            else
            {
                coursematerial.FileUrl = coursematerialInDb.FileUrl;
            }
            coursematerial.ID = id;
            _context.CourseMaterials.Update(coursematerial);
            _context.SaveChanges();
            return NoContent();

            }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            var coursematerial= _context.CourseMaterials.Find(id);
            if(coursematerial == null) { return NotFound();}
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Files", coursematerial.FileUrl);


            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
            _context.CourseMaterials.Remove(coursematerial);
            _context.SaveChanges(); 
            return NoContent();
        }

    }
      
       
    }

