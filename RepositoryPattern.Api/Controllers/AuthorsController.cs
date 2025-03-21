using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Core.Models;
using RepositoryPattern.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RepositoryPattern.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID provided.");

            try
            {
                var author = await _unitOfWork.Authors.GetAsync(id);
                if (author == null)
                    return NotFound("Author not found.");
                var authorDto = _mapper.Map<AuthorDto>(author);
                return Ok(authorDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var authors = await _unitOfWork.Authors.GetAllAsync();
                var authorDtos = _mapper.Map<IEnumerable<AuthorDto>>(authors);
                return Ok(authorDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid ID provided.");

            try
            {
                var res = _unitOfWork.Authors.Delete(id);
                _unitOfWork.Compelete();
                return Ok(res);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDto dto)
        {
            if (dto == null)
                return BadRequest("Author data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var author = _mapper.Map<Author>(dto);
                var res = await _unitOfWork.Authors.CreateAsync(author);
                _unitOfWork.Compelete();
                var createdDto = _mapper.Map<AuthorDto>(res);
                return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Internal server error: {innerMessage}");
            }
        }


        [HttpPut]
        public async Task<IActionResult> Update([FromBody] AuthorDto dto)
        {
            if (dto == null)
                return BadRequest("Author data is null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Id <= 0)
                return BadRequest("Invalid author ID provided.");

            try
            {
                var author = await _unitOfWork.Authors.GetAsync(dto.Id);
                if (author == null)
                    return NotFound("Author not found.");

                _mapper.Map(dto, author);
                var res = _unitOfWork.Authors.Update(author);
                _unitOfWork.Compelete();
                var updatedDto = _mapper.Map<AuthorDto>(res);
                return Ok(updatedDto);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
