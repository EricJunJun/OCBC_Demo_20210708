using Microsoft.VisualStudio.TestTools.UnitTesting;
using OCBC.BusinessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using WebAPI;

namespace WebAPI.Tests.Controllers
{
    [TestClass]
    public class OCBCServiceTest
    {
        [TestMethod]
        public void TestSaveRegisterDetails()
        {
            // Arrange
            OCBCService ocbcSvc = new OCBCService();
            // Act
            string result = ocbcSvc.SaveRegisterDetails(new OCBC.BusinessEntity.User()
            {
                Id = Guid.NewGuid(),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "Test002Email@gmail.com",
                PhoneNumber = 81111818,
                Password = "12345678",
                Balance = 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                IsDeleted = false
            });

            // Assert
           Assert.AreEqual("Registered successfully.",result);
        }

        [TestMethod]
        public void TestTransferMoney()
        {
            // Arrange
            OCBCService ocbcSvc = new OCBCService();

            // Act
            string result = ocbcSvc.AddTransactionInfo(new OCBC.BusinessEntity.TransferHistory()
            {

                Id = Guid.NewGuid(),
                SenderId = new Guid("A7520073-282A-4960-A0A9-D2DA2DA7D05A"),
                RecipientEmail = "Bob@test.com",
                TransferAmount = 100
            });

            // Assert
            Assert.IsTrue(result.Split('|').Length > 0);
        }

        [TestMethod]
        public void TestDepositMoney()
        {
            // Arrange
            OCBCService ocbcSvc = new OCBCService();

            // Act
            string result = ocbcSvc.DepositMoney(userId: "8532570B-83C7-4EC0-B2D2-7E773F99205C", amount: 100);
            
            // Assert
            Assert.IsTrue(result.Split('|').Length > 0);
        }
    }
}
