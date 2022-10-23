using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SystemServiceAPICore3.Domain.Helpers
{
    public static class EntityHelper
    {
        public static object[] GetActualKeys<TEntity>(params string[] value)
        {
            var keyPros = GetPrimaryKeys<TEntity>();
            if (keyPros.Length != value.Length)
            {
                return Array.Empty<object>();
            }

            var list = new List<object>();
            for (var i = 0; i < keyPros.Length; i++)
            {
                var objKey = ChangeType(keyPros[i].PropertyType, value[i]);
                list.Add(objKey);
            }

            return list.ToArray();
        }

        public static PropertyInfo[] GetPrimaryKeys<TEntity>()
        {
            // Get properties key list.
            var properties = typeof(TEntity).GetProperties()
                .Where(IsDefineKey)
                .ToList();

            // Sort keys with column order.
            properties.Sort(CompareColumnAttribute);

            // Return value.
            return properties.ToArray();
        }

        public static object[] GetPrimaryKeyValues<TEntity>(TEntity entity)
        {
            // Get PropertyInfo list.
            var properties = GetPrimaryKeys<TEntity>();

            // Get value.
            var listObjs = properties.Select(property => property.GetValue(entity)).ToList();

            // Return value.
            return listObjs.ToArray();
        }

        public static bool IsDefineKey(PropertyInfo property)
        {
            return Attribute.IsDefined(property, typeof(KeyAttribute)) || property.Name.ToUpper() == "ID";
        }

        private static int CompareColumnAttribute(MemberInfo x, MemberInfo y)
        {
            // Get column attribute.
            var xColAtt = x.GetCustomAttribute<ColumnAttribute>();
            var yColAtt = y.GetCustomAttribute<ColumnAttribute>();

            if (xColAtt == null && yColAtt == null)
            {
                return 0;
            }

            if (xColAtt == null)
            {
                return -1;
            }

            if (yColAtt == null)
            {
                return 1;
            }

            // In case exist column attribute both x,y. Compare its column order.
            return xColAtt.Order.CompareTo(yColAtt.Order);
        }

        public static object ChangeType(Type t, object value)
        {
            var tc = TypeDescriptor.GetConverter(t);
            return tc.ConvertFrom(value?.ToString() ?? string.Empty);
        }

        public static Expression<Func<TEntity, object>>[] GetKeySortExpression<TEntity>()
        {
            var expressions = new List<Expression<Func<TEntity, object>>>();

            // Get expression based on each key.
            var keys = GetPrimaryKeys<TEntity>();
            foreach (var key in keys)
            {
                var express = Build<TEntity>(key.Name);
                expressions.Add(express);
            }

            return expressions.ToArray();
        }

        public static Expression<Func<TEntity, object>> Build<TEntity>(string fieldName)
        {
            var param = Expression.Parameter(typeof(TEntity));
            var field = Expression.PropertyOrField(param, fieldName);

            return Expression.Lambda<Func<TEntity, object>>(field, param);
        }

        public static Expression<Func<TEntity, bool>> GetKeyExpression<TEntity>(object[] ids)
        {
            // Get key property list.
            var keys = GetPrimaryKeys<TEntity>();

            return GetKeyExpression<TEntity>(ids, keys);
        }

        public static Expression<Func<TEntity, bool>> GetKeyExpression<TEntity>(object[] ids, PropertyInfo[] keys)
        {
            // Validate entity.
            if (ids.Length != keys.Length)
            {
                return null;
            }

            var argParam = Expression.Parameter(typeof(TEntity));
            BinaryExpression andExp = null;
            for (var i = 0; i < keys.Length; i++)
            {
                // Get Property key infor.
                var key = keys[i];
                var name = key.Name;
                var type = key.PropertyType;
                var id = ids[i];

                // Build property expression.
                var pro = Expression.Property(argParam, name);
                var val = Expression.Constant(id, type);
                var exp = Expression.Equal(pro, val);

                //andExp = andExp == null ? exp : Expression.AndAssign(andExp, exp);
                andExp = andExp == null ? exp : Expression.AndAlso(andExp, exp);
            }

            // Get lambda expression.
            var lambda = andExp != null ? Expression.Lambda<Func<TEntity, bool>>(andExp, argParam)
                : null;

            return lambda;
        }

        public static string GetNameValue<T>(params T[] objects)
        {
            // Validate param.
            if (objects == null)
            {
                return string.Empty;
            }

            // Get PropertyInfo list.
            var data = new StringBuilder(typeof(T).FullName);
            var properties = typeof(T).GetProperties();

            data.AppendLine("[");
            foreach (var entity in objects)
            {
                data.AppendLine("{");

                foreach (var property in properties)
                {
                    // Get name-value of property.
                    var name = property.Name;
                    var value = property.GetValue(entity);
                    var msg = IsDefineKey(property) ? $"[Key]{name} = {value};"
                                                    : $"{name} = {value},";
                    data.AppendLine(msg);
                }

                data.AppendLine("},");
            }
            data.AppendLine("]");

            // Return value.
            return data.ToString();
        }
    }
}
