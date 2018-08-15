using AgenciaDeEmpleoVirutal.Business;
using AgenciaDeEmpleoVirutal.Business.Referentials;
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

        protected UserBl UserBusiness;

        public UserBlTestBase()
        {
            UserRepMoq = new Mock<IGenericRep<User>>();
            UserBusiness = new UserBl(UserRepMoq.Object);
        }
    }
}
