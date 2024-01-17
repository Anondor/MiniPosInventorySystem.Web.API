using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniPosInventorySystem.Web.API.Models;
using System.Net;

namespace MiniPosInventorySystem.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly APIDbContext _context;
        public CategoryController(APIDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Category model)
        {
            var response = new ApiResponse();
            try
            {
                await _context.Categories.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ResponseException = ex.Message;
                response.IsError = true;
                return response;
            }
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetCategories()
        {
            var response = new ApiResponse();
            try
            {
                var brand = await _context.Categories.ToListAsync();

                if (brand == null)
                {
                    response.Message = "Category not  found";
                    response.IsError = true;
                }
                response.Result = brand;
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ResponseException = ex.Message;
                response.IsError = true;
                return response;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse>> GetCategories(int id)
        {
            var response = new ApiResponse();
            try
            {
                var brand = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == id);
                if (brand == null)
                {
                    response.Message = "Category not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = brand;
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.ResponseException = ex.Message;
                response.IsError = true;
                return response;
            }
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse>> PutCategory(Category model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.Categories.FirstOrDefaultAsync(x => x.CategoryId == model.CategoryId);
                if (dbModel == null)
                {
                    response.Message = "Brand data not found";
                    response.IsError = true;
                    return response;
                }
                dbModel.Name = model.Name;
                dbModel.Status = model.Status;
                _context.Categories.Update(dbModel);
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsError = true;
                return response;

            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> DeleteCategory(int id)
        {
            var response = new ApiResponse();

            if (_context.Brands == null)
            {
                response.Message = "No Item Available";
                response.IsError = true;
                return response;
            }
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    response.Message = "Brand data is not found";
                    response.IsError = true;
                    return response;

                }
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                return response;

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsError = true;
                return response;

            }


        }

    }
}
