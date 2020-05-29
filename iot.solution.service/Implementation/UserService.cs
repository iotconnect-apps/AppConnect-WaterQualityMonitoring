using component.helper;
using component.logger;
using iot.solution.common;
using iot.solution.model.Repository.Interface;
using iot.solution.service.Interface;
using IoTConnect.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Entity = iot.solution.entity;
using IOT = IoTConnect.Model;
using Model = iot.solution.model.Models;

namespace iot.solution.service.Data
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;
        private readonly IotConnectClient _iotConnectClient;

        public UserService(IUserRepository userRepository, ILogger logManager)
        {
            _logger = logManager;
            _userRepository = userRepository;
            _iotConnectClient = new IotConnectClient(component.helper.SolutionConfiguration.BearerToken, component.helper.SolutionConfiguration.Configuration.EnvironmentCode, component.helper.SolutionConfiguration.Configuration.SolutionKey);
        }
        public List<Entity.User> Get()
        {
            try
            {
                return _userRepository.GetAll().Where(e => !e.IsDeleted).Select(p => Mapper.Configuration.Mapper.Map<Entity.User>(p)).ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, $"UserService.GetAll, Error: {ex.Message}");
                return null;
            }
        }
        public Entity.User Get(Guid id)
        {
            try
            {
                return _userRepository.FindBy(r => r.Guid == id).Select(p => Mapper.Configuration.Mapper.Map<Entity.User>(p)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, $"UserService.Get, Error: {ex.Message}");
                return null;
            }
        }
        public Entity.SearchResult<List<Entity.UserResponse>> List(Entity.SearchRequest request)
        {
            try
            {
                //var result = _userRepository.List(request);
                //return result;
                var result = _userRepository.List(request);
                return new Entity.SearchResult<List<Entity.UserResponse>>()
                {
                    Items = result.Items.Select(p => Mapper.Configuration.Mapper.Map<Entity.UserResponse>(p)).ToList(),
                    Count = result.Count
                };
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, $"UserService.List, Error: {ex.Message}");
                return new Entity.SearchResult<List<Entity.UserResponse>>();
            }
        }
        public Entity.ActionStatus Manage(Entity.AddUserRequest request)
        {
            Entity.ActionStatus actionStatus = new Entity.ActionStatus(true);
            try
            {
                if (request.Id == null || request.Id == Guid.Empty)
                {
                    //var addUserResult = _iotConnectClient.User.Add(Mapper.Configuration.Mapper.Map<IOT.AddUserModel>(request)).Result;
                    var addUserResult = AsyncHelpers.RunSync<IOT.DataResponse<IOT.AddUserResult>>(() =>
                       _iotConnectClient.User.Add(Mapper.Configuration.Mapper.Map<IOT.AddUserModel>(request)));

                    if (addUserResult != null && addUserResult.status && addUserResult.data != null)
                    {
                        request.Id = Guid.Parse(addUserResult.data.newId.ToUpper());
                        var dbUser = Mapper.Configuration.Mapper.Map<Entity.AddUserRequest, Model.User>(request);
                        dbUser.Guid = request.Id;
                        dbUser.CreatedDate = DateTime.Now;
                        dbUser.CompanyGuid = SolutionConfiguration.CompanyId;
                        dbUser.CreatedBy = SolutionConfiguration.CurrentUserId;
                        dbUser.IsActive = true;
                        dbUser.IsDeleted = false;
                        dbUser.EntityGuid = request.EntityGuid.Value;
                        var tmpdbUser = _userRepository.FindBy(u => u.Guid == dbUser.Guid).FirstOrDefault();
                        if (tmpdbUser == null)
                        {
                            actionStatus = _userRepository.Insert(dbUser);
                        }
                        else
                        {
                            actionStatus = new Entity.ActionStatus()
                            {
                                Data = tmpdbUser,
                                Message = "",
                                Success = true
                            };
                        }

                        actionStatus.Data = Mapper.Configuration.Mapper.Map<Model.User, Entity.UserResponse>(actionStatus.Data);
                        if (!actionStatus.Success)
                        {
                            _logger.Error($"User is not added in solution database, Error: {actionStatus.Message}");
                            var deleteEntityResult = _iotConnectClient.User.Delete(request.Id.ToString()).Result;
                            if (deleteEntityResult != null && !deleteEntityResult.status)
                            {
                                _logger.Error($"User is not deleted from iotconnect, Error: {deleteEntityResult.message}");
                                actionStatus.Success = false;
                                actionStatus.Message = new UtilityHelper().IOTResultMessage(deleteEntityResult.errorMessages);
                            }
                        }
                    }
                    else
                    {
                        _logger.Error($"User is not added in iotconnect, Error: {addUserResult.message}");
                        actionStatus.Success = false;
                        actionStatus.Message = new UtilityHelper().IOTResultMessage(addUserResult.errorMessages);
                    }
                }
                else
                {
                    var olddbUser = _userRepository.FindBy(x => x.Guid.Equals(request.Id)).FirstOrDefault();
                    if (olddbUser == null)
                    {
                        throw new NotFoundCustomException($"{CommonException.Name.NoRecordsFound} : User");
                    }

                    //var updateEntityResult = _iotConnectClient.User.Update(request.Id.ToString(), Mapper.Configuration.Mapper.Map<IOT.UpdateUserModel>(request)).Result;
                    var updateEntityResult = AsyncHelpers.RunSync<IOT.DataResponse<IOT.UpdateUserResult>>(() =>
                       _iotConnectClient.User.Update(request.Id.ToString(), Mapper.Configuration.Mapper.Map<IOT.UpdateUserModel>(request)));

                    if (updateEntityResult != null && updateEntityResult.status && updateEntityResult.data != null)
                    {
                        bool? olddbUserStatus = olddbUser.IsActive;
                        var dbUser = Mapper.Configuration.Mapper.Map(request, olddbUser);
                        //Update Status if user is not modifying his own profile
                        if (request.Id != SolutionConfiguration.CurrentUserId && olddbUserStatus != request.IsActive)
                        {                           
                                var updateStatusEntityResult = AsyncHelpers.RunSync<IOT.DataResponse<IOT.UpdateUserStatusResult>>(() =>
                           _iotConnectClient.User.UpdateUserStatus(request.Id.ToString(), request.IsActive));
                            if (updateStatusEntityResult != null && updateStatusEntityResult.status && updateStatusEntityResult.data != null && updateStatusEntityResult.errorMessages.Count > 0)
                            {
                                _logger.Error($"User is not updated in iotconnect, Error: {updateStatusEntityResult.message}");
                                actionStatus.Success = false;
                                actionStatus.Message = new UtilityHelper().IOTResultMessage(updateStatusEntityResult.errorMessages);
                                return actionStatus;
                            }
                        }
                        else
                        {
                            dbUser.IsActive = updateEntityResult.data.isActive;
                        }
                        //
                        dbUser.CreatedBy = olddbUser.CreatedBy;
                        dbUser.CreatedDate = olddbUser.CreatedDate;
                        dbUser.UpdatedDate = DateTime.Now;
                        dbUser.UpdatedBy = component.helper.SolutionConfiguration.CurrentUserId;
                        dbUser.CompanyGuid = component.helper.SolutionConfiguration.CompanyId;
                        actionStatus = _userRepository.Update(dbUser);
                        actionStatus.Data = Mapper.Configuration.Mapper.Map<Model.User, Entity.UserResponse>(actionStatus.Data);
                        if (!actionStatus.Success)
                        {
                            _logger.Error($"User is not updated in solution database, Error: {actionStatus.Message}");
                            actionStatus.Success = false;
                            actionStatus.Message = "Somthing went wrong!";
                        }
                    }
                    else
                    {
                        _logger.Error($"User is not added in iotconnect, Error: {updateEntityResult.message}");
                        actionStatus.Success = false;
                        actionStatus.Message = new UtilityHelper().IOTResultMessage(updateEntityResult.errorMessages);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, "UserManager.Delete " + ex);
                actionStatus.Success = false;
                actionStatus.Message = ex.Message;
            }
            return actionStatus;
        }
        public Entity.ActionStatus Delete(Guid id)
        {
            Entity.ActionStatus actionStatus = new Entity.ActionStatus(true);
            try
            {
                var dbUser = _userRepository.FindBy(x => x.Guid.Equals(id)).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new NotFoundCustomException($"{CommonException.Name.NoRecordsFound} : User");
                }

                var deleteEntityResult = _iotConnectClient.User.Delete(id.ToString()).Result;
                if (deleteEntityResult != null && deleteEntityResult.status)
                {
                    dbUser.IsDeleted = true;
                    dbUser.UpdatedDate = DateTime.Now;
                    dbUser.UpdatedBy = component.helper.SolutionConfiguration.CurrentUserId;
                    actionStatus = _userRepository.Update(dbUser);
                    actionStatus.Data = Mapper.Configuration.Mapper.Map<Model.User, Entity.User>(actionStatus.Data);
                    if (!actionStatus.Success)
                        _logger.Error($"User is not deleted in solution database, Error: {actionStatus.Message}");
                }
                else
                {
                    _logger.Error($"User is not deleted from iotconnect, Error: {deleteEntityResult.message}");
                    actionStatus.Success = false;
                    actionStatus.Message = new UtilityHelper().IOTResultMessage(deleteEntityResult.errorMessages);  // //  IOTResultMessage  ; //"Something Went Wrong!";
                }
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, "UserManager.Delete " + ex);
                actionStatus.Success = false;
                actionStatus.Message = ex.Message;
            }
            return actionStatus;
        }
        public Entity.ActionStatus ChangePassword(Entity.ChangePasswordRequest request)
        {
            Entity.ActionStatus actionStatus = new Entity.ActionStatus(true);
            try
            {
                var dbUser = _userRepository.GetByUniqueId(x => x.Guid == component.helper.SolutionConfiguration.CurrentUserId);
                if (dbUser == null)
                {
                    throw new NotFoundCustomException($"{CommonException.Name.NoRecordsFound} : User");
                }
                request.Email = dbUser.Email;
                var changePasswordResults = _iotConnectClient.User.ChangePassword(Mapper.Configuration.Mapper.Map<IOT.ChangePasswordModel>(request)).Result;

                if (changePasswordResults != null && changePasswordResults.status && changePasswordResults.data != null)
                {
                    return new Entity.ActionStatus(true);
                }
                actionStatus.Success = false;
                actionStatus.Message = new UtilityHelper().IOTResultMessage(changePasswordResults.errorMessages);

            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, "UserManager.InsertUser " + ex);
                return new Entity.ActionStatus
                {
                    Success = false,
                    Message = ex.Message
                };
            }
            return actionStatus;
        }
        public Entity.ActionStatus Login(Entity.LoginRequest request)
        {
            Entity.ActionStatus result = new Entity.ActionStatus(true);
            try
            {
                IOT.DataResponse<IOT.LoginResult> loginResult = _iotConnectClient.Login.Login(new IOT.LoginModel()
                {
                    UserName = request.Username,
                    Password = request.Password
                }).Result;

                result.Success = loginResult.status;
                if (loginResult != null && loginResult.status)
                {
                    JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                    var tokenS = hand.ReadJwtToken(loginResult.data.access_token);
                    var jsonValue = tokenS.Claims?.SingleOrDefault(p => p.Type == "user")?.Value;
                    Entity.UserDetail userDetail = Newtonsoft.Json.JsonConvert.DeserializeObject<Entity.UserDetail>(jsonValue);
                    var user = _userRepository.GetByUniqueId(r => r.Guid == Guid.Parse(userDetail.Id));                    
                    
                    if (user == null)
                    {
                        return new Entity.ActionStatus() { Success = false, Message = "User does not exist in Solution" };
                    }
                    userDetail.FullName = user.FirstName.ToString() + " " + user.LastName.ToString();

                    result.Data = new Entity.LoginResponse
                    {
                        status = loginResult.data.status,
                        data = loginResult.data.data,
                        message = loginResult.message,
                        token_type = loginResult.data.token_type,
                        access_token = loginResult.data.access_token,
                        refresh_token = loginResult.data.refresh_token,
                        expires_in = loginResult.data.expires_in,
                        UserDetail = userDetail
                    };
                }
                else
                {
                    result.Message = loginResult.message;
                }

            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, $"UserManager.Login {ex.Message}");
                return new Entity.ActionStatus(false, ex.Message);
            }
            return result;
        }
        public Entity.LoginResponse RefreshToken(Entity.RefreshTokenRequest request)
        {
            try
            {
                var loginResult = _iotConnectClient.Login.RefreshToken("", new IOT.RefreshTokenModel() { RefreshToken = request.Token }).Result;
                //IOT.LoginResult loginResult = _iotConnectClient.Login.RefreshToken("", new IOT.RefreshTokenModel() { RefreshToken = request.Token }).Result;
                return new Entity.LoginResponse
                {
                    status = loginResult.data.status,
                    data = loginResult.data.data,
                    message = loginResult.message,
                    token_type = loginResult.data.token_type,
                    access_token = loginResult.data.access_token,
                    refresh_token = loginResult.data.refresh_token,
                    expires_in = loginResult.data.expires_in,
                };
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, "UserManager.ValidateUser " + ex);
                return null;
            }
        }
        public Entity.ActionStatus UpdateStatus(Guid id, bool status)
        {
            Entity.ActionStatus actionStatus = new Entity.ActionStatus(true);
            try
            {
                var dbUser = _userRepository.FindBy(x => x.Guid.Equals(id)).FirstOrDefault();
                if (dbUser == null)
                {
                    throw new NotFoundCustomException($"{CommonException.Name.NoRecordsFound} : User");
                }
                try
                {
                    var updateUserStatusResult = _iotConnectClient.User.UpdateUserStatus(dbUser.Guid.ToString(), status).Result;
                    if (updateUserStatusResult != null && updateUserStatusResult.status)
                    {
                        dbUser.IsActive = status;
                        dbUser.UpdatedDate = DateTime.Now;
                        dbUser.UpdatedBy = component.helper.SolutionConfiguration.CurrentUserId;
                        return _userRepository.Update(dbUser);
                    }
                    else
                    {
                        _logger.Error($"User status is not updated in iotconnect, Error: {updateUserStatusResult.message}");
                        actionStatus.Success = false;
                        actionStatus.Message = new UtilityHelper().IOTResultMessage(updateUserStatusResult.errorMessages);//"Something Went Wrong!";
                    }
                }
                catch (IoTConnectException ex)
                {
                    _logger.Error(Constants.ACTION_EXCEPTION, "UserManager.Delete " + ex);
                    actionStatus.Success = false;
                    actionStatus.Message = ex.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(Constants.ACTION_EXCEPTION, "UserManager.Delete " + ex);
                actionStatus.Success = false;
                actionStatus.Message = ex.Message;
            }
            return actionStatus;
        }
    }
}