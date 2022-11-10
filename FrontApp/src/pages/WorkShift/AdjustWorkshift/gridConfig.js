import * as cheetahGrid from "cheetah-grid";

var justificationOptions = [
  { value: null, caption: "Vazio" },
  { value: "Cobertura de posto", caption: "Cobertura de posto" },
  { value: "Cobrindo Férias", caption: "Cobrindo Férias" },
  { value: "Divergencia de escala", caption: "Divergencia de escala" },
  { value: "Duplicidade de marcação", caption: "Duplicidade de marcação" },
  { value: "Entrada antecipada", caption: "Entrada antecipada" },
  { value: "Entrada tardia", caption: "Entrada tardia" },
  { value: "Erro de Sistema", caption: "Erro de Sistema" },
  { value: "Erro operacional", caption: "Erro operacional" },
  { value: "Erro terminal", caption: "Erro terminal" },
  { value: "Esquecimento", caption: "Esquecimento" },
  { value: "Falta de energia", caption: "Falta de energia" },
  { value: "Outros", caption: "Outros" },
  { value: "Posto Incorreto", caption: "Posto Incorreto" },
  { value: "Problema na biometria", caption: "Problema na biometria" },
  { value: "Saida antecipada", caption: "Saida antecipada" },
  { value: "Saida tardia", caption: "Saida tardia" },
  { value: "Sistema indisponivel", caption: "Sistema indisponivel" },
];


var inputEditor = new cheetahGrid.columns.action.InlineInputEditor();
var justificationReason = new cheetahGrid.columns.action.InlineMenuEditor({ options: justificationOptions, });


const GridAdjust = {
  eventTypes: { ...cheetahGrid.ListGrid.EVENT_TYPE },
  defaultRowHeight: 25,
  headerRowHeight: 23,
  columns: [
    {
      field: "index",
      caption: "#",
      minWidth: 30,
      width: "3.44%",
      action: inputEditor
    },
    {
      field: "matriculation",
      caption: "Matrícula",
      width: "9.17%",
      minWidth: "80",
      action: inputEditor
    },
    {
      field: "date",
      caption: "Data",
      width: "10.32%",
      minWidth: 90,
      action: inputEditor
    },
    {
      field: "replaceTime",
      caption: "Corrigir marcação",
      width: "15.48%",
      minWidth: "135",
      action: inputEditor
    },
    {
      field: "hour",
      caption: "Hora",
      width: "5.73%",
      minWidth: 50,
      action: inputEditor
    },
    {
      field: "reference",
      caption: "Referência",
      width: "10.32%",
      minWidth: 90,
      action: inputEditor
    },
    {
      field: "justification",
      caption: "Justificativa",
      width: "30.96%",
      minWidth: 270,
      action: justificationReason
    },
    {
      field: "note",
      caption: "Observações",
      width: "12.61%",
      minWidth: 110,
      action: inputEditor
    },
  ],
  rows: [
    {
      "matriculation": "X000001",
      "date": "03/11/2022",
      "hour": "08:45",
      "reference": "03/11/2022",
      "justification": "Outros",
      "note": "teste",
      "index": "1",
      "replaceTime": ""
    },
    {
      "matriculation": "X000001",
      "date": "03/11/2022",
      "hour": "08:00",
      "reference": "03/11/2022",
      "justification": "Outros",
      "note": "teste",
      "index": "2",
      "replaceTime": "08:45"
    },
    {
      "matriculation": "X000001",
      "date": "03/11/2022",
      "hour": "08:00",
      "reference": "03/11/2022",
      "justification": "Outros",
      "note": "teste",
      "index": "3",
      "replaceTime": "cancelar"
    }
  ]
};

for (let i = 1; i < 1000; i++)
  GridAdjust.rows.push({
    matriculation: "",
    date: "",
    hour: "",
    reference: "",
    justification: "",
    note: "",
  });

export default GridAdjust;
