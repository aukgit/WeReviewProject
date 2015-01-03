using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereViewApp.Models.EntityModel.Derivables {
 
    interface ISavableApp {
         string IdeaBy { get; set; }
         string Developers { get; set; }
         string Publishers { get; set; }

         string Tags { get; set; }
    }
}
