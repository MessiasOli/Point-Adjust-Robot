<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn flat dense round icon="menu" aria-label="Menu" @click="toggleLeftDrawer" />

        <q-toolbar-title class="cursor-pointer" @click="sentToHome" flat>
          Robô de pontos
        </q-toolbar-title>

        <div>App {{ appVersion }}</div>
      </q-toolbar>
    </q-header>

    <q-drawer v-model="leftDrawerOpen" show-if-above bordered>
      <q-list>
        <q-item-label header> Menu </q-item-label>

        <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view class="container" />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent, ref } from "vue";
import EssentialLink from "components/EssentialLink.vue";

const linksList = [
  {
    title: "Home",
    caption: "",
    icon: "home",
    route: { name: "home" },
  },
  {
    title: "Ajuste de Marcação",
    caption: "",
    icon: "update",
    route: { name: "AdjustWorkShift" },
  },
  {
    title: "Cobertura de posto",
    caption: "",
    icon: "supervisor_account",
    route: { name: "CoverWorkShift" },
  },
  {
    title: "Lançar Ausência",
    caption: "",
    icon: "work_off",
    route: { name: "Absence" },
  },
  {
    title: "Configurações",
    caption: "",
    icon: "settings",
    route: { name: "Settings" },
  },
];

export default defineComponent({
  name: "MainLayout",

  components: {
    EssentialLink,
  },

  computed: {
    appVersion() {
      return this.$store.getters.appVersion;
    },
    // other computed values here
  },

  setup() {
    const leftDrawerOpen = ref(false);

    return {
      essentialLinks: linksList,
      leftDrawerOpen,
      toggleLeftDrawer() {
        leftDrawerOpen.value = !leftDrawerOpen.value;
      },
    };
  },

  methods: {
    sentToHome() {
      this.$router.push({ name: "home" }).catch(() => { });
    },
  },

  mounted() {
    console.log("this", this);
  },
});
</script>

<style scoped>
.container {
  padding: 8px;
  height: 93vh;
  overflow-y: auto;
}
</style>
