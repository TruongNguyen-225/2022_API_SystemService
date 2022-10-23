using System;
using AutoMapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SystemServiceAPICore3.Bo.Interface;
using SystemServiceAPICore3.Uow.Interface;
using SystemServiceAPICore3.Domain.Helpers;
using SystemServiceAPICore3.Repositories.Interfaces;
using SystemServiceAPICore3.Services.Expressions;

namespace SystemServiceAPICore3.Bo
{
    public abstract class EntityBo<TDto, TEntity> : IEntityBo<TDto>
        where TDto : class
        where TEntity : class
    {
        #region -- Variables --

        protected IUnitOfWork unitOfWork;
        protected IMapper mapper;

        #endregion

        #region -- Properties --
        #endregion

        #region -- Constructors --

        //public EntityBo(IServiceProvider serviceProvider)
        //{
        //    this.unitOfWork = serviceProvider.GetService<IUnitOfWork>();
        //    this.mapper = serviceProvider.GetService<IMapper>();
        //}

        public EntityBo(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        #endregion

        #region -- Overrides --
        #endregion

        #region -- Methods --

        #region Get

        public virtual TDto Get(object[] ids)
        {
            // Validate parameters
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            TDto dto = null;

            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get model object.
            var model = repository.Get(ids);
            if (model != null)
            {
                // Map to Dto object.
                dto = mapper.Map<TEntity, TDto>(model);
            }

            return dto;
        }

        public virtual IEnumerable<TDto> GetList()
        {
            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get result data.
            var models = Find(repository, rep => rep.GetAll());

            // Map model list to entity list.
            var dtos = mapper.Map<TEntity[], TDto[]>(models.ToArray());

            return dtos;
        }

        public virtual IEnumerable<TDto> FindBy(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Validate parameters
            if (propertyPredicate == null)
            {
                throw new ArgumentNullException(nameof(propertyPredicate));
            }

            var dtos = new List<TDto>();

            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            // Get model entities.
            var models = Find(repository, rep => rep.FindBy(modelPredicate));
            if (models != null)
            {
                // Map to Dto list.
                dtos = mapper.Map<List<TEntity>, List<TDto>>(models.ToList());
            }

            return dtos;
        }

        public virtual TDto FirstOrDefault(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Validate parameters
            if (propertyPredicate == null)
            {
                throw new ArgumentNullException(nameof(propertyPredicate));
            }

            TDto dto = null;

            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            // Get result data.
            var model = repository.FirstOrDefault(modelPredicate);
            if (model != null)
            {
                // Map to Dto object.
                dto = mapper.Map<TEntity, TDto>(model);
            }

            return dto;
        }

        public virtual async Task<TDto> GetAsync(object[] ids)
        {
            // Validate parameters
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            TDto dto = null;

            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get model object.
            var model = await repository.GetAsync(ids);
            if (model != null)
            {
                // Map to Dto object.
                dto = mapper.Map<TEntity, TDto>(model);
            }

            return dto;
        }

        public virtual async Task<IEnumerable<TDto>> GetListAsync()
        {
            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get result data.
            var models = await FindAsync(repository, rep => rep.GetAllAsync());

            // Map model list to entity list.
            var dtos = mapper.Map<TEntity[], TDto[]>(models.ToArray());

            return dtos;
        }

        public virtual async Task<IEnumerable<TDto>> FindByAsync(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Validate parameters
            if (propertyPredicate == null)
            {
                throw new ArgumentNullException(nameof(propertyPredicate));
            }

            var dtos = new List<TDto>();

            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            var models = await FindAsync(repository, rep => repository.FindByAsync(modelPredicate));
            if (models != null)
            {
                // Map to Dto list.
                dtos = mapper.Map<List<TEntity>, List<TDto>>(models.ToList());
            }

            return dtos;
        }

        public virtual async Task<TDto> FirstOrDefaultAsync(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Validate parameters
            if (propertyPredicate == null)
            {
                throw new ArgumentNullException(nameof(propertyPredicate));
            }

            TDto dto = null;

            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            // Get result data.
            var model = await repository.FirstOrDefaultAsync(modelPredicate);
            if (model != null)
            {
                // Map to Dto object.
                dto = mapper.Map<TEntity, TDto>(model);
            }

            return dto;
        }

        public virtual bool Exists(object[] ids)
        {
            // Validate parameters
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            // Get repository.
            var repository = GetRepository<TEntity>();

            // Get result data.
            var model = repository.FirstOrDefault(EntityHelper.GetKeyExpression<TEntity>(ids));
            return model != null;
        }

        #endregion

        #region Insert

        public virtual TDto Insert(TDto dto)
        {
            // Validate parameters
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Create model.
            var model = mapper.Map<TDto, TEntity>(dto);

            // Add model data context.
            Insert(unitOfWork, model);

            // Save change to datacontext.
            unitOfWork.Save();

            // Map model to entity.
            dto = mapper.Map<TEntity, TDto>(model);

            return dto;
        }

        public virtual TDto[] Insert(TDto[] dtos)
        {
            // Validate parameters
            if (dtos == null)
            {
                throw new ArgumentNullException(nameof(dtos));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Map model list to entity list.
            var models = mapper.Map<TDto[], TEntity[]>(dtos);

            // Add model data context.
            Insert(unitOfWork, models);

            // Save change to datacontext.
            unitOfWork.Save();

            // Map model to entity.
            dtos = mapper.Map<TEntity[], TDto[]>(models);

            return dtos;
        }

        public async Task<TDto> InsertAsync(TDto dto)
        {
            // Validate parameters
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Create model.
            var model = mapper.Map<TDto, TEntity>(dto);

            // Add model data context.
            await InsertAsync(unitOfWork, model);

            // Save change to datacontext.
            await unitOfWork.SaveAsync();

            // Map model to entity.
            dto = mapper.Map<TEntity, TDto>(model);

            return dto;
        }

        public async Task<TDto[]> InsertAsync(TDto[] dtos)
        {
            // Validate parameters
            if (dtos == null)
            {
                throw new ArgumentNullException(nameof(dtos));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Map entity to model.
            var models = mapper.Map<TDto[], TEntity[]>(dtos);

            // Add model data context.
            await InsertAsync(unitOfWork, models);

            // Save change to datacontext.
            await unitOfWork.SaveAsync();

            // Map model to entity.
            dtos = mapper.Map<TEntity[], TDto[]>(models);

            return dtos;
        }

        #endregion

        #region Update

        public virtual TDto Update(TDto dto)
        {
            // Validate parameters
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Update entity.
            Update(repository, dto);

            // Save change to datacontext.
            unitOfWork.Save();

            //Return fresh object after update
            return dto;
        }

        public virtual TDto[] Update(TDto[] dtos)
        {
            // Validate parameters
            if (dtos == null || dtos.Length == 0)
            {
                throw new ArgumentNullException(nameof(dtos));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get data context.
            Update(repository, dtos);

            // Save changed.
            unitOfWork.Save();

            //Return fresh object after update
            return dtos;
        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            // Validate parameters
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Update entity.
            await UpdateAsync(repository, dto);

            // Save change to datacontext.
            await unitOfWork.SaveAsync();

            //Return fresh object after update
            return dto;
        }

        public virtual async Task<TDto[]> UpdateAsync(TDto[] dtos)
        {
            // Validate parameters
            if (dtos == null || dtos.Length == 0)
            {
                throw new ArgumentNullException(nameof(dtos));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Update data.
            await UpdateAsync(repository, dtos);

            // Save changed.
            await unitOfWork.SaveAsync();

            //Return fresh object after update
            return dtos;
        }

        #endregion

        #region Delete

        public virtual bool Delete(object[] ids)
        {
            // Validate parameters
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Delete
            var result = repository.Delete(ids);

            // Save change to data context.
            if (result)
            {
                unitOfWork.Save();
            }

            return result;
        }

        public virtual bool Delete(TDto dto)
        {
            // Validate parameters
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Delete
            var result = Delete(unitOfWork, dto);

            // Save change to data context.
            if (result)
            {
                unitOfWork.Save();
            }

            return true;
        }

        public virtual bool Delete(TDto[] dtos)
        {
            // Validate parameters
            if (dtos == null || dtos.Length == 0)
            {
                throw new ArgumentNullException(nameof(dtos));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Delete
            var result = Delete(unitOfWork, dtos);

            // Save change to data context.
            if (result)
            {
                unitOfWork.Save();
            }

            return true;
        }

        public virtual bool Delete(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            // Delete
            var result = repository.Delete(modelPredicate);

            // Save change to data context.
            if (result)
            {
                unitOfWork.Save();
            }

            return true;
        }

        public virtual async Task<bool> DeleteAsync(object[] ids)
        {
            // Validate parameters
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Delete
            var result = await repository.DeleteAsync(ids);

            // Save change to data context.
            if (result)
            {
                await unitOfWork.SaveAsync();
            }

            return result;
        }

        public virtual async Task<bool> DeleteAsync(TDto dto)
        {
            // Validate parameters
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Delete
            var result = await DeleteAsync(unitOfWork, dto);

            // Save change to data context.
            if (result)
            {
                await unitOfWork.SaveAsync();
            }

            return true;
        }

        public virtual async Task<bool> DeleteAsync(TDto[] dtos)
        {
            // Validate params.
            if (dtos == null || dtos.Length == 0)
            {
                throw new ArgumentNullException(nameof(dtos));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Delete
            var result = await DeleteAsync(unitOfWork, dtos);

            // Save change to data context.
            if (result)
            {
                await unitOfWork.SaveAsync();
            }

            return result;
        }

        public virtual async Task<bool> DeleteAsync(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Validate params.
            if (propertyPredicate == null)
            {
                throw new ArgumentNullException(nameof(propertyPredicate));
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            // Delete
            var result = await repository.DeleteAsync(modelPredicate);

            // Save change to data context.
            if (result)
            {
                await unitOfWork.SaveAsync();
            }

            return true;
        }

        #endregion

        #region Aggregates

        public virtual int Count()
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result data.
            return repository.Count();
        }

        public virtual int Count(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Validate params.
            if (propertyPredicate == null)
            {
                throw new ArgumentNullException(nameof(propertyPredicate));
            }

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result data.
            return repository.Count(modelPredicate);
        }

        public virtual async Task<int> CountAsync()
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result data.
            return await repository.CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<TDto, bool>> propertyPredicate)
        {
            // Validate params.
            if (propertyPredicate == null)
            {
                throw new ArgumentNullException(nameof(propertyPredicate));
            }

            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(propertyPredicate);

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result data.
            return await repository.CountAsync(modelPredicate);
        }

        #endregion

        #region Paging

        public virtual IEnumerable<TDto> GetListPaged(Expression<Func<TDto, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<TDto, object>>[] sortingExpression)
        {
            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(predicate);

            // Get sort expression.
            var sortPredicates = new List<Expression<Func<TEntity, object>>>();
            foreach (var item in sortingExpression)
            {
                var sortPredicate = Expression<TDto, TEntity>.Tranform(item);
                sortPredicates.Add(sortPredicate);
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result.
            var models = repository.GetListPaged(
                                            modelPredicate,
                                            pageNo,
                                            pageSize,
                                            ascending,
                                            sortPredicates.ToArray())
                                      .ToArray();

            // Map model list to entity list.
            var dtos = mapper.Map<IEnumerable<TEntity>, List<TDto>>(models);

            return dtos;
        }

        public virtual IEnumerable<TDto> GetListPaged(Expression<Func<TDto, bool>> predicate, int pageNo, int pageSize)
        {
            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(predicate);

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result.
            var models = repository.GetListPaged(
                                        modelPredicate,
                                        pageNo,
                                        pageSize);

            // Map model list to entity list.
            var dtos = mapper.Map<IEnumerable<TEntity>, List<TDto>>(models);

            return dtos;
        }

        public virtual async Task<IEnumerable<TDto>> GetListPagedAsync(Expression<Func<TDto, bool>> predicate, int pageNo, int pageSize, bool ascending = true, params Expression<Func<TDto, object>>[] sortingExpression)
        {
            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(predicate);

            // Get sort expression.
            var sortPredicates = new List<Expression<Func<TEntity, object>>>();
            foreach (var item in sortingExpression)
            {
                var sortPredicate = Expression<TDto, TEntity>.Tranform(item);
                sortPredicates.Add(sortPredicate);
            }

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result.
            var models = await repository.GetListPagedAsync(
                modelPredicate,
                pageNo,
                pageSize,
                ascending,
                sortPredicates.ToArray());

            // Map model list to entity list.
            var dtos = mapper.Map<IEnumerable<TEntity>, List<TDto>>(models);

            return dtos;
        }

        public virtual async Task<IEnumerable<TDto>> GetListPagedAsync(Expression<Func<TDto, bool>> predicate, int pageNo, int pageSize)
        {
            // Get property model predicate.
            var modelPredicate = Expression<TDto, TEntity>.Tranform(predicate);

            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Get result.
            var models = await repository.GetListPagedAsync(
                modelPredicate,
                pageNo,
                pageSize);

            // Map model list to entity list.
            var dtos = mapper.Map<IEnumerable<TEntity>, List<TDto>>(models);

            return dtos;
        }

        #endregion

        #region Execution

        public virtual int ExecuteSqlRaw(string sql, params object[] parameters)
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return unitOfWork.ExecuteSqlRaw(sql, parameters);
        }

        public virtual int ExecuteSqlInterpolated(FormattableString sql)
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return unitOfWork.ExecuteSqlInterpolated(sql);
        }

        public virtual IEnumerable<TAny> FromSqlRaw<TAny>(string sql, params object[] parameters) where TAny : class
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return unitOfWork.FromSqlRaw<TAny>(sql, parameters);
        }

        public virtual IEnumerable<TAny> FromSqlInterpolated<TAny>(FormattableString sql) where TAny : class
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return unitOfWork.FromSqlInterpolated<TAny>(sql);
        }

        public virtual async Task<int> ExecuteSqlRawAsync(string sql, params object[] parameters)
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return await unitOfWork.ExecuteSqlRawAsync(sql, parameters);
        }

        public virtual async Task<int> ExecuteSqlInterpolatedAsync(FormattableString sql)
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return await unitOfWork.ExecuteSqlInterpolatedAsync(sql);
        }

        public virtual async Task<IEnumerable<TAny>> FromSqlRawAsync<TAny>(string query, params object[] parameters) where TAny : class
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return await unitOfWork.FromSqlRawAsync<TAny>(query, parameters);
        }

        public virtual async Task<IEnumerable<TAny>> FromSqlInterpolatedAsync<TAny>(FormattableString sql) where TAny : class
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return await unitOfWork.FromSqlInterpolatedAsync<TAny>(sql);
        }

