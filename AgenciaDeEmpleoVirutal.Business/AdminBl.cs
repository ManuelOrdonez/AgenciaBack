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
    using System.Globalization;
    using System.Linq;

    /// <summary>
    /// Administrator Business logic
    /// </summary>
    public class AdminBl : BusinessBase<User>, IAdminBl
    {
        /// <summary>
        /// User Repository
        /// </summary>
        private readonly IGenericRep<User> _usersRepo;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="usersRepo"></param>
        /// <param name="openTokService"></param>
        public AdminBl(IGenericRep<User> usersRepo, IOpenTokExternalService openTokService)
        {
            _usersRepo = usersRepo;
        }

        /// <summary>
        /// Method to Create Funcionary
        /// </summary>
        /// <param name="funcionary"></param>
        /// <returns></returns>
        public Response<CreateOrUpdateFuncionaryResponse> CreateFuncionary(CreateFuncionaryRequest funcionary)
        {
            if (funcionary == null)
            {
                throw new ArgumentNullException("funcionary");
            }
            string message = string.Empty;
            var errorsMesage = funcionary.Validate().ToList();
            if (errorsMesage.Count > 0)
            {
                return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);
            }
            var funcoinaries = _usersRepo.GetAsyncAll(string.Format(new CultureInfo("es-CO"), "{0}_{1}", funcionary.NoDocument, funcionary.CodTypeDocument)).Result;

            int pos = 0;
            if (!ValRegistriesUser(funcoinaries, out pos))
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>(ServiceResponseCode.UserAlreadyExist);
            }
            if (pos == 0)
            {
                /// Existe un usuario tipo persona que se debe desabilitar para continuar con el proceso de creación 
                /// del usuario
                funcoinaries[0].State = UserStates.Disable.ToString();
                if (!_usersRepo.AddOrUpdate(funcoinaries[0]).Result)
                {
                    return ResponseFail<CreateOrUpdateFuncionaryResponse>();
                }
                message = "Usuario creado exitosamente. El usuario que tenia registrado como Persona queda inactivo.";
            }
            else
            {
                message = "Usuario creado exitosamente.";
            }

            var funcionaryEntity = new User
            {
                Position = funcionary.Position,
                State = funcionary.State ? UserStates.Enable.ToString() : UserStates.Disable.ToString(),
                NoDocument = funcionary.NoDocument,
                LastName = UString.UppercaseWords(funcionary.LastName),
                Name = UString.UppercaseWords(funcionary.Name),
                Password = funcionary.Password,
                Role = funcionary.Role,
                DeviceId = string.Empty,
                UserName = string.Format(new CultureInfo("es-CO"), "{0}_{1}", funcionary.NoDocument, funcionary.CodTypeDocument),
                CodTypeDocument = funcionary.CodTypeDocument.ToString(new CultureInfo("es-CO")),
                TypeDocument = funcionary.TypeDocument,
                Email = string.Format(new CultureInfo("es-CO"), "{0}@colsubsidio.com", funcionary.InternalMail),
                UserType = UsersTypes.Funcionario.ToString(),
                CountCallAttended = 0,
                Available = false,
                RegisterDate = DateTimeOffset.UtcNow.AddHours(-5)                
            };
            var result = _usersRepo.AddOrUpdate(funcionaryEntity).Result;

            if (!result)
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            }
            return ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse> { new CreateOrUpdateFuncionaryResponse { Message = message } });
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

        /// <summary>
        /// Method to Get User Funcionary
        /// </summary>
        /// <param name="lUser"></param>
        /// <param name="funtionary"></param>
        /// <param name="people"></param>
        private void GetUserFuncionary(List<User> lUser, out User funtionary, out User people)
        {
            funtionary = null;
            people = null;
            if (!lUser.Any())
            {
                return;
            }
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
                        throw new InvalidOperationException("Unexpected value UserType = " + item.UserType);
                }
            }
        }

        /// <summary>
        /// Method to Update Funcionary Info
        /// </summary>
        /// <param name="funcionaryReq"></param>
        /// <returns></returns>
        public Response<CreateOrUpdateFuncionaryResponse> UpdateFuncionaryInfo(UpdateFuncionaryRequest funcionaryReq)
        {
            if (funcionaryReq == null)
            {
                throw new ArgumentNullException("funcionaryReq");
            }
            var errorsMesage = funcionaryReq.Validate().ToList();
            if (errorsMesage.Count > 0)
            {
                return ResponseBadRequest<CreateOrUpdateFuncionaryResponse>(errorsMesage);
            }

            List<User> funcionaries = _usersRepo.GetAsyncAll(string.Format(new CultureInfo("es-CO"), "{0}_{1}", funcionaryReq.NoDocument, funcionaryReq.TypeDocument)).Result;
            User funcionary = null;
            User people = null;
            GetUserFuncionary(funcionaries, out funcionary, out people);
            if (funcionary == null)
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            }

            funcionary.Email = string.Format(new CultureInfo("es-CO"), "{0}@colsubsidio.com", funcionaryReq.InternalMail);
            funcionary.Name = UString.UppercaseWords(funcionaryReq.Name);
            funcionary.LastName = UString.UppercaseWords(funcionaryReq.LastName);
            funcionary.Role = funcionaryReq.Role;
            funcionary.Position = funcionaryReq.Position;
            funcionary.State = funcionaryReq.State ? UserStates.Enable.ToString() : UserStates.Disable.ToString();

            var result = _usersRepo.AddOrUpdate(funcionary).Result;
            if (!result)
            {
                return ResponseFail<CreateOrUpdateFuncionaryResponse>();
            }

            if (people != null)
            {
                people.State = funcionaryReq.State ? UserStates.Disable.ToString() : UserStates.Enable.ToString();
                result = _usersRepo.AddOrUpdate(people).Result;
                if (!result)
                {
                    return ResponseFail<CreateOrUpdateFuncionaryResponse>();
                }
            }
            return ResponseSuccess(new List<CreateOrUpdateFuncionaryResponse>());
        }

        /// <summary>
        /// Method to Get Funcionary Info
        /// </summary>
        /// <param name="funcionaryMail"></param>
        /// <returns></returns>
        public Response<FuncionaryInfoResponse> GetFuncionaryInfo(string funcionaryMail)
        {
            if (string.IsNullOrEmpty(funcionaryMail))
            {
                return ResponseFail<FuncionaryInfoResponse>(ServiceResponseCode.BadRequest);
            }
            var result = _usersRepo.GetSomeAsync("Email", string.Format(new CultureInfo("es-CO"), "{0}@colsubsidio.com", funcionaryMail)).Result;
            if (!result.Any())
            {
                return ResponseFail<FuncionaryInfoResponse>();
            }
            var funcionary = new List<FuncionaryInfoResponse>
            {
                new FuncionaryInfoResponse
                {
                    Position = result.FirstOrDefault().Position,
                    Role = result.FirstOrDefault().Role,
                    Mail = result.FirstOrDefault().Email,
                    Name = result.FirstOrDefault().Name,
                    LastName = result.FirstOrDefault().LastName,
                    State = result.FirstOrDefault().State.Equals(UserStates.Enable.ToString()),
                    CodTypeDocument = result.FirstOrDefault().CodTypeDocument,
                    NoDocument = result.FirstOrDefault().NoDocument,
                    TypeDocument = result.FirstOrDefault().TypeDocument
                }
            };
            return ResponseSuccess(funcionary);
        }

        /// <summary>
        /// Method to Get All Funcionaries
        /// </summary>
        /// <returns></returns>
        public Response<FuncionaryInfoResponse> GetAllFuncionaries()
        {
            var funcionaries = _usersRepo.GetByPatitionKeyAsync(UsersTypes.Funcionario.ToString().ToLower(new CultureInfo("es-CO"))).Result;
            if (funcionaries.Count == 0 || funcionaries is null)
            {
                return ResponseFail<FuncionaryInfoResponse>();
            }
            var funcionariesInfo = new List<FuncionaryInfoResponse>();
            funcionaries.ForEach(f =>
            {
                funcionariesInfo.Add(new FuncionaryInfoResponse
                {
                    Position = f.Position,
                    Role = f.Role,
                    Mail = f.Email,
                    State = f.State.Equals(UserStates.Enable.ToString()),
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
