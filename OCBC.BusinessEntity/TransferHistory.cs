using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCBC.BusinessEntity
{
    public class TransferHistory
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string RecipientEmail { get; set; }
        public DateTime TransferDate { get; set; }
        public decimal TransferAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
