using Galery.Server.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Galery.Server.DAL.Helpers
{
    public static class QueryBuilder
    {
        static Dictionary<string, string> CreateQueries = new Dictionary<string, string>();
        static Dictionary<string, string> UpdateQueries = new Dictionary<string, string>();
        static Dictionary<string, string> m2mJoinQueries = new Dictionary<string, string>();

        public static string CreateQuery(object obj)
        {
            var type = obj.GetType();

            if (CreateQueries.ContainsKey(type.Name))
                return CreateQueries[type.Name];

            var props = type.GetProperties().Where(e=>e.Name!="Id");

            var sb = new StringBuilder();
            sb.Append($"insert into [{type.Name}](");

            foreach (var prop in props)
            {
                sb.Append($"[{prop.Name}],");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(") values(");

            foreach (var prop in props)
            {
                sb.Append($"@{prop.Name},");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(");" +
                "SELECT CAST(SCOPE_IDENTITY() as int)");

            var result = sb.ToString();
            CreateQueries.Add(type.Name, result);
            return result;
        }

        public static string UpdateQuery(object obj)
        {
            var type = obj.GetType();

            if (UpdateQueries.ContainsKey(type.Name))
                return UpdateQueries[type.Name];

            var props = type.GetProperties();

            var sb = new StringBuilder();
            sb.Append($"UPDATE [{type.Name}] SET");

            foreach (var prop in props.Where(e=>e.Name != "Id"))
            {
                sb.Append($"[{prop.Name}] = @{prop.Name},");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append(" WHERE [Id] = @Id");

            var result = sb.ToString();
            UpdateQueries.Add(type.Name, result);
            return result;
        }

        public static string m2mJoinQuery<Entity1, Entity2>(Expression<Func<Entity1, object>> joinedProp1,
            Expression<Func<Entity2, object>> joinedProp2, Expression<Func<Entity2, object>> filterParam, string paramName)
        {
            Type type1 = typeof(Entity1);
            Type type2 = typeof(Entity2);
            string key = $"{type1} {nameof(Entity2)}";

            if (m2mJoinQueries.ContainsKey(key))
                return m2mJoinQueries[key];

            string variable2 = type2.Name.ToUpper();
            string variable1 = type1.Name.ToUpper();

            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            foreach (var prop in type1.GetProperties())
                sb.Append($"{variable1}.[{prop.Name}], ");
            sb.Remove(sb.Length - 2, 1);

            string fromStr = $" FROM [{type2.Name}] AS {variable2} JOIN [{type1.Name}] AS {variable1} " +
                $"ON {variable1}.[{PropertyName(joinedProp1)}] = {variable2}.[{PropertyName(joinedProp2)}]" +
                $" WHERE {variable2}.[{PropertyName(filterParam)}] = @{paramName}";

            sb.Append(fromStr);
            string result = sb.ToString();
            m2mJoinQueries.Add(key, result);
            return result;
        }

        public static string TakeSkipQuery<T>(Expression<Func<T, object>> orderByProp, int? skip, int? take)
        {
            var sb = new StringBuilder();
            sb.Append($"ORDER BY [{PropertyName(orderByProp)}]");

            if(skip != null && skip.Value > 0)
            {
                sb.Append($" OFFSET {skip.Value} ROWS ");
            }
            if(take!= null && take.Value > 0)
            {
                sb.Append($"FETCH NEXT {take.Value} ROWS ONLY");
            }
            return sb.ToString();
        }


        static string PropertyName<T>(Expression<Func<T, object>> expr)
        {
            var parameter = expr.Parameters[0];
            if(expr.Body is UnaryExpression unary)
            {
                return unary.Operand.ToString().Replace($"{parameter.Name}.", "");
            }
            throw new Exception("Не удалось составить строку команды");
        }
    }
}
