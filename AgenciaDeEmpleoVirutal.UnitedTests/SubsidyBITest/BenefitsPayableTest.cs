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
    /// Benefits Payable Test
    /// </summary>
    /// <seealso cref="AgenciaDeEmpleoVirutal.UnitedTests.SubsidyBITest.SubsidyBITestBase" />
    [TestClass]
    public class BenefitsPayableTest : SubsidyBITestBase
    {
        /// <summary>
        /// Benefitses the payable when fields are null or empty return error.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void BenefitsPayable_WhenFieldsAreNullOrEmpty_ReturnError()
        {
            /// Arrange
            var request = new FosfecRequest();
            var errorsMessage = request.Validate().ToList();
            var expected = ResponseBadRequest<RequestStatusResponse>(errorsMessage);

            /// Act
            var result = subsidyBusinessLogic.BenefitsPayable(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        /// <summary>
        /// Benefitses the payable when LDAP service fail return error.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void BenefitsPayable_WhenLdapServiceFail_ReturnError()
        {
            /// Arrange
            var request = new FosfecRequest()
            {
                CodTypeDocument = "1",
                NoDocument = "1234567"
            };
            var resultLdapService = new BenefitsPayableResult
            {
                Code = (int)ServiceResponseCode.ErrorRequest
            };
            var expected = ResponseFail<RequestStatusResponse>(ServiceResponseCode.ServiceExternalError);
            _LdapServices.Setup(lp => lp.BenefitsPayable(request)).Returns(resultLdapService);

            /// Act
            var result = subsidyBusinessLogic.BenefitsPayable(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsFalse(result.TransactionMade);
            Assert.IsNull(result.Data);
        }

        /// <summary>
        /// Benefitses the payable when LDAP service response success return success.
        /// </summary>
        [TestMethod, TestCategory("SubsidyBI")]
        public void BenefitsPayable_WhenLdapServiceResponseSuccess_ReturnSuccess()
        {
            /// Arrange
            var request = new FosfecRequest()
            {
                CodTypeDocument = "1",
                NoDocument = "1234567"
            };
            var resultLdapService = new BenefitsPayableResult
            {
                Code = (int)ServiceResponseCode.Success,
                Beneficio = new Beneficio[]
                {
                    new Beneficio
                    {
                        BeneficioPorPagar = new Beneficioporpagar[]
                        {
                            new Beneficioporpagar
                            {
                                NumeroCuenta = new Cuenta { Numero = "numero" },
                                FechaVencimiento = "fechaVencimiento",
                                Sucursal = "sucursal",
                                ValorAlimentacion = "valorAlimentacion",
                                ValorCuotaModeradora = "valorCuotaModeradora"
                            }
                        }
                    }
                }
            };

            var response = new List<BenefitsPayableResponse>();

            foreach (var item in resultLdapService.Beneficio.FirstOrDefault().BeneficioPorPagar)
            {
                response.Add(new BenefitsPayableResponse
                {
                    cuenta = item.NumeroCuenta,
                    fechaVencimiento = item.FechaVencimiento,
                    sucursal = item.Sucursal,
                    valorAlimentacion = item.ValorAlimentacion,
                    valorCuotaModeradora = item.ValorCuotaModeradora
                });
            }

            var expected = ResponseSuccess<BenefitsPayableResponse>(response);
            _LdapServices.Setup(lp => lp.BenefitsPayable(request)).Returns(resultLdapService);

            /// Act
            var result = subsidyBusinessLogic.BenefitsPayable(request);

            /// Assert
            Assert.AreEqual(expected.Message.ToString(), result.Message.ToString());
            Assert.AreEqual(expected.CodeResponse, result.CodeResponse);
            Assert.IsTrue(result.TransactionMade);
            Assert.IsNotNull(result.Data);
        }
    }
}
