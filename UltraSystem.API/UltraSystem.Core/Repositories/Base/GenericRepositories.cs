﻿using Dapper;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model.Core;
using static Dapper.SqlMapper;

namespace UltraSystem.Core
{
    public class GenericRepositories<T> : IGenericRepository<T> where T : class
    {
        public readonly IDbContext<T> _dbContext;
        public GenericRepositories(IDbContext<T> dbContext)
        {
            _dbContext = dbContext;
        }
        public async virtual Task<object> Add(T entity)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            var param = new Dictionary<string, object>();
            Type typeOfModel = typeof(T);
            PropertyInfo[] properties = typeOfModel.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                param.Add($"v_{propertyName}", property.GetValue(entity, null) ?? "");
            }
            var nameStore = $"Proc_Insert{textInfo.ToTitleCase(GetTableName())}";
            var resUpdate = await _dbContext.ExcuseUsingStore(param, nameStore);
            return resUpdate > 0;
        }

        public async virtual Task<bool> Delete(object ID)
        {
            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                var query = $"DELETE FROM {GetTableName()} WHERE {textInfo.ToTitleCase(GetTableName())}ID = @Id";
                var res = await connection.ExecuteAsync(query, new { Id = ID });
                return res > 0;
            }
        }

        public virtual Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async virtual Task<T> GetById(object ID)
        {
            var baseModel = (BaseModel)Activator.CreateInstance(typeof(T));
            using (var connection = _dbContext.CreateConnection())
            {
                connection.Open();
                var query = $"SELECT * FROM {baseModel.GetTableName() ?? GetTableName()} WHERE {baseModel?.GetPrimaryKey()} = @Id";
                var param = new Dictionary<string, object>() { { "@Id", ID } };
                var rs = await connection.QueryAsync<T>(query, param);
                if (rs.Any())
                {
                    return rs.FirstOrDefault();
                }
                return null;
            }
        }

        public virtual string GetTableName()
        {
            throw new NotImplementedException();
        }

        public async Task<object> MultiInsert(List<BaseModel> data,IDbTransaction dbTransaction,bool selectKey)
        {
            if (data == null || !data.Any())
            {
                return true;
            }
            var param = new Dictionary<string , object>();
            var queryInsert = BuildQueryMultiInsert(data,ref param, selectKey).ToString() ?? "";
            var resInsert = await _dbContext.ExecuteScalarUsingStore(param, queryInsert, dbTransaction, CommandType.Text);
            if (selectKey)
            {
                return resInsert;
            }
            else
            {
                return Convert.ToInt32(resInsert) > 0;
            }
        }

        private object BuildQueryMultiInsert(List<BaseModel> data, ref Dictionary<string, object> param, bool selectKey)
        {
            var sql = new StringBuilder();
            var tableName = data[0].GetTableName();
            var columnKey = data[0].GetPrimaryKey();
            var columns = new List<string>();
            var columnsInTable =  this.GetColumnByTableName(tableName).Result;
            int count = 0;
            bool isFirst = true;
            foreach (var column in columnsInTable)
            {
                if (data[0].ContainProperty(column) && column != columnKey)
                {
                    columns.Add(column);
                }
            }
            sql.AppendLine($"Insert Into {tableName} ({string.Join(",", columns)}) values");
            foreach (var basemodel in data)
            {
                if (isFirst)
                {
                    sql.AppendLine($"({"@" + string.Join($"_{count},@", columns)}_{count})");
                    isFirst = false;
                }
                else
                {
                    sql.AppendLine($",({"@" + string.Join($"_{count},@", columns)}_{count})");
                }
                foreach (var col in columns)
                {
                    param.Add($"@{col}_{count}", basemodel.GetValue(col, basemodel));
                }
                count += 1;
            }
            sql.AppendLine(";");
            if (selectKey)
            {
                sql.AppendLine("SELECT LAST_INSERT_ID();");
                // nếu insert User
               // sql.appendline("select 'models[0].getPrimaryKeyValue()'");
            }
            else
            {
                sql.AppendLine("SELECT ROW_COUNT();");
            }
            return sql.ToString();
        }
        private async Task<List<string>> GetColumnByTableName(string tableName)
        {
            var param = new Dictionary<string, object>();
            param.Add("@TableName", tableName);
            var sql = $"SELECT c.COLUMN_NAME from information_schema.Columns c where c.TABLE_NAME = @TableName and c.TABLE_SCHEMA = database() order by c.ORDINAL_POSITION;";
            var columns = await _dbContext.QueryUsingStore<string>(param, sql,commandType:CommandType.Text);
            return columns.ToList();
;        }

        public async virtual Task<bool> Update(T entity)
        {
            var param = new Dictionary<string, object>();
            Type typeOfModel = typeof(T);
            PropertyInfo[] properties = typeOfModel.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                param.Add($"v_{propertyName}", property.GetValue(entity, null) ?? "");
            }
            var nameStore = $"Proc_Update{GetTableName()}";
            var resUpdate = await _dbContext.ExcuseUsingStore(param, nameStore);
            return resUpdate > 0;
        }

        public async Task<bool> UpdateCustomColumn(T entity, List<string> columnsToUpdate, string condition, IDbTransaction? transaction = null)
        {
            string setClause = string.Join(", ", columnsToUpdate.Select(col => col + " = @" + col));
            string query = $"UPDATE {GetTableName()} SET {setClause} WHERE {condition}";
            using (var db = _dbContext.CreateConnection())
            {
                var param = new Dictionary<string, object>();
                Type type = typeof(T);
                // Add parameters for each column in the list
                foreach (string column in columnsToUpdate)
                {
                    var property = type.GetProperty(column.ToString());
                    if (property != null)
                    {
                        param.Add($"@{column}", property.GetValue(entity) ?? "");
                    }
                }
                if (transaction != null)
                {
                    var res = await transaction.Connection.ExecuteAsync(query, param);
                    return res > 0;
                }
                else
                {
                    var res = await db.ExecuteAsync(query, param);
                    return res > 0;
                }

            }
        }
    }
}
