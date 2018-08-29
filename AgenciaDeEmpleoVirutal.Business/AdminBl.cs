namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System.Collections.Generic;
    using System.Linq;

    public class AdminBl : BusinessBase<User> , IAdminBl
    {
        private IGenericRep<User> _usersRepo;

        public AdminBl(IGenericRep<User> usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public Response<CreateOrUpdateFuncionaryResponse> CreateFuncionary(CreateFuncionaryRequest funcionaryReq)
        {
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0) return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);

            /// funcoinaries.Any(f => f.UserName == string.Format("{0}_{1}", funcionaryReq.NoDocument,funcionaryReq.CodTypeDocument))
            var funcoinaries =_usersRepo.GetAsync(string.Format("{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.CodTypeDocument)).Result;
            if (funcoinaries != null)
                return ResponseFail<CreateOrUpdateFuncionaryResponse>(ServiceResponseCode.UserAlreadyExist);

            var funcionaryEntity = new User()
            {
                Position = funcionaryReq.Position,
                State = funcionaryReq.State ? UserStates.Enable.ToString() : UserStates.Disable.ToString(),
                NoDocument = funcionaryReq.NoDocument,
                LastName = funcionaryReq.LastName,
                Name = funcionaryReq.Name,
                Password = funcionaryReq.Password,
                Role = funcionaryReq.Role,
                DeviceId = string.Empty,
                UserName = string.Format("{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.CodTypeDocument),
                CodTypeDocument = funcionaryReq.CodTypeDocument.ToString(),
                TypeDocument = funcionaryReq.TypeDocument,
                Email = string.Format("{0}@colsubsidio.com", funcionaryReq.InternalMail),
                UserType = UsersTypes.Funcionario.ToString()
            };
            var result = _usersRepo.AddOrUpdate(funcionaryEntity).Result;

            return result ? ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>()) : 
                ResponseFail<CreateOrUpdateFuncionaryResponse>();            
        }
        
        public Response<CreateOrUpdateFuncionaryResponse> UpdateFuncionaryInfo(UpdateFuncionaryRequest funcionaryReq)
        {
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0) return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);

            var funcionary = _usersRepo.GetAsync(string.Format("{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.TypeDocument)).Result;
            if (funcionary == null) return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            var modFuncionary = funcionary;

            modFuncionary.Email = string.Format("{0}@colsubsidio.com", funcionaryReq.InternalMail);
            modFuncionary.Name = funcionaryReq.Name;
            modFuncionary.LastName = funcionaryReq.LastName;
            modFuncionary.Role = funcionaryReq.Role;
            modFuncionary.Position = funcionaryReq.Position;
            modFuncionary.State = funcionaryReq.State == true ? UserStates.Enable.ToString() : UserStates.Disable.ToString();

            var result = _usersRepo.AddOrUpdate(modFuncionary).Result;
            if (!result) ResponseFail<CreateOrUpdateFuncionaryResponse>();
            return ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>());
        }       

        public Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail)
        {
            if (string.IsNullOrEmpty(funcionaryMail)) return ResponseFail<FuncionaryInfoResponse>(ServiceResponseCode.BadRequest);
            var result = _usersRepo.GetSomeAsync("Email",string.Format("{0}@colsubsidio.com", funcionaryMail)).Result;
            if (!result.Any()) return ResponseFail<FuncionaryInfoResponse>();
            var funcionary = new List<FuncionaryInfoResponse>()
            {
                new FuncionaryInfoResponse()
                {
                    Position = result.FirstOrDefault().Position,
                    Role = result.FirstOrDefault().Role,
                    Mail = result.FirstOrDefault().Email,
                    Name = result.FirstOrDefault().Name,
                    LastName = result.FirstOrDefault().LastName,
                    State = result.FirstOrDefault().State.Equals(UserStates.Enable.ToString()) ? true : false,
                    CodTypeDocument = result.FirstOrDefault().CodTypeDocument,
                    NoDocument = result.FirstOrDefault().NoDocument,
                    TypeDocument = result.FirstOrDefault().TypeDocument
                }
            };
            return ResponseSuccess(funcionary);
        }

        public Response<FuncionaryInfoResponse> GetAllFuncionaries()
        {
            var funcionaries = _usersRepo.GetByPatitionKeyAsync(UsersTypes.Funcionario.ToString().ToLower()).Result;
            if (funcionaries.Count == 0 || funcionaries == null) return ResponseFail<FuncionaryInfoResponse>();
            var funcionariesInfo = new List<FuncionaryInfoResponse>();
            funcionaries.ForEach(f => {
                funcionariesInfo.Add(new FuncionaryInfoResponse()
                {
                    Position = f.Position,
                    Role = f.Role,                    
                    Mail = f.Email,
                    State = f.State.Equals(UserStates.Enable.ToString()) ? true : false,
                    Name = f.Name,
                    LastName = f.LastName,
                    TypeDocument = f.TypeDocument,
                    NoDocument = f.NoDocument,
                    CodTypeDocument = f.CodTypeDocument
                });
            });
            return ResponseSuccess(funcionariesInfo);
        }
    }
}
