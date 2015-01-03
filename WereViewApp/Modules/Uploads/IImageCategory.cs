using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WereViewApp.Modules.Uploads {
    public interface IImageCategory {
         string CategoryName { get; set; }
         double Width { get; set; }
         double Height { get; set; }
    }
}
