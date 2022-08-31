using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using UrunKatalogProjesi.Data.Entities;
using UrunKatalogProjesi.Data.UnitofWork;
using UrunKatalogProjesi.Data.Repositories;
using UrunKatalogProjesi.Service.Exceptions;
using Microsoft.AspNetCore.Identity;
using UrunKatalogProjesi.Data.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace UrunKatalogProjesi.Service.Services
{
    public class BaseService<Dto,TEntity> : IBaseService<Dto, TEntity> where TEntity : class
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly IUnitofWork _unitofWork;
        protected readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseService(IBaseRepository<TEntity> repository, IUnitofWork unitofWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base()
        {
            _repository = repository;
            _unitofWork = unitofWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public virtual async Task<ResponseEntity> GetAllAsync()
        {
            try
            {
                var allRecord = await _repository.GetAllAsync();
                var mappedResult = _mapper.Map<IEnumerable<TEntity>, IEnumerable<Dto>>(allRecord);
                return new ResponseEntity(mappedResult);
            }
            catch (Exception e)
            {
                throw new Exception("Get All Error");
            }
        }

        public virtual async Task<ResponseEntity> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                var mappedResult = _mapper.Map<TEntity, Dto>(result);
                return new ResponseEntity(mappedResult);
            }
            catch (Exception e)
            {
                throw new ClientException("No Data with ID: "+id);
            }
        }

        public virtual async Task<ResponseEntity> InsertAsync(Dto entity)
        {
            try
            {
                var tempEntity = _mapper.Map<Dto, TEntity>(entity);
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var property = tempEntity.GetType().GetProperty("CreatedBy");
                if (property != null)
                    property.SetValue(tempEntity, userId, null);
                var result = _repository.InsertAsync(tempEntity);
                await _unitofWork.CommitAsync();
                return new ResponseEntity(entity);
            }
            catch (Exception e)
            {
                throw new Exception("Save Error");
            }
        }

        public virtual async Task<ResponseEntity> UpdateAsync(int id, Dto entity)
        {
            try
            {
                var unUpdatedEntity = await _repository.GetByIdAsync(id);
                if (unUpdatedEntity == null)
                {
                    throw new ClientException("No Data");
                }
                var tempEntity = _mapper.Map<Dto, TEntity>(entity);
                var currentUser = _httpContextAccessor.HttpContext.User;
                var userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                tempEntity.GetType().GetProperty("Id").SetValue(tempEntity, id);
                var tempProperty = unUpdatedEntity.GetType().GetProperty("CreatedBy");
                var tempPropertyDate = unUpdatedEntity.GetType().GetProperty("CreatedDate");
                var property = tempEntity.GetType().GetProperty("ModifiedBy");
                var propertyDate = tempEntity.GetType().GetProperty("ModifiedDate");
                if (tempProperty != null && tempPropertyDate != null && property != null && propertyDate != null)
                {
                    var currentTime = DateTime.UtcNow;
                    var createdUser = tempProperty.GetValue(unUpdatedEntity);
                    var createdTime = tempPropertyDate.GetValue(unUpdatedEntity);
                    tempEntity.GetType().GetProperty("CreatedBy").SetValue(tempEntity,createdUser);
                    tempEntity.GetType().GetProperty("CreatedDate").SetValue(tempEntity, createdTime);
                    property.SetValue(tempEntity, userId, null);
                    propertyDate.SetValue(tempEntity, currentTime, null);
                }
                _repository.Update(tempEntity);
                await _unitofWork.CommitAsync();
                return new ResponseEntity(entity);
            }
            catch (Exception e)
            {
                throw new Exception("Update Error");
            }
        }

        public virtual async Task<ResponseEntity> DeleteAsync(int id)
        {
            try
            {
                var deleteEntity = await _repository.GetByIdAsync(id);
                if (deleteEntity == null)
                {
                    throw new ClientException("No Data");
                }
                _repository.Delete(deleteEntity);
                _unitofWork.Commit();
                return new ResponseEntity(deleteEntity);
            }
            catch (Exception e)
            {
                throw new Exception("Delete Error");
            }
        }
    }
}
