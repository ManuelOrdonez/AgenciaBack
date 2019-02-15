namespace AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest
{
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Entities;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Request;
    using AgenciaDeEmpleoVirutal.Entities.ExternalService.Response;
    using AgenciaDeEmpleoVirutal.Entities.Responses;
    using AgenciaDeEmpleoVirutal.Utils;
    using AgenciaDeEmpleoVirutal.Utils.ResponseMessages;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Request Status Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest.SubsidyBITestBase" />
    [TestClass]
    public class RequestStatusTest : SubsidyBITestBase
    {
        /// <summary>
        /// Requests the status when fields are null or empty return error.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void RequestStatus_WhenFieldsAreNullOrEmpty_ReturnError()
        {
            /// Arrange
            var request = new FosfecRequest();
            var errorsMessage = request.Validate().ToList();
            var expected = ResponseBadRequest<RequestStatusResponse>(errorsMessage);

            /// Act
            var result = subsidyBusinessLogic.RequestStatus(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        /// <summary>
        /// Requests the status when LDAP service fail return service external error.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void RequestStatus_WhenLdapServiceFail_ReturnServiceExternalError()
        {
            /// Arrange
            var request = new FosfecRequest()
            {
                CodTypeDocument = "1",
                NoDocument = "1234567"
            };
            var resultLdapService = new RequestStatusResult
            {
                Code = (int)ServiceResponseCode.ErrorRequest
            };
            var expected = ResponseFail<RequestStatusResponse>(ServiceResponseCode.ServiceExternalError);
            _LdapServices.Setup(lp => lp.RequestStatus(request)).Returns(resultLdapService);
            
            /// Act
            var result = subsidyBusinessLogic.RequestStatus(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        /// <summary>
        /// Requests the status when LDAP service response success return success.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void RequestStatus_WhenLdapServiceResponseSuccess_ReturnSuccess()
        {
            /// Arrange
            var request = new FosfecRequest()
            {
                CodTypeDocument = "1",
                NoDocument = "1234567"
            };
            Solicitud[] solicitud = new Solicitud[]
            {
                new Solicitud
                {
                    cesante = "cesante",
                    documento = new Documento{ numero = "numero", tipo = "tipo"},
                    estadoSolicitud = new Estadosolicitud
                    {
                        causal = "causal",
                        codigo = "codigo",
                        descripcion = "descripcion",
                        fecha = "fecha"
                    },
                    formulario = new Formulario { numero = "numero" },
                    nombre = new Nombre { primerApellido = "primerApellido", primero = "primero"},
                    postulacion = new Postulacion { fecha = "fecha" },
                    radicacion = new Radicacion { numero ="numero" }
                }
            };
            var resultLdapService = new RequestStatusResult
            {
                Code = (int)ServiceResponseCode.Success,
                solicitud = solicitud,
            };

            var response = new List<RequestStatusResponse>
            {
                new RequestStatusResponse
                {
                    causal = resultLdapService.solicitud.FirstOrDefault().estadoSolicitud.causal,
                    codigo = resultLdapService.solicitud.FirstOrDefault().estadoSolicitud.codigo,
                    descripcion = resultLdapService.solicitud.FirstOrDefault().estadoSolicitud.descripcion,
                    fecha = resultLdapService.solicitud.FirstOrDefault().estadoSolicitud.fecha
                }
            };

            var expected = ResponseSuccess<RequestStatusResponse>(response);
            _LdapServices.Setup(lp => lp.RequestStatus(request)).Returns(resultLdapService);

            /// Act
            var result = subsidyBusinessLogic.RequestStatus(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
        }
    }
}
