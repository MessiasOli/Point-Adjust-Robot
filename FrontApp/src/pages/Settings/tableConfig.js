const Table = {
  header: [
    {
      name: "timeStamp",
      required: true,
      label: "Data e Hora",
      align: "left",
      // style: "width: 200px",
      field: (row) => row.timeStamp,
      format: (val) => `${ val }`,
      sortable: true,
    },
    {
      name: "info",
      required: true,
      label: "Informação",
      align: "left",
      style: "width: 200px",
      field: (row) => row.info,
      format: (val) => `${val}`,
      sortable: true,
    },
    {
      name: "level",
      required: true,
      label: "Nível",
      align: "left",
      field: "level",
      style: "width: 200px",
    },
    {
      name: "step",
      required: true,
      align: "center",
      label: "Momento de parada",
      style: "width: 500px",
      field: "step",
      sortable: true,
    },
  ],
};

export default Table;
