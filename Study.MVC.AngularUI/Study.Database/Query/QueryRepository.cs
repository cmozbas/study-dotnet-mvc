using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Study.Database.Query
{

    public interface IQueryRepository
    {
        Task<IEnumerable<T>> GetQueryFromXmlAsync<T>(string queryId, params object[] parameters) where T : new();
        Task<IEnumerable<T>> GetQueryFromStringAsync<T>(string query, params object[] parameters) where T : new();
    }

    public class QueryRepository : IQueryRepository
    {
        private StudyContext context;

        public QueryRepository(StudyContext context)
        {
            this.context = context;
        }


        public async Task<IEnumerable<T>> GetQueryFromXmlAsync<T>(string queryId, params object[] parameters) where T : new()
        {
            var result = new List<T>();

            string queryFilePath = "/Repositories/Query/Queries.xml";

            if (!File.Exists(queryFilePath))
                return result;

            XElement element = XElement.Load(queryFilePath);
            var res = element?.Elements("query")?.FirstOrDefault(n => n.Attribute("id").Value == queryId);

            if (res == null)
                return result;

            string query = res.Value;
            if (string.IsNullOrWhiteSpace(query))
                return result;

            // Replace parameters if any in query
            query = string.Format(query, parameters);
            var queryResult = await GetQueryResponse<T>(query);

            return queryResult;
        }

        public async Task<IEnumerable<T>> GetQueryFromStringAsync<T>(string query, params object[] parameters) where T : new()
        {
            var result = new List<T>();

            if (string.IsNullOrWhiteSpace(query))
                return result;

            // Replace parameters if any in query
            query = string.Format(query, parameters);
            var queryResult = await GetQueryResponse<T>(query);

            return queryResult;
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetQueryResponse<T>(string query) where T : new()
        {
            IEnumerable<T> result = null;

            var conn = this.context.Database.GetDbConnection();
            try
            {
                await conn.OpenAsync();
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    DbDataReader reader = await command.ExecuteReaderAsync();

                    result = reader.ConvertToObject<T>();

                    reader.Dispose();
                }
            }
            catch (Exception exp)
            {
                var msg = exp.Message;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }


    }

}
