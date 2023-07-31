import * as cheetahGrid from "cheetah-grid";

var reasonOptions = [
  { value: null, caption: "Vazio" },
  { value: "Aguardo de Processamento RH definitivo", caption: "Aguardo de Processamento RH definitivo" },
  { value: "Ausência de colaborador", caption: "Ausência de colaborador" },
  { value: "Cobertura de férias", caption: "Cobertura de férias" },
  { value: "Cobertura de posto", caption: "Cobertura de posto" },
  { value: "Hora-Extra - Não Programada Faturada", caption: "Hora-Extra - Não Programada Faturada" },
  { value: "Hora-Extra - Não Programada Não Faturada", caption: "Hora-Extra - Não Programada Não Faturada" },
  { value: "Hora-Extra - Programada Faturada", caption: "Hora-Extra - Programada Faturada" },
  { value: "Hora-Extra - Programada Não Faturada", caption: "Hora-Extra - Programada Não Faturada" },
  { value: "Implantação", caption: "Implantação" },
];

var featureOptions = [
  { value: null, caption: "Vazio" },
  { value: "FT", caption: "FT" },
  { value: "HORA EXTRA", caption: "HORA EXTRA" },
  { value: "HORA REGULAR", caption: "HORA REGULAR" },
];

var selectorReason = new cheetahGrid.columns.action.InlineMenuEditor({ options: reasonOptions, });
var featureReason = new cheetahGrid.columns.action.InlineMenuEditor({ options: featureOptions, });
var inputEditor = new cheetahGrid.columns.action.InlineInputEditor();

var styleHeader = { textAlign: "justify", font: "15px sans-serif" };
var styleCell = { textOverflow: "elipse" };

const GridAdjust = {
  eventTypes: { ...cheetahGrid.ListGrid.EVENT_TYPE },
  defaultRowHeight: 25,
  headerRowHeight: 23,
  columns: [
    {
      field: "index",
      caption: "#",
      width: 17,
      headerStyle: styleHeader,
      style: styleCell,
      action: inputEditor
    },
    {
      field: "operationType",
      caption: "Tipo de Operação",
      width: "150",
      headerStyle: styleHeader,
      style: styleCell,
      action: inputEditor
    },
    {
      field: "matriculation",
      caption: "Matrícula",
      width: 90,
      action: inputEditor
    },
    {
      field: "client",
      caption: "Cliente",
      width: 180,
      action: inputEditor
    },
    {
      field: "place",
      caption: "Posto",
      width: 180,
      action: inputEditor
    },
    {
      field: "reason",
      caption: "Motivo",
      width: 357,
      action: selectorReason,
    },
    {
      field: "hedgingFeature",
      caption: "Cobertura",
      width: 165,
      action: featureReason
    },
    {
      field: "startDate",
      caption: "Data Início",
      width: 95,
      action: inputEditor
    },
    {
      field: "endDate",
      caption: "Data Fim",
      width: 95,
      action: inputEditor
    },
    {
      field: "enterTimeManually",
      caption: "Inf. Horario Man.",
      width: 125,
      action: inputEditor
    },
    {
      field: "postCalculationProfile",
      caption: "Posto Util. no Perfil de Apur.",
      width: 210,
      action: inputEditor
    },
    {
      field: "employeeHours",
      caption: "Horario do Colaborador",
      width: 185,
      action: inputEditor
    },
    {
      field: "entry1",
      caption: "Entrada",
      width: 65,
      action: inputEditor
    },
    {
      field: "departure1",
      caption: "Saida",
      width: 50,
      action: inputEditor
    },
    {
      field: "entry2",
      caption: "Entrada",
      width: 65,
      action: inputEditor
    },
    {
      field: "departure2",
      caption: "Saida",
      width: 50,
      action: inputEditor
    },
    {
      field: "description",
      caption: "Descrição",
      width: 270,
      action: inputEditor
    },
  ],
  rows: [
    {
      index: "1",
      operationType: "Cobertura de Posto",
      matriculation: "X000001",
      client: "TESTE NEXTI",
      place: "CC - SP - TESTE",
      reason: "Hora-Extra - Programada Faturada",
      hedgingFeature: "HORA EXTRA",
      startDate: "21/12/2022",
      endDate: "21/12/2022",
      enterTimeManually: "Não",
      postCalculationProfile: "",
      employeeHours: "IM|08:00-17:00",
      entry1: "",
      departure1: "",
      entry2: "",
      departure2: "",
      description: "Acerto Hora Extra - Autorizada",
    },
    {
      index: "2",
      operationType: "Cobertura de Posto",
      matriculation: "X000001",
      client: "TESTE NEXTI",
      place: "CC - SP - TESTE",
      reason: "Hora-Extra - Programada Faturada",
      hedgingFeature: "FT",
      startDate: "21/12/2022",
      endDate: "21/12/2022",
      enterTimeManually: "Sim",
      postCalculationProfile: "",
      employeeHours: "",
      entry1: "07: 00",
      departure1: "11: 00",
      entry2: "12: 00",
      departure2: "18: 00",
      description: "Acerto Hora Extra - Autorizada",
    },
  ],
};

export default GridAdjust;
