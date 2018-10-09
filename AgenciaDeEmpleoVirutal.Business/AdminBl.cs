namespace AgenciaDeEmpleoVirutal.Business
{
    using AgenciaDeEmpleoVirutal.Business.Referentials;
    using AgenciaDeEmpleoVirutal.Contracts.Business;
    using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
    using AgenciaDeEmpleoVirutal.Contracts.Referentials;
    using AgenciaDeEmpleoVirutal.Entities;
    using AgenciaDeEmpleoVirutal.Entities.Referentials;
    using AgenciaDeEmpleoVirutal.Entities.Requests;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.Enum;
    using AgenciaDeEmpleoVirutal.Utils.Helpers;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AdminBl : BusinessBase<User>, IAdminBl , IDisposable
    {
        private IGenericRep<User> _usersRepo;

        private IOpenTokExternalService _openTokExternalService;

        private Crypto _crypto;

        public AdminBl(IGenericRep<User> usersRepo, IOpenTokExternalService openTokService)
        {
            _usersRepo = usersRepo;
            _crypto = new Crypto();
            _openTokExternalService = openTokService;
        }


        public Response<CreateOrUpdateFuncionaryResponse> CreateFuncionary(CreateFuncionaryRequest funcionaryReq)
        {
            string message = string.Empty;
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0)
            {
                return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);
            }
            var funcoinaries = _usersRepo.GetAsyncAll(string.Format("{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.CodTypeDocument)).Result;

            int pos = 0;
            /// Valida cuando existe mas de un registro y 
            if (!ValRegistriesUser(funcoinaries, out pos))
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>(ServiceResponseCode.UserAlreadyExist);
            }
            if (pos == 0)
            {
                /// Existe un usuario tipo persona que se debe desabilitar para continuar con el proceso de creación 
                /// del usuario
                funcoinaries[0].State = UserStates.Disable.ToString();
                var resultp = _usersRepo.AddOrUpdate(funcoinaries[0]).Result;
                message = "Usuario creado exitosamente. El usuario que tenia registrado como Persona queda inactivo.";
            }
            else
            {
                message = "Usuario creado exitosamente.";
            }

            var funcionaryEntity = new User()
            {
                Position = funcionaryReq.Position,
                State = funcionaryReq.State ? UserStates.Enable.ToString() : UserStates.Disable.ToString(),
                NoDocument = funcionaryReq.NoDocument,
                LastName = Utils.Helpers.UString.UppercaseWords(funcionaryReq.LastName),
                Name = Utils.Helpers.UString.UppercaseWords(funcionaryReq.Name),
                Password = funcionaryReq.Password,
                Role = funcionaryReq.Role,
                DeviceId = string.Empty,
                UserName = string.Format("{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.CodTypeDocument),
                CodTypeDocument = funcionaryReq.CodTypeDocument.ToString(),
                TypeDocument = funcionaryReq.TypeDocument,
                Email = string.Format("{0}@colsubsidio.com", funcionaryReq.InternalMail),
                UserType = UsersTypes.Funcionario.ToString(),
                OpenTokSessionId = _openTokExternalService.CreateSession(),
                CountCallAttended = 0,
                Available = false
            };
            var result = _usersRepo.AddOrUpdate(funcionaryEntity).Result;

            if (!result)
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            }
            return ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>() { new CreateOrUpdateFuncionaryResponse() { Message = message } });

        }
        /// <summary>
        /// Función que determina si el usuario existen es persona para que permita crear el usuario como funcionario
        /// </summary>
        /// <param name="lUser">Lista de usuarios registrados</param>
        /// <param name="position">posición que se encuentra el registro de persona</param>
        /// <returns></returns>
        private bool ValRegistriesUser(List<User> lUser, out int position)
        {
            bool eRta = true;
            position = -1;
            if (lUser.Count > 0)
            {
                eRta = false;
                if (lUser[0].UserType == "cesante")
                {
                    position = 0;
                    eRta = true;
                }
            }

            return eRta;
        }
        private void GetUserFuncionary(List<User> lUser, out User funtionary, out User people)
        {
            funtionary = null;
            people = null;
            foreach (var item in lUser)
            {
                switch (item.UserType)
                {
                    case "cesante":
                        people = item;
                        break;
                    case "empresa":
                        break;
                    case "funcionario":
                        funtionary = item;
                        break;
                    default:
                        break;
                }
            }

        }
        public Response<CreateOrUpdateFuncionaryResponse> UpdateFuncionaryInfo(UpdateFuncionaryRequest funcionaryReq)
        {
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0)
            {
                return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);
            }

            //var funcionary = _usersRepo.GetAsync(string.Format("{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.TypeDocument)).Result;
            List<User> funcionaries = _usersRepo.GetAsyncAll(string.Format("{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.TypeDocument)).Result;
            User funcionary = null;
            User people = null;
            GetUserFuncionary(funcionaries, out funcionary, out people);
            if (funcionary == null)
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            }

            funcionary.Email = string.Format("{0}@colsubsidio.com", funcionaryReq.InternalMail);
            funcionary.Name = Utils.Helpers.UString.UppercaseWords(funcionaryReq.Name);
            funcionary.LastName = Utils.Helpers.UString.UppercaseWords(funcionaryReq.LastName);
            funcionary.Role = funcionaryReq.Role;
            funcionary.Position = funcionaryReq.Position;
            funcionary.State = funcionaryReq.State == true ? UserStates.Enable.ToString() : UserStates.Disable.ToString();

            var result = _usersRepo.AddOrUpdate(funcionary).Result;
            if (!result)
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            }

            if (people != null)
            {
                people.State = funcionaryReq.State == true ? UserStates.Disable.ToString() : UserStates.Enable.ToString();
                result = _usersRepo.AddOrUpdate(people).Result;
                if (!result)
                {
                    return ResponseFail<CreateOrUpdateFuncionaryResponse>();
                }
            }
            return ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>());
        }

        public Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail)
        {
            if (string.IsNullOrEmpty(funcionaryMail))
            {
                return ResponseFail<FuncionaryInfoResponse>(ServiceResponseCode.BadRequest);
            }
            var result = _usersRepo.GetSomeAsync("Email", string.Format("{0}@colsubsidio.com", funcionaryMail)).Result;
            if (!result.Any())
            {
                return ResponseFail<FuncionaryInfoResponse>();
            }
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
            if (funcionaries.Count == 0 || funcionaries is null)
            {
                return ResponseFail<FuncionaryInfoResponse>();
            }
            var funcionariesInfo = new List<FuncionaryInfoResponse>();
            funcionaries.ForEach(f =>
            {
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
        public void Dispose()
        {
            if (this._crypto != null)
            {
                this._crypto.Dispose(); 
            }
        }
    }
}
