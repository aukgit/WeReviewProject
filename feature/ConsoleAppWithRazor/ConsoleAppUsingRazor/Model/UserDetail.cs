using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateEmailUsingRazor.Model
{

    public class UserDetail
    {

        public UserDetail()
        {
            PurchasedItems = new List<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public List<string> PurchasedItems { get; set; }

    }

}
