using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SystemServiceAPICore3.Repositories.Interfaces;

namespace SystemServiceAPICore3.Domain.Helpers
{
    public static class EntityBoHelper
    {
        public static IEnumerable<TEntity> GetModels<TEntityDto, TEntity>(IGenericRepository<TEntity> repository, params TEntityDto[] dtoList)
            where TEntityDto : class
            where TEntity : class
        {
            // Get key expression list.
            var keyExpress = GetExpressionKey<TEntityDto, TEntity>(dtoList);

            // Get model list based on expression keys.
            return repository.FindBy(keyExpress);
        }

        public static async Task<IEnumerable<TEntity>> GetModelsAsync<TEntityDto, TEntity>(IGenericRepository<TEntity> repository, params TEntityDto[] dtoList)
            where TEntityDto : class
            where TEntity : class
        {
            // Get key expression list.
            var keyExpress = GetExpressionKey<TEntityDto, TEntity>(dtoList);

            // Get model list based on expression keys.
            return await repository.FindByAsync(keyExpress);
        }

        public static Expression<Func<TEntity, bool>> GetExpressionKey<TEntityDto, TEntity>(params TEntityDto[] dtoList)
        {
            // Build expression key.
            Expression<Func<TEntity, bool>> express = null;
            var keyDtoProperties = EntityHelper.GetPrimaryKeys<TEntityDto>();
            var keyEntityProperties = EntityHelper.GetPrimaryKeys<TEntity>();

            foreach (var dto in dtoList)
            {
                // Get key expression.
                var keyValues = keyDtoProperties.Select(property => property.GetValue(dto))
                                          .ToArray();

                // Get key express.
                var keyExp = EntityHelper.GetKeyExpression<TEntity>(keyValues, keyEntityProperties);

                // Merge key expression.
                //express = express == null ? keyExp : express.OrElse(keyExp);
            }

            return express;
        }
    }
}
