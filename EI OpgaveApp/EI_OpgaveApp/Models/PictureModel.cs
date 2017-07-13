using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EI_OpgaveApp.Models
{
    public class PictureModel
    {
        [PrimaryKey]
        public string id { get; set; }
        public string Picture { get; set; }
    }
}
