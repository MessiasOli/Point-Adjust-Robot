<template>
  <div class="container-settings">
    <div class="settings">
      <div>
        <label>Digite aqui o usuário Nexti que será utilizado no robô</label>
        <div class="auth" @submit="onSubmit" @reset="onReset">
          <q-input
            class="q-pa-sm"
            filled
            v-model="user"
            label="Usuário nexti"
            lazy-rules
            :rules="[
              (val) =>
                (val && val.length > 0) || 'Necessário inserir um login válido',
            ]"
            @change="saveConfig"
            :dense="true"
          />
          <q-input
            class="q-pa-sm"
            v-model="password"
            filled
            :type="isPwd ? 'password' : 'text'"
            label="Senha Nexti"
            @change="saveConfig"
            :rules="[
              (val) =>
                (val && val.length > 0) || 'Necessário inserir um login válido',
            ]"
            :dense="true"
          >
            <template v-slot:append>
              <q-icon
                :name="isPwd ? 'visibility_off' : 'visibility'"
                class="cursor-pointer"
                @click="isPwd = !isPwd"
              />
            </template>
          </q-input>
        </div>
      </div>
      <div>
        <q-toggle
          v-model="showChrome"
          checked-icon="check"
          color="green"
          label="Abrir Chrome ao executar as rotinas."
          unchecked-icon="clear"
        />
        <q-btn
          @click="clearLogs"
          icon-right="delete"
          color="primary"
          label="Limpar Logs"
          unchecked-icon="clear"
        />
      </div>
    </div>
    <hr />
    <q-table
      ref="table"
      title="Registros do sistema"
      :filter="filter"
      :rows="rows"
      :columns="columns"
      row-key="timeStamp"
      class="my-sticky-virtscroll-table"
      virtual-scroll
      :rows-per-page-options="[0]"
      :virtual-scroll-sticky-size-start="48"
    >
      <template v-slot:top-right>
        <q-input
          borderless
          dense
          debounce="200"
          color="primary"
          v-model="filter"
        >
          <template v-slot:append>
            <q-icon name="search" />
          </template>
        </q-input>
      </template>

      <template v-slot:header="props">
        <q-tr :props="props">
          <q-th auto-width />
          <q-th v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.label }}
          </q-th>
        </q-tr>
      </template>

      <template v-slot:body="props">
        <q-tr :props="props">
          <q-td auto-width>
            <q-btn
              size="sm"
              color="accent"
              round
              dense
              @click="show(props)"
              :icon="props.expand ? 'remove' : 'add'"
            ></q-btn>
          </q-td>
          <q-td v-for="col in props.cols" :key="col.name" :props="props">
            {{ col.value }}
          </q-td>
        </q-tr>
        <q-tr v-show="props.expand" :props="props">
          <q-td colspan="100%">
            <div class="text-left">
              <p>Passo: {{ props.row.step }} - {{ props.row.data }}.</p>
              <p>Falha: {{ props.row.message }}</p>
            </div>
          </q-td>
        </q-tr>
      </template>
    </q-table>
  </div>
</template>

<script>
import { ref } from "vue";
import Table from "./tableConfig";
import { Auth } from "../../config/global";

export default {
  name: "Settigns-vue",

  inject: ["showMessage", "showWarning", "dialog", "working"],

  setup() {
    return {
      filter: ref(""),
      columns: Table.header,
    };
  },
  data() {
    return {
      user: "",
      password: "",
      isPwd: true,
      rows: [],
      showChrome: true,
    };
  },

  watch: {
    showChrome() {
      this.saveConfig();
    },
  },

  methods: {
    show(props) {
      props.expand = !props.expand;
      console.log("🦾🤖 >> props", props);
    },

    loadLogs() {
      this.$api.get("/getlogs").then((res) => {
        if (res.status == 200) {
          this.filter = "rows";
          this.rows = res.data;
          setTimeout(() => {
            this.filter = "";
          }, 400);
        }
      });
    },

    sentClearLogs() {
      this.$api.get("/DeleteLogs").then((res) => {
        if (res.status == 200) {
          this.filter = "rows";
          this.rows = [];
          setTimeout(() => {
            this.filter = "";
            this.showMessage("Registros do sistema apagados com sucesso!");
          }, 400);
        }
      });
    },

    clearLogs() {
      this.dialog(
        "Os registros são apagados automaticamente em 2 dias, ainda deseja apagar ?",
        this.sentClearLogs
      );
    },

    saveConfig() {
      Auth.set(this.user, this.password, this.showChrome);
      console.log("🦾🤖 >> this.showChrome", this.showChrome);
    },

    getSettings() {
      let { user, password, showChrome } = Auth.get();
      this.user = user;
      this.password = password;
      this.showChrome = showChrome;
      console.log("🦾🤖 >> showChrome", showChrome);
    },
  },

  beforeMount() {
    this.loadLogs();
    this.getSettings();
  },
};
</script>

<style lang="scss" scoped>
.container-settings {
  height: 93vh;
}

.settings {
  display: flex;
  justify-content: space-around;
}

.settings > div:last-child {
  display: flex;
  flex-direction: column;
}

.auth {
  display: flex;
  align-items: center;
  width: 100%;
  margin-bottom: 15px;
}

.my-sticky-virtscroll-table {
  /* height or max-height is important */
  height: calc(100% - 110px);
}

.q-table__top,
.q-table__bottom,
thead tr:first-child th {
  /* bg color is important for th; just specify one */
  background-color: #fff;
}

thead tr th {
  position: sticky;
  z-index: 1;
}
/* this will be the loading indicator */
thead tr:last-child th {
  top: 48px;
}
/* height of all previous header rows */
thead tr:first-child th {
  top: 0;
}
</style>
