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
        @click="sendAdjusts"
        class="q-ml-sm"
        color="primary"
        icon="auto_mode"
        label="Enviar dados"
      />
    </div>
  </div>
</template>

<script>
import VGrid from "@revolist/vue3-datagrid";
import GridAdjust from "./gridConfig";

export default {
  name: "AdjustWorkshift",
  inject: ["showMessage"],
  components: {
    VGrid,
  },

  data() {
    return {
      columns: GridAdjust.columns,
      rows: GridAdjust.rows,
      options: GridAdjust.options,
      wait: false,
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

    sendAdjusts() {
      this.showMessage("Iniciando trabalho!", "build");
      this.wait = true;
      setTimeout(() => {
        this.wait = false;
        this.showMessage("Trabalho concluído!", "successs");
      }, 7000);
    },
  },
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
</style>
