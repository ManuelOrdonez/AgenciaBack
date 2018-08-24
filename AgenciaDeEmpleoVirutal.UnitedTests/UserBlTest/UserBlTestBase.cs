using AgenciaDeEmpleoVirutal.Business;
using AgenciaDeEmpleoVirutal.Business.Referentials;
using AgenciaDeEmpleoVirutal.Contracts.ExternalServices;
using AgenciaDeEmpleoVirutal.Contracts.Referentials;
using AgenciaDeEmpleoVirutal.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AgenciaDeEmpleoVirutal.UnitedTests.UserBlTest
{
    public class UserBlTestBase : BusinessBase<User>
    {
        protected Mock<IGenericRep<User>> UserRepMoq;

        protected Mock<ILdapServices> LdapServices;

        protected UserBl UserBusiness;

        public UserBlTestBase()
        {
            UserRepMoq = new Mock<IGenericRep<User>>();
            LdapServices = new Mock<ILdapServices>();
            UserBusiness = new UserBl(UserRepMoq.Object, LdapServices.Object);
        }
    }
}
