using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Core.Model.RequestModel;

namespace UltraSystem.Core.Repositories
{
    public class RolePermisstionRepository : GenericRepositories<RolePermisstion>, IRolePermisstionRepository
    {
        public RolePermisstionRepository(IDbContext<RolePermisstion> dbContext) : base(dbContext)
        {
        }

        public async Task<List<RolePermisstion>> GetRolePermisstionsByUserID(string UserID)
        {
            var param = new Dictionary<string, object>
            {
                {"v_UserID",UserID }
            };
            var rs = new List<RolePermisstion>();
            using (var db = _dbContext.CreateConnection())
            {
                rs = await _dbContext.QueryUsingStore(param, "Proc_GetRoleByUserID") as List<RolePermisstion>;
            }
            return rs;
        }
        public async Task<object> InsertRolePermisstion(RolePermisstionRequestModel model)
        {
            var param = new Dictionary<string, object>
            {
                {"@RoleName", model.RoleName ?? ""}
            };
            using(var db = _dbContext.CreateConnection())
            {
                db.Open();
                var transaction = db.BeginTransaction();
                var sqlInsert = "INSERT INTO role (RoleName) VALUES (@RoleName);SELECT  LAST_INSERT_ID();";
                var resInsertRole = await _dbContext.ExecuteScalarUsingStore(param, sqlInsert, transaction, CommandType.Text);
                var roleId = int.Parse(resInsertRole.ToString() ?? "0");
                if (roleId <= 0)
                {
                    transaction.Rollback();
                    return null;
                }
                var listPermisstionDetail = new List<PermisstionDetail>();
                foreach (var item in model.PermissionIDs)
                {
                    var permistionDetail = new PermisstionDetail() 
                    {
                        RoleID = roleId,
                        PermisstionID = item
                    };
                    listPermisstionDetail.Add(permistionDetail);
                }
                var dataPermisstion = listPermisstionDetail.Cast<BaseModel>().ToList();
                var resInsertPermisstion = await MultiInsert(dataPermisstion, transaction, false);
                if (Convert.ToBoolean(resInsertPermisstion))
                {
                    transaction.Commit();
                    return roleId;
                }
            }
            return null;
        }
        public async Task<List<RolePermisstion>> GetAllRolePermisstions()
        {
            var rs = new List<RolePermisstion>();
            var sql = "  SELECT r.RoleID,p1.PermisstionName,r.RoleName,p.PermisstionID FROM user u JOIN role r ON u.RoleID = r.RoleID JOIN permisstiondetail p ON r.RoleID = p.RoleID JOIN permisstion p1 ON p1.PermisstionID = p.PermisstionID;";
            using (var db = _dbContext.CreateConnection())
            {
                rs = await _dbContext.QueryUsingStore(null, sql, commandType: CommandType.Text) as List<RolePermisstion>;
            }
            return rs.ToList();
        }

    }
}
