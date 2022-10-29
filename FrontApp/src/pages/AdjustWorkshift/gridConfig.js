const GridAdjust = {
  opstions: {
    canFocus: true,
    range: true,
  },
  columns: [
    {
      prop: "matriculation",
      name: "Matrícula",
      sortable: true,
    },
    {
      prop: "data",
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
    { prop: "justification", name: "Justificativa", sortable: true, size: 300 },
    { prop: "note", name: "Observações", sortable: true, size: 300 },
  ],
  rows: [
    {
      matriculation: "X000001",
      data: "20/10/2022",
      hour: "12:30",
      replaceTime: "",
      reference: "20/10/2022",
      justification: "Outros",
      note: "Acerto 3 Marcações segundo Política de Ponto",
    },
    {
      matriculation: "X000001",
      data: "21/10/2022",
      hour: "12:30",
      replaceTime: "",
      reference: "21/10/2022",
      justification: "Outros",
      note: "Acerto 3 Marcações segundo Política de Ponto",
    },
    {
      matriculation: "X000001",
      data: "21/10/2022",
      replaceTime: "",
      hour: "12:30",
      reference: "21/10/2022",
      justification: "Outros",
      note: "Acerto 3 Marcações segundo Política de Ponto",
    },
    {
      matriculation: "X000001",
      data: "21/10/2022",
      replaceTime: "",
      hour: "12:30",
      reference: "21/10/2022",
      justification: "Outros",
      note: "Acerto 3 Marcações segundo Política de Ponto",
    },
  ],
};

for (let i = 1; i < 1000; i++)
  GridAdjust.rows.push({
    matriculation: "",
    data: "",
    hour: "",
    reference: "",
    justification: "",
    note: "",
  });

export default GridAdjust;
