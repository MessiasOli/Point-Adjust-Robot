using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.Diagnostics;
using System.Xml.Linq;
using System;

namespace Point_Adjust_Robot.Core.Model
{
    public class Replecement
    {
        public bool removed { get; set; } //  Registro removido(sim ou não)

        public string personName { get; set; }//  Nome do colaborador que irá realizar a cobertura
        public string replacementReasonName { get; set; } //  Nome do motivo da cobertura

        public string absenteeName { get; set; }//     Nome do colaborador que irá ser coberto

        public string replacementResourceExternalId { get; set; } //         Id do recurso da cobertura
        public int replacementTypeId { get; set; }
        /*
             ID do tipo de cobertura
            Valores:
            ID = 1 - COBERTURA DE POSTO
            ID = 2 - COBERTURA DE COLABORADOR
            ID = 3 - TROCA DE HORÁRIO
            ID = 4 - INVERSÃO DE ESCALA
        */

        public string shiftExternalId { get; set; } // ID externo do horário

        public string replacementResourceName { get; set; } //  Nome do recurso da cobertura

        public string finishDateTime { get; set; } // example: ddMMyyyyHHmmss  Data fim da cobertura
        public int personId { get; set; } //  ID do colaborador que irá realizar a cobertura
        public int workplaceId { get; set; } //   ID do posto da cobertura
        public string registerDate { get; set; } // example: ddMMyyyyHHmmss  Data de registro da cobertura
        public int replacementReasonId { get; set; } //  Id do Motivo da cobertura

        public string note { get; set; } //  Anotação da cobertura

        public int absenteeId { get; set; } //  ID do colaborador que irá ser coberto

        public int id { get; set; } //  ID da cobertura

        public int replacementResourceId { get; set; } //   recurso da cobertura

        public string personExternalId { get; set; }//  ID externo do colaborador que irá realizar a cobertura

        public string replacementReasonExternalId { get; set; }// Id externo do motivo da cobertura

        public string workplaceExternalId { get; set; } // ID external do posto da cobertura
        public int shiftId { get; set; } //  ID do horário

        public string startDateTime { get; set; } // example: ddMMyyyyHHmmss  Data inicio da cobertura

        public string absenteeExternalId { get; set; } //   ID externo do colaborador que irá ser coberto

        public string lastUpdate { get; set; } // example: ddMMyyyyHHmmss  Data da ultima alteração do registro

    }
}
