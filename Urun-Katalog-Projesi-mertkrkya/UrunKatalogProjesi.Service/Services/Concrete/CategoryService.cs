using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CategoryService : BaseService<CategoryDto,Category>, ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitofWork _unitofWork;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository repository, IMapper mapper, IUnitofWork unitofWork, IHttpContextAccessor httpContextAccessor) : base(repository, unitofWork, mapper,httpContextAccessor)
        {
            _categoryRepository = repository;
            _unitofWork = unitofWork;
            _mapper = mapper;
        }
        public override async Task<ResponseEntity> GetAllAsync()
        {
            try
            {
                var tempResult = await _categoryRepository.GetAllAsync();
                var result = _mapper.Map(tempResult,typeof(IEnumerable<Category>),typeof(IEnumerable<CategoryDto>));
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
                var tempResult = await _categoryRepository.GetByIdAsync(id);
                var result = _mapper.Map<Category,DetailCategoryDto>(tempResult);
                return new ResponseEntity(result);
            }
            catch (Exception e)
            {
                throw new ClientException("No Data with ID: " + id);
            }
        }
    }
}
