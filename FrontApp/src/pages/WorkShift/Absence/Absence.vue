<template>
  <div padding>
    <div class="header">
      <h6>Lançar Ausência</h6>
      <q-input v-model="search" filled placeholder="Filtrar">
        <template v-slot:append>
          <q-icon name="search" />
        </template>
      </q-input>
    </div>

    <div class="grid-absence"></div>

    <div class="footer">
      <div>
        <q-badge outline transparent align="middle" color="primary">
          {{ rows.filter((a) => a.matriculation != "").length }}
        </q-badge>
        <q-tooltip :offset="[0, 8]">Número de linhas preenchidas.</q-tooltip>
      </div>
      <div class="group-btn">
        <div>
          <q-spinner-tail class="q-mr-lg" v-if="wait" color="purple" size="2.5em" />
          <q-tooltip :offset="[0, 8]">Aguardando retorno dos dados.</q-tooltip>
        </div>
        <q-btn @click="addLines" class="q-ml-sm" color="primary" icon="add" label="Adicionar mais linhas" />
        <q-btn @click="clearLines" class="q-ml-sm" color="primary" icon="clear_all" label="Limpar" />
        <q-btn @click="showDialog" class="q-ml-sm" color="primary" icon="auto_mode" label="Enviar dados" />
      </div>
    </div>
  </div>
</template>

<script>
import * as cheetahGrid from "cheetah-grid";
import CommandAbsence from "../../../services/commands/CommandAbsence";
import GridAbsence from "./gridAbsence";
import { MixinWorkShift } from "../mixinWorkShift";

export default {
  name: "Absense-Vue",
  mixins: [MixinWorkShift],
  inject: ["showMessage", "showWarning", "dialog", "working"],

  data() {
    return {
      grid: {},
      columns: GridAbsence.columns,
      rows: [],
      rowsBkp: [],
      options: GridAbsence.options,
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
      this.rows = this.rowsBkp.filter((d) => JSON.stringify(d).toUpperCase().includes(filterUpper));
      this.Add1000Lines();
      this.setGrid();
    },
  },

  methods: {
    updateRows() {
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
        this.sendAbsences
      );
    },

    getJob(keyJob) {
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

    sendAbsences() {
      let data = this.rows.filter(r => {
        return r.matriculation != ""
      })
      console.log("🦾🤖 >> data", data)
      let commandAbsence;

      if (data.length == 0) {
        this.showWarning("Não há dados dados para ser enviado");
        return
      }

      try {
        commandAbsence = new CommandAbsence(data);
      } catch (e) {
        console.error("🦾🤖 Tipo de dado", typeof e)
        console.error("🦾🤖 >> e", e)
        this.handleError(e)
        return;
      }
      this.$store.commit("callStopJob", false)
      let finish = this.working("Preparando inicio...");
      this.$api
        .post(`/SetAbsenceWorkshift`, commandAbsence)
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
        parentElement: document.querySelector(".grid-absence"),
        allowRangePaste: true,
        header: GridAbsence.columns,
        defaultRowHeight: GridAbsence.defaultRowHeight,
        headerRowHeight: GridAbsence.headerRowHeight
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
    this.rowsBkp = this.rowsBkp.length > 0 ? this.rowsBkp : GridAbsence.rows
    this.rows = this.rowsBkp;
    this.Add1000Lines()
    this.setGrid()
  },
}
</script>

<style>
.grid-absence .cheetah-grid__inline-menu,
.grid-absence .cheetah-grid__inline-menu--shown {
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

.grid-absence {
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
