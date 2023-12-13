using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using UltraSystem.Application.Helpers;
using UltraSystem.Application.Interface;
using UltraSystem.Core.Interface;
using UltraSystem.Core.Model;
using UltraSystem.Core.Model.Core;
using UltraSystem.Core.Model.RequestModel;
using UltraSystem.Core.Model.ResponseModel;
using static Dapper.SqlMapper;

namespace UltraSystem.Application.Service
{
    public class UsersService : BaseService<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailService _mailService;
        private readonly IJwtUtils _jwtUtils;
        private readonly IRolePermisstionRepository _rolePermisstionRepository;
        public UsersService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
            _rolePermisstionRepository = serviceProvider.GetRequiredService<IRolePermisstionRepository>();
            _mailService = serviceProvider.GetRequiredService<IMailService>();
            _jwtUtils = serviceProvider.GetRequiredService<IJwtUtils>();
        }
        public async override Task<ServiceResponse> Add(User entity)
        {
            if (entity == null)
            {
                return new ServiceResponse(false, "Dữ liệu trống");
            }
            var validateUser = ValidateUser(entity);
            if (!string.IsNullOrEmpty(validateUser))
            {
                return new ServiceResponse(false, validateUser);
            }
            var checkExitsUser = await _userRepository.CheckByUserNameAndEmail(entity.UserName ?? "", entity.Email ?? "");
            if (checkExitsUser == null)
            {
                entity.HashPassword = AuthenHelpers.HashPassword(entity.UserName + entity.Password);
                var rsInsert = await _userRepository.Add(entity);
                if (Convert.ToInt32(rsInsert) > 0)
                {
                    return new ServiceResponse(true, "Tạo thành công");
                }
            }
            return new ServiceResponse(false, "Tài khoản đã tồn tại");
        }
        public async Task<ServiceResponse> GetUserLicenses()
        {
            var userLisenseFromDb = await _userRepository.GetUserAndLicensOwner();
            if (userLisenseFromDb == null)
            {
                return new ServiceResponse(false, "Lấy dữ liệu thất bại");
            }
            var dicUserLicense = userLisenseFromDb.GroupBy(x => x.UserID).ToDictionary(g => g.Key, g => g.ToList());
            var rsData = new List<UserLicenseResponseModel>();
            foreach (var item in dicUserLicense)
            {
                var userInfor = new UserLicenseResponseModel()
                {
                    UserID = item.Value[0].UserID,
                    UserName = item.Value[0].UserName,
                    Email = item.Value[0].Email,
                    RoleID = item.Value[0].RoleID
                };
                foreach (var userLicense in item.Value)
                {
                    var licenseInfor = new UserLicenseFromModel()
                    {
                        PurchasedProductID = userLicense.PurchasedProductID,
                        LicenseID = userLicense.LicenseID,
                        PurchaseDate = userLicense.PurchaseDate,
                        KeyID = userLicense.KeyID,
                        ProductID = userLicense.ProductID,
                        IsActive = userLicense.IsActive,
                        ExpiryDate = userLicense.ExpiryDate,
                        Price = userLicense.Price,
                        MaxLicenseCount = userLicense.MaxLicenseCount,
                    };
                    userInfor.licensesInfor.Add(licenseInfor);
                }
                rsData.Add(userInfor);
            }
            return new ServiceResponse(true, "Lấy dữ liệu thành công", rsData);
        }
        public async Task<ServiceResponse> GetAllRole()
        {
            var lstRoles = await _rolePermisstionRepository.GetAllRolePermisstions();
            if (!lstRoles.Any())
            {
                return new ServiceResponse(true, "Du lieu trong", lstRoles);
            }
            var resData = new List<RolePermisstionsResponseModel>();
            var dicRoles = lstRoles.GroupBy(x => x.RoleID).ToDictionary(k => k.Key, g => g.ToList());
            foreach (var dicRole in dicRoles)
            {
                var rolePermisstion = new RolePermisstionsResponseModel();
                var key = dicRole.Key;
                rolePermisstion.RoleID = key;
                rolePermisstion.RoleName = dicRole.Value[0]?.RoleName ?? "";
                var lstPermisstion = new List<Permisstion>();
                foreach (var item in dicRole.Value)
                {
                    var permisstion = new Permisstion();
                    permisstion.PermisstionID = item.PermisstionID;
                    permisstion.PermisstionName = item.PermisstionName;
                    lstPermisstion.Add(permisstion);
                }
                rolePermisstion.Permisstions = lstPermisstion;
                resData.Add(rolePermisstion);
            }
            return new ServiceResponse(true, "Lấy dữ liệu thành công", resData);
        }
        public async Task<ServiceResponse> InsertRolePermisstion(RolePermisstionRequestModel model)
        {
            var res = await _rolePermisstionRepository.InsertRolePermisstion(model);
            if (res == null)
            {
                return new ServiceResponse(false, "Có lỗi xảy ra");
            }
            return new ServiceResponse(true, "Thêm thành công", res);
        }
        public string ValidateUser(User user)
        {
            var validateUser = ValidationHelper.ValidatEmpty(user.UserName ?? "", "Tài khoản");
            if (!string.IsNullOrEmpty(validateUser))
            {
                return validateUser;
            }
            var validatePassword = ValidationHelper.ValidatEmpty(user.Password ?? "", "Mật khẩu");
            if (!string.IsNullOrEmpty(validatePassword))
            {
                return validatePassword;
            }
            var validateEmail = ValidationHelper.IsValidEmail(user.Email ?? "");
            if (!string.IsNullOrEmpty(validateEmail))
            {
                return validateEmail;
            }
            return "";
        }
        public async override Task<ServiceResponse> GetById(object ID)
        {
            try
            {
                var data = await base.GetById(_UserID);
                return new ServiceResponse(true, "lay du lieu thanh cong", data);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, ex.Message);
            }
        }
        public async Task<ServiceResponse> SendMail()
        {
            var link = _jwtUtils.GenerateResetLink();
            var mailRs = _mailService.SendMail(_Email, "abc", link);
            return new ServiceResponse(true, "Gửi thành công");
        }
        public async Task<ServiceResponse> ResetPassword(string newPassword)
        {
            var hashPassWord = AuthenHelpers.HashPassword(_UserName + newPassword);
            var userEntity = new User()
            {
                HashPassword = hashPassWord,
            };
            var condition = $"UserID = '{_UserID}'";
            var resUpdate = await _userRepository.UpdateCustomColumn(userEntity, new List<string>() { "HashPassword" }, condition);
            if (resUpdate)
            {
                return new ServiceResponse(true, "Mật khẩu đã thay đổi");
            }
            return new ServiceResponse(false, "Có lỗi xảy ra");
        }
    }
}
