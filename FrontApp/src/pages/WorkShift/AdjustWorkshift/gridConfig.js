const GridAdjust = {
  opstions: {
    canFocus: true,
    range: true,
  },
  columns: [
    {
      prop: "index",
      name: "#",
      sortable: true,
      size: 60,
    },
    {
      prop: "matriculation",
      name: "Matrícula",
      sortable: true,
    },
    {
      prop: "date",
      name: "Data",
      sortable: true,
      size: 120,
    },
    {
      prop: "replaceTime",
      name: "Corrigir marcação",
      size: 145,
      sortable: true,
    },
    { prop: "hour", name: "Hora", sortable: true, size: 100 },
    { prop: "reference", name: "Referência", sortable: true, size: 120 },
    { prop: "justification", name: "Justificativa", sortable: true, size: 270 },
    { prop: "note", name: "Observações", sortable: true, size: 270 },
  ],
  rows: [
    // {
    //   "matriculation": "X000001",
    //   "date": "03/11/2022",
    //   "hour": "08:45",
    //   "reference": "03/11/2022",
    //   "justification": "Outros",
    //   "note": "teste",
    //   "index": "1",
    //   "replaceTime": ""
    // },
    // {
    //   "matriculation": "X000001",
    //   "date": "03/11/2022",
    //   "hour": "08:00",
    //   "reference": "03/11/2022",
    //   "justification": "Outros",
    //   "note": "teste",
    //   "index": "2",
    //   "replaceTime": "08:45"
    // },
    // {
    //   "matriculation": "X000001",
    //   "date": "03/11/2022",
    //   "hour": "08:00",
    //   "reference": "03/11/2022",
    //   "justification": "Outros",
    //   "note": "teste",
    //   "index": "3",
    //   "replaceTime": "cancelar"
    // }
    // ultimo teste
  //   {
  //     "date": "04/11/2022",
  //     "hour": "14:03",
  //     "replaceTime": "",
  //     "reference": "04/11/2022",
  //     "justification": "Esquecimento",
  //     "note": "Ajuste 3 marcações, segundo\\n  Política de Ponto_teste7Nov",
  //     "index": "1",
  //     "matriculation": "1680",
  //     "Key": "1-1680"
  // },
  // {
  //     "date": "04/11/2022",
  //     "hour": "12:19",
  //     "replaceTime": "",
  //     "reference": "04/11/2022",
  //     "justification": "Esquecimento",
  //     "note": "Ajuste 3 marcações, segundo Política de Ponto_teste7Nov",
  //     "index": "2",
  //     "matriculation": "2055",
  //     "Key": "2-2055"
  // }

  {
    "date": "04/11/2022",
    "hour": "14:03",
    "replaceTime": "",
    "reference": "04/11/2022",
    "justification": "Esquecimento",
    "note": "Ajuste 3 marcações, segundo\\n  Política de Ponto_teste7Nov",
    "index": "1",
    "matriculation": "1680",
    "Key": "1-1680"
},
{
    "date": "04/11/2022",
    "hour": "12:19",
    "replaceTime": "",
    "reference": "04/11/2022",
    "justification": "Esquecimento",
    "note": "Ajuste 3 marcações, segundo Política de Ponto_teste7Nov",
    "index": "2",
    "matriculation": "2055",
    "Key": "2-2055"
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
