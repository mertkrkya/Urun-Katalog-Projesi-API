using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UrunKatalogProjesi.Data.Dto;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.Models;
using UrunKatalogProjesi.Data.Repositories.Abstract;
using UrunKatalogProjesi.Data.UnitofWork;
using UrunKatalogProjesi.Service.Exceptions;
using UrunKatalogProjesi.Service.Services.Abstract;

namespace UrunKatalogProjesi.Service.Services.Concrete
{
    public class ProductService : BaseService<ProductDto,Product>, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductService(IProductRepository repository, IMapper mapper, IUnitofWork unitofWork, IHttpContextAccessor httpContextAccessor) : base(repository, unitofWork, mapper, httpContextAccessor)
        {
            _productRepository = repository;
            _unitofWork = unitofWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public override async Task<ResponseEntity> GetAllAsync()
        {
            try
            {
                var tempResult = await _productRepository.GetAllAsync();
                var result = _mapper.Map(tempResult, typeof(IEnumerable<Product>), typeof(IEnumerable<ProductDto>));
                return new ResponseEntity(result);
            }
            catch (Exception e)
            {
                throw new Exception("Get All Error");
            }
        }
        public override async Task<ResponseEntity> GetByIdAsync(int id)
        {
            try
            {
                var tempResult = await _productRepository.GetByIdAsync(id);
                var result = _mapper.Map(tempResult, typeof(Product), typeof(DetailProductDto));
                return new ResponseEntity(result);
            }
            catch (Exception e)
            {
                throw new ClientException("No Data with ID: " + id);
            }
        }
        public override async Task<ResponseEntity> InsertAsync(ProductDto entity)
        {
            try
            {
                var tempEntity = _mapper.Map<ProductDto, Product>(entity);
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                tempEntity.CreatedBy = userId;
                tempEntity.OwnerId = userId;
                var result = _productRepository.InsertAsync(tempEntity);
                await _unitofWork.CommitAsync();
                return new ResponseEntity(entity);
            }
            catch (Exception e)
            {
                throw new Exception("Product Save Error. Message: " + e.Message);
            }
        }
        public virtual async Task<ResponseEntity> UpdateAsync(int id, ProductDto entity)
        {
            try
            {
                var unUpdatedEntity = await _productRepository.GetByIdAsync(id);
                if (unUpdatedEntity == null)
                {
                    throw new ClientException("No Product Data");
                }
                var tempEntity = _mapper.Map<ProductDto, Product>(entity);
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var currentTime = DateTime.UtcNow;
                tempEntity.Id = unUpdatedEntity.Id;
                tempEntity.CreatedBy = userId;
                tempEntity.OwnerId = userId;
                tempEntity.ModifiedBy = userId;
                tempEntity.ModifiedDate = currentTime;
                _productRepository.Update(tempEntity);
                await _unitofWork.CommitAsync();
                var mapEntity = _mapper.Map<Product, ProductDto>(tempEntity);
                return new ResponseEntity(mapEntity);
            }
            catch (Exception e)
            {
                throw new Exception("Product Update Error");
            }
        }
    }
}
