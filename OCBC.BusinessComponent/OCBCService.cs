using OCBC.BusinessEntity;
using OCBC.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OCBC.BusinessService
{
    public class OCBCService
    {
        OCBCDataAccess ocbcDataAccess = new OCBCDataAccess();
        public string SaveRegisterDetails(User user)
        {
            return ocbcDataAccess.SaveRegisterDetails(user);
        }

        public string AddTransactionInfo(TransferHistory transferHistory)
        {
            return ocbcDataAccess.AddTransactionInfo(transferHistory);
        }

        public User ValidateUser(LoginViewModel model)
        {
            return ocbcDataAccess.ValidateUser(model);
        }

        public string DepositMoney(string userId, decimal amount)
        {
            return ocbcDataAccess.DepositMoney(userId, amount);
        }
    }
}
