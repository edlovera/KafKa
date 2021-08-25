using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZK_Kafka.DBContext.Modelos
{

    class BioData
    {
        public int BioDataID { get; set; }
        public string EmpresaNombreBD { get; set; }
        public int Pin { get; set; }
        public int No { get; set; }
        public int Index { get; set; }
        public int Valid { get; set; }
        public int Duress { get; set; }
        public int Type { get; set; }
        public string Majorver { get; set; }
        public string Minorver { get; set; }
        public string Format { get; set; }
        public string Tmp { get; set; }
        public string Equipo { get; set; }
    }
    class SPBioData
    {
        [Key]
        public bool Status { get; set; }
    }

  
}
