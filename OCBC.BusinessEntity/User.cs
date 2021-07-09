using System;
using AesEncryptDecrypt;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCBC.BusinessEntity
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal BirthDate { get; set; }
        public string Sexy { get; set; }
        public string Email { get; set; }
        public decimal PhoneNumber { get; set; }
        private string password;
        public string Password
        {
            get
            {
                var t = TokenManagement.Create();
                password = t.DecryptToken(password).ToString();
                return password;
            }
            set
            {
                if (value != null)
                {
                    var t = TokenManagement.Create();
                    password = t.EncryptToken(value);
                };
            }
        }
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
