using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereViewApp.Models.DesignPattern.Interfaces {
    interface IDevUserRole {
        long Id { get; set; }

        string Name { get; set; }
        byte PriorityLevel { get; set; }
    }
}
