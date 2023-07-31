import * as cheetahGrid from "cheetah-grid";
import Absence from "src/model/Absence";

var situationOptions = [
  { value: null, caption: "Vazio" },
  { value: "ABONADO", caption: "- ABONADO" },
  { value: "ABONADO CLIENTE", caption: "- ABONADO CLIENTE" },
  { value: "ABONO GREVE", caption: "- ABONO GREVE" },
  { value: "Afastamento temporario por doenca", caption: "- Afastamento temporario por doenca" },
  { value: "Afastamento temporario por motivo de licenca maternidade pago pelo inss", caption: "- Afastamento temporario por motivo de licenca maternidade pago pelo inss" },
  { value: "Ausência Avisada", caption: "- Ausência Avisada" },
  { value: "AVISO PRÉVIO", caption: "- AVISO PRÉVIO" },
  { value: "CASAMENTO                ", caption: "- CASAMENTO                " },
  { value: "CURSO", caption: "- CURSO" },
  { value: "DECLARACAO DE COMP.(HORAS)", caption: "- DECLARACAO DE COMP.(HORAS)" },
  { value: "DECLARACAO DE COMPARECIME", caption: "- DECLARACAO DE COMPARECIME" },
  { value: "DESCONTAR", caption: "- DESCONTAR" },
  { value: "DIA/ HORAS COMPENSADAS", caption: "- DIA/ HORAS COMPENSADAS" },
  { value: "DOACAO DE SANGUE         ", caption: "- DOACAO DE SANGUE         " },
  { value: "DOMINGO DO MÊS", caption: "- DOMINGO DO MÊS" },
  { value: "FALECIMENTO              ", caption: "- FALECIMENTO              " },
  { value: "FALTA", caption: "- FALTA" },
  { value: "FALTA (HORAS)", caption: "- FALTA (HORAS)" },
  { value: "FALTA COM COBERTURA", caption: "- FALTA COM COBERTURA" },
  { value: "FERIADO                  ", caption: "- FERIADO                  " },
  { value: "FÉRIAS", caption: "- FÉRIAS" },
  { value: "FOLGA", caption: "- FOLGA" },
  { value: "HORA EXTRA AUTORIZADA    ", caption: "- HORA EXTRA AUTORIZADA    " },
  { value: "IMPLANTAÇÃO POSTO", caption: "- IMPLANTAÇÃO POSTO" },
  { value: "JUSTICA DIVERSAS         ", caption: "- JUSTICA DIVERSAS         " },
  { value: "JUSTICA ELEITORAL        ", caption: "- JUSTICA ELEITORAL        " },
  { value: "LIBERADO CLIENTE", caption: "- LIBERADO CLIENTE" },
  { value: "LICENCA PATERNIDADE      ", caption: "- LICENCA PATERNIDADE      " },
  { value: "PEDIDO DEMISSÃO", caption: "- PEDIDO DEMISSÃO" },
  { value: "PROCESSO DE CALCULO RESCISÓRIO", caption: "- PROCESSO DE CALCULO RESCISÓRIO" },
  { value: "REDUÇÃO 7 DIAS APT", caption: "- REDUÇÃO 7 DIAS APT" },
  { value: "SAÍDA ANTECIPADA CLIENTE", caption: "- SAÍDA ANTECIPADA CLIENTE" },
  { value: "SERVIÇO EXTERNO", caption: "- SERVIÇO EXTERNO" },
  { value: "SERVICO MILITAR          ", caption: "- SERVICO MILITAR          " },
  { value: "SUSPENSAO                ", caption: "- SUSPENSAO                " },
  { value: "TREINAMENTO", caption: "- TREINAMENTO" }
];

var wantToAssociateOptions = [
  { value: null, caption: "Vazio" },
  { value: "nao", caption: "Sim" },
  { value: "sim", caption: "Não" }
];

var inputEditor = new cheetahGrid.columns.action.InlineInputEditor();
var situationsInputOptions = new cheetahGrid.columns.action.InlineMenuEditor({ options: situationOptions, });
var assoiationInputOptions = new cheetahGrid.columns.action.InlineMenuEditor({ options: wantToAssociateOptions, });

const GridAbsence = {
  eventTypes: { ...cheetahGrid.ListGrid.EVENT_TYPE },
  defaultRowHeight: 25,
  headerRowHeight: 23,
  columns: [
    {
      field: "index",
      caption: "#",
      minWidth: 30,
      width: "3.6%",
      action: inputEditor
    },
    {
      field: "matriculation",
      caption: "Matrícula",
      width: "9.62%",
      minWidth: "80",
      action: inputEditor
    },
    {
      field: "situation",
      caption: "Situação da Ausência",
      width: "19.85%",
      minWidth: 165,
      action: situationsInputOptions
    },
    {
      field: "startDate",
      caption: "Em",
      width: "10.8%",
      minWidth: "90",
      action: inputEditor
    },
    {
      field: "endDate",
      caption: "Até",
      width: "10.8%",
      minWidth: "90",
      action: inputEditor
    },
    {
      field: "wantToAssociate",
      caption: "Deseja Associar?",
      width: "16.23%",
      minWidth: 135,
      action: assoiationInputOptions
    },
    {
      field: "entry",
      caption: "Entrada",
      width: "7.8%",
      minWidth: 65,
      action: inputEditor
    },
    {
      field: "departure",
      caption: "Saida",
      width: "7.8%",
      minWidth: 65,
      action: inputEditor
    },
    {
      field: "note",
      width: "95",
      caption: "Observação",
      width: "11.4%",
      minWidth: 95,
      action: inputEditor
    },
  ],
  rows: [
    new Absence(
      {
        "index": "1",
        "matriculation": "X000001",
        "situation": "FALTA",
        "startDate": "22/12/2022",
        "endDate": "22/12/2022",
        "wantToAssociate": "nao",
        "entry": "",
        "departure": "",
        "note": "teste"
      }
    ),
    new Absence(
      {
        "index": "2",
        "matriculation": "X000001",
        "situation": "FALTA",
        "startDate": "21/12/2022",
        "endDate": "21/12/2022",
        "wantToAssociate": "sim",
        "entry": "08:00",
        "departure": "18:00",
        "note": "teste"
      }
    )
  ]
};

for (let i = 1; i < 1000; i++)
  GridAbsence.rows.push({
    index: "",
    matriculation: "",
    situation: "",
    startDate: "",
    endDate: "",
    wantToAssociate: "",
    note: "",
  });

export default GridAbsence;
