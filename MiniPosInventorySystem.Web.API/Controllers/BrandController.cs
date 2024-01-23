using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniPosInventorySystem.Web.API.Models;
using System.Linq;
using System.Net;

namespace MiniPosInventorySystem.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly APIDbContext _context;
        public BrandController(APIDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Brand model)
        {
            var response = new ApiResponse();
            try
            {
                await _context.Brands.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "brand data  save Successfully";
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
        public async Task<ActionResult<ApiResponse>> GetBrands(int? pageNumber, int? pageSize, string? name)
        {
            var response = new ApiResponse();
            try
            {
                var brandQuery = _context.Brands.AsQueryable();
                if(name!=null)
                {
                    brandQuery=brandQuery.Where(p => p.Name.Contains(name));
                }
                if (pageNumber!=null && pageSize!=null)
                {
                   brandQuery= brandQuery.Skip((int)(pageSize * (pageNumber))).Take((int)pageSize);
                } 
             
                var brand= await brandQuery.ToListAsync();
                if (brand == null)
                {
                    response.Message = "brand not  found";
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
        public async Task<ActionResult<ApiResponse>> GetBrand(int id)
        {
            var response = new ApiResponse();
            try
            {
                var brand = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == id);
                if (brand == null)
                {
                    response.Message = "brand not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = brand;
                response.Message = "brand Data  found";
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
        public async Task<ActionResult<ApiResponse>> PutBrand( Brand model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.Brands.FirstOrDefaultAsync(x => x.BrandId == model.BrandId);
                if (dbModel == null)
                {
                    response.Message = "Brand data not found";
                    response.IsError = true;
                    return response;
                }
                dbModel.Name = model.Name;
                dbModel.Status = model.Status;
                _context.Brands.Update(dbModel);
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Brand Data Updated";
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
        public async Task<ActionResult<ApiResponse>> DeleteBrand(int id)
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
                var brand = await _context.Brands.FindAsync(id);
                if (brand == null)
                {
                    response.Message = "Brand data is not found";
                    response.IsError = true;
                    return response;

                }
                _context.Brands.Remove(brand);
                response.Message = "Brand Data  Remove";
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