        public virtual IQueryable<TAny> ExecuteStore<TAny>(string sql, object param) where TAny : class
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return unitOfWork.ExecuteStore<TAny>(sql, param);
        }

        public virtual DataSet ExecuteSpDataSet(string sql, object param)
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return unitOfWork.ExecuteSpDataSet(sql, param);
        }

        public virtual DataTable ExecuteSpDataTable(string sql, object param)
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            return unitOfWork.ExecuteSpDataTable(sql, param);
        }

        #endregion

        #region Others

        protected IGenericRepository<TAny> GetRepository<TAny>()
            where TAny : class
        {
            // Get data context.
            var unitOfWork = GetDataContext();

            // Get repository.
            var repository = unitOfWork.Repository<TAny>();
            return repository;
        }

        protected IUnitOfWork GetDataContext()
        {
            return unitOfWork;
        }

        protected IEnumerable<TEntity> Find(
            IGenericRepository<TEntity> repository,
            Func<IGenericRepository<TEntity>, IEnumerable<TEntity>> getFunc)
        {
            // In case EF, set no tracking.
            var old = SetNoTracking(repository, false);

            // Get result.
            var result = getFunc(repository);

            // In case EF, restore no tracking.
            SetNoTracking(repository, old);

            return result;
        }

        protected async Task<IEnumerable<TEntity>> FindAsync(
            IGenericRepository<TEntity> repository,
            Func<IGenericRepository<TEntity>, Task<IEnumerable<TEntity>>> getFunc)
        {
            // In case EF, set no tracking.
            var old = SetNoTracking(repository, false);

            var result = await getFunc(repository);

            // In case EF, restore no tracking.
            SetNoTracking(repository, old);

            return result;
        }

        protected bool SetNoTracking(IGenericRepository<TEntity> repository, bool value)
        {
            // In case EF, set no tracking.
            var savedValue = true;
            if (repository is IBaseEfCoreGenericRepository<TEntity> efRepository)
            {
                savedValue = efRepository.AsNoTracking;
                efRepository.AsNoTracking = value;
            }

            return savedValue;
        }

        protected bool SetAutoDetectChange(IUnitOfWork unitOfWork, bool value)
        {
            // In case  EF, set auto detect change to false.
            var savedValue = true;
            if (unitOfWork is IBaseEfCoreUnitOfWork efUnitOfWork)
            {
                savedValue = efUnitOfWork.AutoDetectChange;
                efUnitOfWork.AutoDetectChange = value;
            }

            return savedValue;
        }

        protected void Insert(IUnitOfWork unitOfWork, TEntity model)
        {
            // In case  EF, set auto detect change to false.
            var savedValue = SetAutoDetectChange(unitOfWork, true);

            // Add model data context.
            var repository = unitOfWork.Repository<TEntity>();
            repository.Insert(model);

            // In case EF, restore value.
            SetAutoDetectChange(unitOfWork, savedValue);
        }

        protected void Insert(IUnitOfWork unitOfWork, TEntity[] models)
        {
            // In case  EF, set auto detect change to false.
            var savedValue = SetAutoDetectChange(unitOfWork, false);

            // Add model data context.
            var repository = unitOfWork.Repository<TEntity>();
            repository.Insert(models);

            // In case EF, restore value.
            SetAutoDetectChange(unitOfWork, savedValue);
        }

        protected async Task InsertAsync(IUnitOfWork unitOfWork, TEntity models)
        {
            // In case  EF, set auto detect change to false.
            var savedValue = SetAutoDetectChange(unitOfWork, true);

            var repository = unitOfWork.Repository<TEntity>();
            await repository.InsertAsync(models);

            // In case EF, restore value.
            SetAutoDetectChange(unitOfWork, savedValue);
        }

        protected async Task InsertAsync(IUnitOfWork unitOfWork, TEntity[] models)
        {
            // In case  EF, set auto detect change to false.
            var savedValue = SetAutoDetectChange(unitOfWork, true);

            var repository = unitOfWork.Repository<TEntity>();
            await repository.InsertAsync(models);

            // In case EF, restore value.
            SetAutoDetectChange(unitOfWork, savedValue);
        }

        protected TDto Update(IGenericRepository<TEntity> repository, TDto dto)
        {
            // Get and mapping.
            var models = GetAndMap(repository, dto);
            if (models.Length == 1)
            {
                // Update model.
                repository.Update(models[0]);
            }

            return dto;
        }

        protected TDto[] Update(IGenericRepository<TEntity> repository, params TDto[] dtos)
        {
            // Get and mapping.
            var models = GetAndMap(repository, dtos);
            if (models.Length == dtos.Length)
            {
                // Update model.
                repository.Update(models);
            }

            return dtos;
        }

        protected TEntity[] GetAndMap(IGenericRepository<TEntity> repository, params TDto[] dtos)
        {
            string getKey(object[] keys)
            {
                var skey = string.Empty;
                foreach (var key in keys)
                {
                    skey += $"{key}^";
                }

                return skey;
            }

            // Get models.
            var models = EntityBoHelper.GetModels(repository, dtos).ToArray();
            if (models.Length == dtos.Length)
            {
                // Build entity keys value.
                var keyDic = new Dictionary<string, TDto>();
                for (var i = 0; i < dtos.Length; i++)
                {
                    var entity = dtos[i];
                    var keys = EntityHelper.GetPrimaryKeyValues(entity);
                    var skey = getKey(keys);

                    keyDic.Add(skey, entity);
                }

                // Map entities to models.
                for (var i = 0; i < models.Length; i++)
                {
                    var model = models[i];
                    var keys = EntityHelper.GetPrimaryKeyValues(model);
                    var skey = getKey(keys);
                    if (keyDic.TryGetValue(skey, out TDto entity))
                    {
                        mapper.Map(entity, model);
                    }
                }
            }

            return models;
        }

        protected async Task<TDto> UpdateAsync(IGenericRepository<TEntity> repository, TDto dto)
        {
            // Get and mapping.
            var models = await GetAndMapAsync(repository, dto);
            if (models.Length == 1)
            {
                // Update model.
                await repository.UpdateAsync(models[0]);
            }

            return dto;
        }

        protected async Task<TDto[]> UpdateAsync(IGenericRepository<TEntity> repository, params TDto[] dtos)
        {
            // Get and mapping.
            var models = await GetAndMapAsync(repository, dtos);
            if (models.Length == dtos.Length)
            {
                // Update model.
                await repository.UpdateAsync(models);
            }

            return dtos;
        }

        protected async Task<TEntity[]> GetAndMapAsync(IGenericRepository<TEntity> repository, params TDto[] dtos)
        {
            string getKey(object[] keys)
            {
                var skey = string.Empty;
                foreach (var key in keys)
                {
                    skey += $"{key}^";
                }

                return skey;
            }

            // Get models.
            var models = (await EntityBoHelper.GetModelsAsync(repository, dtos)).ToArray();
            if (models.Length == dtos.Length)
            {
                // Build entity keys value.
                var keyDic = new Dictionary<string, TDto>();
                for (var i = 0; i < dtos.Length; i++)
                {
                    var entity = dtos[i];
                    var keys = EntityHelper.GetPrimaryKeyValues(entity);
                    var skey = getKey(keys);

                    keyDic.Add(skey, entity);
                }

                // Map entities to models.
                for (var i = 0; i < models.Length; i++)
                {
                    var model = models[i];
                    var keys = EntityHelper.GetPrimaryKeyValues(model);
                    var skey = getKey(keys);
                    if (keyDic.TryGetValue(skey, out TDto entity))
                    {
                        mapper.Map(entity, model);
                    }
                }
            }

            return models;
        }

        protected virtual bool Delete(IUnitOfWork unitOfWork, params TDto[] dtos)
        {
            // Get key expression list.
            var keyExpress = EntityBoHelper.GetExpressionKey<TDto, TEntity>(dtos);

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Delete data.
            return repository.Delete(keyExpress);
        }

        protected virtual async Task<bool> DeleteAsync(IUnitOfWork unitOfWork, params TDto[] dtos)
        {
            // Get key expression list.
            var keyExpress = EntityBoHelper.GetExpressionKey<TDto, TEntity>(dtos);

            // Get repository.
            var repository = unitOfWork.Repository<TEntity>();

            // Delete data.
            return await repository.DeleteAsync(keyExpress);
        }

        #endregion

        #region Virtual

        protected virtual IQueryable<TAny> GetQueryable<TAny>()
            where TAny : class
        {
            var unitOfWork = GetDataContext();
            if (unitOfWork is IBaseEfCoreUnitOfWork efUnitOfWork)
            {
                return efUnitOfWork.EfRepository<TAny>().AsQueryable();
            }

            return Array.Empty<TAny>().AsQueryable();
        }

        #endregion

        #endregion
    }
}

