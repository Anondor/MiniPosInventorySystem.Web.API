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
    public class UnitController : ControllerBase
    {
        private readonly APIDbContext _context;

        public UnitController(APIDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Unit model)
        {
            var response = new ApiResponse();
            try
            {
                await _context.Units.AddAsync(model);
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
        public async Task<ActionResult<ApiResponse>> GetUnits()
        {
            var response = new ApiResponse();
            try
            {
                var unit = await _context.Units.ToListAsync();

                if (unit == null)
                {
                    response.Message = "Unit not  found";
                    response.IsError = true;
                }
                response.Result = unit;
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
        public async Task<ActionResult<ApiResponse>> GetUnit(int id)
        {
            var response = new ApiResponse();
            try
            {
                var unit = await _context.Units.FirstOrDefaultAsync(x => x.UnitId == id);
                if (unit == null)
                {
                    response.Message = "Unit not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = unit;
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
        public async Task<ActionResult<ApiResponse>> PutUnit(Unit model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.Units.FirstOrDefaultAsync(x => x.UnitId == model.UnitId);
                if (dbModel == null)
                {
                    response.Message = "Unit data not found";
                    response.IsError = true;
                    return response;
                }
                response.Message = "Unit data update successfully";
                dbModel.Name = model.Name;
                dbModel.Status = model.Status;
                _context.Units.Update(dbModel);
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
        public async Task<ActionResult<ApiResponse>> DeleteUnit(int id)
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
                var unit = await _context.Units.FindAsync(id);
                if (unit == null)
                {
                    response.Message = "Unit data is not found";
                    response.IsError = true;
                    return response;

                }
                _context.Units.Remove(unit);
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
