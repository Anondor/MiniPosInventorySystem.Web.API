﻿using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniPosInventorySystem.Web.API.Models;
using System.Net;

namespace MiniPosInventorySystem.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly APIDbContext _context;
        public ProductController(APIDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Save(Product model)
        {
            var response = new ApiResponse();
            try
            {
                await _context.Products.AddAsync(model);
                await _context.SaveChangesAsync();

                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Product data  save Successfully";
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
        public async Task<ActionResult<ApiResponse>> GetProducts(int? pageNumber, int? pageSize, string? name)
        {
            var response = new ApiResponse();
            try
            {
                var productQuery = _context.Products.AsQueryable();
                if (name != null)
                {
                    productQuery = productQuery.Where(p => p.Name.Contains(name));
                }
                if (pageNumber != null && pageSize != null)
                {
                    productQuery = productQuery.Skip((int)(pageSize * (pageNumber))).Take((int)pageSize);
                }

                var product = await productQuery.ToListAsync();
                if (product == null)
                {
                    response.Message = "Product not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = product;
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
        public async Task<ActionResult<ApiResponse>> GetProduct(int id)
        {
            var response = new ApiResponse();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
                if (product == null)
                {
                    response.Message = "Product not  found";
                    response.IsError = true;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    return response;
                }
                response.Result = product;
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
        public async Task<ActionResult<ApiResponse>> PutProduct(Product model)
        {
            var response = new ApiResponse();
            try
            {
                var dbModel = await _context.Products.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (dbModel == null)
                {
                    response.Message = "Brand data not found";
                    response.IsError = true;
                    return response;
                }
                dbModel.Name = model.Name;
                dbModel.Status = model.Status;
                dbModel.BrandId = model.BrandId;
                dbModel.CategoryId = model.CategoryId;
                _context.Products.Update(dbModel);
                await _context.SaveChangesAsync();
                response.StatusCode = (int)HttpStatusCode.OK;
                response.Message = "Product Data Updated";
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
        public async Task<ActionResult<ApiResponse>> DeleteProduct(int id)
        {
            var response = new ApiResponse();

            if (_context.Products == null)
            {
                response.Message = "No Item Available";
                response.IsError = true;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    response.Message = "Product data is not found";
                    response.IsError = true;
                    return response;
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                response.Message = "Product Data  Remove";
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
