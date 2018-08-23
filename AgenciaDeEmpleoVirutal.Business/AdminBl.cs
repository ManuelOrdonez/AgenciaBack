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
            var funcoinaries =_usersRepo.GetAll().Result;
            if (funcoinaries.Any(f => f.UserName == string.Format("{0}@colsubsidio.com", funcionaryReq.InternalMail)))
                return ResponseFail<CreateOrUpdateFuncionaryResponse>(ServiceResponseCode.UserAlreadyExist);
            var funcionaryEntity = new User()
            {
                Position = funcionaryReq.Position,
                State = funcionaryReq.State ? UserStates.Enable.ToString() : UserStates.Disable.ToString(),
                NoDocument = funcoinaries == null ? "FUNC-01" : string.Format("FUNC-0{0}", funcoinaries.ToList().Count + 1),
                LastName = funcionaryReq.LastName,
                Name = funcionaryReq.Name,
                Password = funcionaryReq.Password,
                Role = funcionaryReq.Role,
                DeviceId = string.Empty,
                UserName = string.Format("{0}@colsubsidio.com",funcionaryReq.InternalMail),
                TypeDocument = ""                
            };
            var result = _usersRepo.AddOrUpdate(funcionaryEntity).Result;

            return result ? ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>()) : 
                ResponseFail<CreateOrUpdateFuncionaryResponse>();            
        }
        
        public Response<CreateOrUpdateFuncionaryResponse> UpdateFuncionaryInfo(UpdateFuncionaryRequest funcionaryReq)
        {
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0) return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);

            var funcionary = _usersRepo.GetAsync(string.Format("{0}@colsubsidio.com", funcionaryReq.InternalMail)).Result;
            if (funcionary == null) return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            var modFuncionary = funcionary;

            modFuncionary.Name = funcionaryReq.Name;
            modFuncionary.LastName = funcionaryReq.LastName;
            modFuncionary.Role = funcionaryReq.Role;
            modFuncionary.State = funcionaryReq.State == true ? UserStates.Enable.ToString() : UserStates.Disable.ToString();

            var result = _usersRepo.AddOrUpdate(modFuncionary).Result;
            if (!result) ResponseFail<CreateOrUpdateFuncionaryResponse>();
            return ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>());
        }       

        public Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail)
        {
            if (string.IsNullOrEmpty(funcionaryMail)) return ResponseFail<FuncionaryInfoResponse>(ServiceResponseCode.BadRequest);
            var result = _usersRepo.GetAsync(string.Format("{0}@colsubsidio.com", funcionaryMail)).Result;
            if (result == null) return ResponseFail<FuncionaryInfoResponse>();
            var funcionary = new List<FuncionaryInfoResponse>()
            {
                new FuncionaryInfoResponse()
                {
                    Position = result.Position,
                    Role = result.Role,
                    Mail = result.NoDocument,
                    Name = result.Name,
                    LastName = result.LastName,
                    State = result.State.Equals(UserStates.Enable.ToString()) ? true : false,
                }
            };
            return ResponseSuccess(funcionary);
        }

        public Response<FuncionaryInfoResponse> GetAllFuncionaries()
        {
            var funcionaries = _usersRepo.GetSomeAsync("TypeDocument", string.Empty).Result;
            if (funcionaries.Count == 0 || funcionaries == null) return ResponseFail<FuncionaryInfoResponse>();
            var funcionariesInfo = new List<FuncionaryInfoResponse>();
            funcionaries.ForEach(f => {
                funcionariesInfo.Add(new FuncionaryInfoResponse()
                {
                    Position = f.Position,
                    Role = f.Role,
                    Mail = f.NoDocument,
                    State = f.State.Equals(UserStates.Enable.ToString()) ? true : false,
                    Name = f.Name,
                    LastName = f.LastName,
                });
            });
            return ResponseSuccess(funcionariesInfo);
        }
    }
}
