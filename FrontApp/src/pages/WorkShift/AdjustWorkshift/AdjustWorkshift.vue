<template>
  <div padding>
    <div class="header">
      <h6>Ajuste de Marcação</h6>
      <q-input v-model="search" filled placeholder="Filtrar">
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
    </div>

    <div class="grid-adjust"></div>

    <div class="footer">
      <div>
        <q-badge outline transparent align="middle" color="primary">
          {{ rows.filter((a) => a.matriculation != "").length }}
        </q-badge>
        <q-tooltip :offset="[0, 8]">Número de linhas preenchidas.</q-tooltip>
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
import CommandAdjust from "../../../services/commands/CommandAdjust";
import { MixinWorkShift } from "../mixinWorkShift";

export default {
  name: "AdjustWorkshift",
  mixins: [ MixinWorkShift ],
  inject: ["showMessage", "showWarning", "dialog", "working"],

  data() {
    return {
      grid: {},
      columns: GridAdjust.columns,
      rows: [],
      rowsBkp: [],
      options: GridAdjust.options,
      wait: false,
      lastUpdate: "", //moment().format("DD/MM/yyyy HH:mm:ss"),
      search: "",
    };
  },

  watch: {
    search(filter) {
      if (!filter) {
        this.rows = this.rowsBkp;
        this.setGrid();
        return;
      }

      let filterUpper = filter.toUpperCase();
      this.rows = this.rowsBkp.filter((d) => {
        return (
          d.matriculation && d.matriculation.toUpperCase().includes(filterUpper) ||
          d.data && d.data.toUpperCase().includes(filterUpper) ||
          d.hour && d.hour.toUpperCase().includes(filterUpper) ||
          d.reference && d.reference.toUpperCase().includes(filterUpper) ||
          d.justification && d.justification.toUpperCase().includes(filterUpper) ||
          d.note && d.note.toUpperCase().includes(filterUpper)
        );
      });
      this.Add1000Lines();
      this.setGrid();
    },
  },

  methods: {
    updateRows(){
      this.saveAdjustWorkshift(this.grid.records)
    },

    addLines() {
      this.Add1000Lines();
      this.showMessage("1000 novas linhas adicionadas!", "success");
      this.setGrid()
    },

    Add1000Lines() {
      for (let i = 0; i < 1000; i++)
        this.rows.push({
          matriculation: "",
          date: "",
          hour: "",
          reference: "",
          justification: "",
          note: "",
        });
    },

    clearLines() {
      this.rows = [];
      this.saveAdjustWorkshift([])
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
      let commandAdjust;

      if(data.length == 0) {
        this.showWarning("Não há dados dados para ser enviado");
        return
      }

      try {
        commandAdjust = new CommandAdjust(data);
      } catch (e){
        console.error("🦾🤖 >> e", e)
        this.handleError(e)
        return;
      }
      this.$store.commit("callStopJob", false)
      let finish = this.working("Preparando inicio...");
      this.$api
        .post(`/adjustworkshift`, commandAdjust)
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
        parentElement: document.querySelector(".grid-adjust"),
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
    this.rowsBkp = this.getAdjustWorkshift();
    this.rowsBkp = this.rowsBkp.length > 0 ? this.rowsBkp : GridAdjust.rows
    this.rows = this.rowsBkp;
    this.Add1000Lines()
    this.setGrid()
  },
};
</script>

<style>
.grid-adjust .cheetah-grid__inline-menu--shown{
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

.grid-adjust {
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
