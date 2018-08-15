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
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System.Collections.Generic;
    using System.Linq;

    public class AdminBl : BusinessBase<User> , IAdminBl
    {
        private IGenericRep<User> _funcionaryRepo;

        public AdminBl(IGenericRep<User> funcionaryRepo)
        {
            _funcionaryRepo = funcionaryRepo;
        }

        public Response<CreateOrUpdateFuncionaryResponse> CreateOrUpdateFuncionary(CreateOrUpdateFuncionaryRequest funcionaryReq)
        {
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0) return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);
            if (funcionaryReq.InternalMail.ToList().Contains('@')) return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);

            var funcionaryEntity = new User()
            {
                State = funcionaryReq.State,
                EmailAddress = funcionaryReq.InternalMail,
                LastName = funcionaryReq.LastName,
                Name = funcionaryReq.Name,
                Password = funcionaryReq.Password,
                Position = funcionaryReq.Position,
                Role = funcionaryReq.Role
            };
            var result = _funcionaryRepo.AddOrUpdate(funcionaryEntity).Result;

            return result ? ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>()) : 
                ResponseFail<CreateOrUpdateFuncionaryResponse>();            
        }

        public Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail)
        {
            if (string.IsNullOrEmpty(funcionaryMail)) ResponseFail(ServiceResponseCode.BadRequest);
            var result = _funcionaryRepo.GetAsync(funcionaryMail).Result;
            if (result == null || string.IsNullOrEmpty(result.EmailAddress)) ResponseFail<FuncionaryInfoResponse>();
            var funcionary = new List<FuncionaryInfoResponse>()
            {
                new FuncionaryInfoResponse()
                {
                    Position = result.Position,
                    Mail = result.EmailAddress,
                    Name = result.Name,
                    LastName = result.LastName,
                    State = result.State,
                }
            };
            return ResponseSuccess(funcionary);
        }

        public Response<FuncionaryInfoResponse> GetAllFuncionaries()
        {
            var funcionaries = _funcionaryRepo.GetAll().Result;
            if (funcionaries.Count == 0 || funcionaries == null) return ResponseFail<FuncionaryInfoResponse>();
            var funcionariesInfo = new List<FuncionaryInfoResponse>();
            funcionaries.ForEach(f => {
                funcionariesInfo.Add(new FuncionaryInfoResponse(){
                    Mail = f.EmailAddress,
                    State = f.State,
                    Name = f.Name,
                    LastName = f.LastName,
                    Position = f.Position });
            });
            return ResponseSuccess(funcionariesInfo);
        }
    }
}
