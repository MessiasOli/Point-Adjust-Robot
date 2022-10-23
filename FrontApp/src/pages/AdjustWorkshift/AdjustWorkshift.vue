<template>
  <div padding>
    <h6>Ajuste de Marcação</h6>

    <v-grid
      class="table"
      theme="compact"
      :source="rows"
      :columns="columns"
      :options="options"
      :range="true"
    ></v-grid>
    <div class="footer">
      <div>
        <q-badge
          v-if="lastUpdate"
          outline
          transparent
          align="middle"
          color="primary"
        >
          {{ lastUpdate }}
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
import VGrid from "@revolist/vue3-datagrid";
import GridAdjust from "./gridConfig";
import * as moment from "moment";

export default {
  name: "AdjustWorkshift",

  inject: ["showMessage", "dialog", "working"],
  components: {
    VGrid,
  },

  data() {
    return {
      columns: GridAdjust.columns,
      rows: GridAdjust.rows,
      options: GridAdjust.options,
      wait: false,
      lastUpdate: "", //moment().format("DD/MM/yyyy HH:mm:ss"),
    };
  },

  methods: {
    addLines() {
      this.Add1000Lines();
      this.showMessage("1000 novas linhas adicionadas!", "success");
    },

    Add1000Lines() {
      for (let i = 0; i < 1000; i++)
        this.rows.push({
          matriculation: "",
          data: "",
          hour: "",
          reference: "",
          justification: "",
          note: "",
        });
    },

    clearLines() {
      this.rows = [];
      this.Add1000Lines();
      this.showMessage("Tabela limpa!", "success");
    },

    showDialog() {
      this.dialog(
        "Todos os dados dessa tabela serão inseridos, deseja continuar ?",
        this.sendAdjusts
      );
    },

    sendAdjusts() {
      let finish = this.working("Por favor aguarde...");
      let data = this.rows.filter((r) => r.matriculation != "");

      this.$api
        .post(`/adjustworkshift`, data)
        .then((res) => {
          if (res.status == 200) {
            finish(res.data);

            this.lastUpdate = moment().format("DD/MM/yyyy HH:mm:ss");
          } else {
            finish(res.data);
          }
        })
        .catch((error) => {
          console.error(error);
          finish("Falha ao executar a inserção dos dados!", "falha");
        });
    },
  },
  mounted() {},
};
</script>

<style scoped>
.table {
  height: calc(100% - 180px);
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
