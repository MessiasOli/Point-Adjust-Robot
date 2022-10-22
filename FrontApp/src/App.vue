<template>
  <router-view />
</template>

<script>
import { defineComponent } from "vue";
import { useQuasar, QSpinnerGears } from "quasar";

export default defineComponent({
  name: "App",
  data() {
    return {
      $q: useQuasar(),
    };
  },

  provide() {
    return {
      showMessage: this.show,
    };
  },

  methods: {
    show(message, spinner) {
      let type = "success" ? "positive" : null;

      this.$q.notify({
        type,
        spinner: icons(spinner),
        message: message,
        position: "bottom-left",
        color: !spinner
          ? "primary"
          : spinner == "success"
          ? "primary"
          : "purple",
        timeout: 3000,
        progress: true,
      });
    },
  },
});

var icons = function (notify) {
  switch (notify) {
    case "warning":
      return "warning";
    case "alert":
      return "report_problem";
    case "build":
      return QSpinnerGears;
    case "wait":
      return true;
    default:
      return;
  }
};
</script>
