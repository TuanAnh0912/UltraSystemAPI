using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.ResponseModel;

namespace UltraSystem.Core.Repositories
{
    public class UserRepository : GenericRepositories<User>, IUserRepository
    {

        public UserRepository(IDbContext<User> dbContext) : base(dbContext)
        {

        }
        public async Task<User?> CheckLogin(string username, string password)
        {
            var param = new Dictionary<string, object>
                {
                    {"v_UserName",username },
                    {"v_Password", password }
                };
            var rsCheckLogin = (await _dbContext.QueryUsingStore(param, "Proc_CheckLogin")).FirstOrDefault();
            return rsCheckLogin;
        }
        public async Task<User> CheckByUserNameAndEmail(string userName, string email)
        {
            var param1 = new Dictionary<string, object>
            {
                {"v_Email",email},
                {"v_UserName", userName }

            };
            return (await _dbContext.QueryUsingStore(param1, "Proc_CheckUserNameAndEmail")).FirstOrDefault();
        }
        public async override Task<object> Add(User entity)
        {
            var param2 = new Dictionary<string, object>
            {
                {"v_UserID", Guid.NewGuid()},
                {"v_UserName", entity.UserName ?? ""},
                {"v_HashPassword",entity.HashPassword ?? ""},
                {"v_Email",entity.Email ?? ""},
                {"v_RefreshToken", ""},
            };
            using (var db = _dbContext.GetDbTransaction())
            {
                var transaction = _dbContext.GetDbTransaction();
                var sql = "select * from Role;";
                var allRoles = await _dbContext.QueryUsingStore<RolePermisstion>(null, sql, transaction, CommandType.Text);
                if (!allRoles.Any())
                {
                    transaction.Rollback();
                    return 0;
                }
                var userRole = allRoles.Where(x => x.RoleName == "USER").FirstOrDefault();
                param2.Add("v_RoleID", userRole?.RoleID ?? 0);
                var rsInsert = await _dbContext.ExcuseUsingStore(param2, "Proc_InsertUser", transaction);
                if (rsInsert > 0)
                {
                    transaction.Commit();
                }
                return rsInsert;
            }
        }
        public async Task<User?> GetUserByRefreshToken(string resfreshToken)
        {
            var param = new Dictionary<string, object>()
            {
                {"@ResfreshToken",resfreshToken}
            };
            var sql = "select * from user u where u.RefreshToken = @ResfreshToken;";
            var rsGetUser = await _dbContext.QueryUsingStore(param, sql, commandType: CommandType.Text);
            return rsGetUser?.FirstOrDefault() ?? null;
        }
        public async Task<List<UserLicenseFromModel>> GetUserAndLicensOwner()
        {
            var sql = "select * from user as u join purchasedproduct as pp on u.UserID = pp.UserID join license as l on l.LicenseID = pp.LicenseID where l.IsActive = 1;";
            using (var db = _dbContext.CreateConnection())
            {
                var resGetAll = await db.QueryAsync<UserLicenseFromModel>(sql, commandType: CommandType.Text);
                return resGetAll.ToList();
            }
        }
        public async Task<bool> UpdateAcountBalance(decimal acountBalance, Guid userID, IDbTransaction? transaction = null)
        {
            var sql = @"Update user set AccountBalances = @AccountBalances where UserID = @UserID";
            var param = new Dictionary<string, object>()
            {
                {"@AccountBalances", acountBalance},
                {"@UserID", userID}
            };
            var rssUpdate = await _dbContext.ExcuseUsingStore(param, sql, transaction, CommandType.Text);
            return rssUpdate > 0;
        }
        public override string GetTableName()
        {
            return "user";
        }
    }
}
