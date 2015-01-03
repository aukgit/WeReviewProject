using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereViewApp.Models.DesignPattern.Interfaces {
    interface IDevUser {
        long UserID { get; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
    }
}
