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
    using Microsoft.WindowsAzure.Storage.Table;
    using System.Collections.Generic;
    using System.Linq;

    public class AdminBl : BusinessBase<User> , IAdminBl
    {
        private IGenericRep<User> _usersRepo;

        public AdminBl(IGenericRep<User> usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public Response<CreateOrUpdateFuncionaryResponse> CreateOrUpdateFuncionary(CreateOrUpdateFuncionaryRequest funcionaryReq)
        {
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0) return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);           
            var funcionaryEntity = new User()
            {
                Position = funcionaryReq.Position,
                State = funcionaryReq.State ? UserStates.Enable.ToString() : UserStates.Disable.ToString(),
                EmailAddress = string.Format("{0}@colsubsidio.com",funcionaryReq.InternalMail),
                LastName = funcionaryReq.LastName,
                Name = funcionaryReq.Name,
                Password = funcionaryReq.Password,
                Role = funcionaryReq.Role,
                Authenticated = true
            };
            var result = _usersRepo.AddOrUpdate(funcionaryEntity).Result;

            return result ? ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>()) : 
                ResponseFail<CreateOrUpdateFuncionaryResponse>();            
        }

        public Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail)
        {
            if (string.IsNullOrEmpty(funcionaryMail)) return ResponseFail<FuncionaryInfoResponse>(ServiceResponseCode.BadRequest);
            var result = _usersRepo.GetAsync(string.Format("{0}@colsubsidio.com", funcionaryMail)).Result;
            if (result == null || string.IsNullOrEmpty(result.EmailAddress)) return ResponseFail<FuncionaryInfoResponse>();
            var funcionary = new List<FuncionaryInfoResponse>()
            {
                new FuncionaryInfoResponse()
                {
                    Position = result.Position,
                    Role = result.Role,
                    Mail = result.EmailAddress,
                    Name = result.Name,
                    LastName = result.LastName,
                    State = result.State.Equals(UserStates.Enable.ToString()) ? true : false,
                }
            };
            return ResponseSuccess(funcionary);
        }

        public Response<FuncionaryInfoResponse> GetAllFuncionaries()
        {
            var condition = new List<ConditionParameter>
            {
                new ConditionParameter
                {
                    ColumnName = "PartitionKey",
                    Condition = QueryComparisons.Equal,
                    Value = UsersRole.Auxiliar.ToString(),
                },
                new ConditionParameter
                {
                    ColumnName = "PartitionKey",
                    Condition = QueryComparisons.Equal,
                    Value = UsersRole.Orientador.ToString(),
                },
                new ConditionParameter
                {
                    ColumnName = "PartitionKey",
                    Condition = QueryComparisons.Equal,
                    Value = UsersRole.Supervisor.ToString(),
                },
                new ConditionParameter
                {
                    ColumnName = "PartitionKey",
                    Condition = QueryComparisons.Equal,
                    Value = UsersRole.Administrador.ToString(),
                },
            };
            var funcionaries = _usersRepo.GetSomeAsync(condition).Result;
            if (funcionaries.Count == 0 || funcionaries == null) return ResponseFail<FuncionaryInfoResponse>();
            var funcionariesInfo = new List<FuncionaryInfoResponse>();
            funcionaries.ForEach(f => {
                funcionariesInfo.Add(new FuncionaryInfoResponse()
                {
                    Position = f.Position,
                    Role = f.Role,
                    Mail = f.EmailAddress,
                    State = f.State.Equals(UserStates.Enable.ToString()) ? true : false,
                    Name = f.Name,
                    LastName = f.LastName,
                });
            });
            return ResponseSuccess(funcionariesInfo);
        }
    }
}
