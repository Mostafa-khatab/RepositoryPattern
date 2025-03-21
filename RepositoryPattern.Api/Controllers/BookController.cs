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
    public class BookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookController(IUnitOfWork unitOfWork, IMapper mapper)
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
                var book = await _unitOfWork.Books.GetAsync(id);
                if (book == null)
                    return NotFound("Book not found.");
                var bookDto = _mapper.Map<BookDto>(book);
                return Ok(bookDto);
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
                var books = await _unitOfWork.Books.GetAllAsync();
                var bookDtos = _mapper.Map<IEnumerable<BookDto>>(books);
                return Ok(bookDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDto dto)
        {
            if (dto == null)
                return BadRequest("Book data is null.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var author = await _unitOfWork.Authors.GetAsync(dto.AuthorId);
                if (author == null)
                    return NotFound("Author not found.");

                var book = _mapper.Map<Book>(dto);
                var res = await _unitOfWork.Books.CreateAsync(book);
                _unitOfWork.Compelete();
                var createdDto = _mapper.Map<BookDto>(res);
                return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] BookDto dto)
        {
            if (dto == null)
                return BadRequest("Book data is null.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (dto.Id <= 0)
                return BadRequest("Invalid book ID provided.");

            try
            {
                var book = await _unitOfWork.Books.GetAsync(dto.Id);
                if (book == null)
                    return NotFound("Book not found.");

                var author = await _unitOfWork.Authors.GetAsync(dto.AuthorId);
                if (author == null)
                    return NotFound("Author not found.");

                _mapper.Map(dto, book);
                var res = _unitOfWork.Books.Update(book);
                _unitOfWork.Compelete();
                var updatedDto = _mapper.Map<BookDto>(res);
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
