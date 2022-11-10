<template>
  <div padding>
    <div class="header">
      <h6>Cobertura de Posto</h6>
      <q-input v-model="search" filled placeholder="Filtrar">
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
    </div>
    <div class="grid-cover"></div>

    <div class="footer">
      <div>
        <q-badge
          outline
          transparent
          align="middle"
          color="primary"
        >
          {{ rows.filter((a) => a.matriculation != "").length }}
        </q-badge>
        <q-tooltip :offset="[0, 8]">Ultima atualização.</q-tooltip>
      </div>
      <div class="group-btn">
        <div>
          <q-spinner-tail
            class="q-mr-lg"
            v-if="wait"
            color="purple"
            size="2.5em"
          />
          <q-tooltip :offset="[0, 8]">Aguardando retorno dos dados.</q-tooltip>
        </div>
        <q-btn
          @click="addLines"
          class="q-ml-sm"
          color="primary"
          icon="add"
          label="Adicionar mais linhas"
        />
        <q-btn
          @click="clearLines"
          class="q-ml-sm"
          color="primary"
          icon="clear_all"
          label="Limpar"
        />
        <q-btn
          @click="showDialog"
          class="q-ml-sm"
          color="primary"
          icon="auto_mode"
          label="Enviar dados"
        />
      </div>
    </div>
  </div>
</template>

<script>
import * as cheetahGrid from "cheetah-grid";
import GridAdjust from "./gridConfig";
import CommandCover from "../../../model/commands/CommandCover";
import { MixinWorkShift } from "../mixinWorkShift";

export default {
  name: "CoverWorkShift",
  mixins: [ MixinWorkShift ],
  inject: ["showMessage", "showWarning", "dialog", "working"],
  // components: {
  //   VGrid,
  // },

  data() {
    return {
      grid: {},
      columns: GridAdjust.columns,
      rows: [],
      rowsBkp: [],
      options: GridAdjust.options,
      wait: false,
      lastUpdate: "",
      search: "",
    };
  },

  watch: {
    search(filter, lastFilter) {
      if (!filter) {
        this.rows = this.rowsBkp;
        this.setGrid();
        return;
      }

      let filterUpper = filter.toUpperCase();
      this.rows = this.rowsBkp.filter((d) => {
        return (
          d.operationType.toUpperCase().includes(filterUpper) ||
          d.matriculation.toUpperCase().includes(filterUpper) ||
          d.client.toUpperCase().includes(filterUpper) ||
          d.place.toUpperCase().includes(filterUpper) ||
          d.reason.toUpperCase().includes(filterUpper) ||
          d.startDate.toUpperCase().includes(filterUpper) ||
          d.endDate.toUpperCase().includes(filterUpper) ||
          d.description.toUpperCase().includes(filterUpper) ||
          d.entry1.toUpperCase().includes(filterUpper) ||
          d.departure1.toUpperCase().includes(filterUpper) ||
          d.entry2.toUpperCase().includes(filterUpper) ||
          d.departure2.toUpperCase().includes(filterUpper) ||
          d.employeeHours.toUpperCase().includes(filterUpper) ||
          d.hedgingFeature.toUpperCase().includes(filterUpper)
        );
      });
      this.Add1000Lines();
      this.setGrid();
    },
  },

  methods: {
    updateRows(){
      this.saveWorkplace(this.grid.records)
    },

    addLines() {
      this.Add1000Lines();
      this.showMessage("1000 novas linhas adicionadas!", "success");
    },

    Add1000Lines() {
      for (let i = 0; i < 1000; i++)
        this.rows.push({
          index: "",
          operationType: "",
          matriculation: "",
          client: "",
          place: "",
          reason: "",
          hedgingFeature: "",
          startDate: "",
          endDate: "",
          enterTimeManually: "",
          postCalculationProfile: "",
          employeeHours: "",
          entry1: "",
          departure1: "",
          entry2: "",
          departure2: "",
          description: "",
        });
    },

    clearLines() {
      this.rows = [];
      this.saveWorkplace([])
      this.Add1000Lines();
      this.showMessage("Tabela limpa!", "success");
      this.setGrid();
    },

    showDialog() {
      this.dialog(
        "Todos os dados dessa tabela serão inseridos, deseja continuar ?",
        this.sendAdjusts
      );
    },

    getJob(keyJob){
      this.$api.get(`/getjob/${keyJob}`)
        .then((res) => {
          if (res.status == 200) {
            let untreatedData = JSON.parse(res.data.untreatedData);
            this.rows = untreatedData
            this.rowsBkp = untreatedData
            this.Add1000Lines();
            this.showMessage(`Tabela atualizada, ${res.data.completed} concluídos, restando ${this.rows.filter(i => i.matriculation != "").length} com falha.`);
            this.setGrid();
          }
        }).catch(console.error);
    },

    sendAdjusts() {
      let data = this.rows.filter((r) => r.matriculation != "");
      let command;

      if(data.length == 0) {
        this.showWarning("Não há dados dados para ser enviado");
        return
      }

      try {
        command = new CommandCover(data);
      } catch(e) {
        this.handleError(e)
        return;
      }
      this.$store.commit("callStopJob", false)
      let finish = this.working("Preparando iniciar...");
      this.$api
        .post(`/SetCoverWorkshift`, command)
        .then((res) => {
          if (res.status == 200) {
            setTimeout(() => {
              this.getStatus(res.data, finish);
            }, 2000);
          } else {
            finish(res.data);
          }
        })
        .catch((error) => {
          console.error(error);
          finish("Falha ao executar a inserção dos dados!", "falha");
        });
    },

    createGrid() {
      this.grid = new cheetahGrid.ListGrid({
        parentElement: document.querySelector(".grid-cover"),
        allowRangePaste: true,
        header: GridAdjust.columns,
        defaultRowHeight: GridAdjust.defaultRowHeight,
        headerRowHeight: GridAdjust.headerRowHeight
      });
      this.grid.theme = "BASIC";
    },

    setGrid(table = this.rows) {
      this.grid.records = table;
    },
  },

  mounted() {
    this.createGrid();
    this.rowsBkp = this.getWorkplace();
    this.rowsBkp = this.rowsBkp.length > 0 ? this.rowsBkp : GridAdjust.rows
    this.rows = this.rowsBkp;
    this.Add1000Lines()
    this.setGrid()
  },
};
</script>

<style>
.grid-cover .cheetah-grid__inline-menu--shown{
  top: 40px !important;
  max-height: 407px;
  min-width: 235px;
}
</style>

<style scoped>
.header {
  display: flex;
  justify-content: space-between;
  align-items: baseline;
}

h6 {
  margin: 20px 8px;
}

.grid-cover {
  height: calc(100% - 130px);
}

.group-btn {
  display: flex;
  margin: 8px 16px;
  justify-content: flex-end;
}

.footer {
  display: flex;
  justify-content: space-between;
}
</style>
