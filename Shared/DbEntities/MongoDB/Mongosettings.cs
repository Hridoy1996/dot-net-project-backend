using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DbEntities.MongoDB
{
   public class Mongosettings
    {
        public string Connection { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
