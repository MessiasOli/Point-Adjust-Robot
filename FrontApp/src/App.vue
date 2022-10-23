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
      dialog: this.dialog,
      working: this.working,
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

    dialog(message, confirm = () => {}, notConfirm = () => {}) {
      this.$q
        .dialog({
          title: "Atenção",
          message: message,
          cancel: true,
          persistent: true,
          ok: {
            label: "Sim",
            push: true,
          },
          cancel: {
            label: "Não",
            push: true,
            color: "negative",
          },
        })
        .onOk(() => {
          confirm();
        })
        .onCancel(() => {
          notConfirm();
        })
        .onDismiss(() => {
          // console.log('I am triggered on both OK and Cancel')
        });
    },

    working(message) {
      const dialog = this.$q.dialog({
        title: message,
        dark: true,
        progress: {
          spinner: QSpinnerGears,
          color: "amber",
        },
        persistent: true, // we want the user to not be able to close it
        ok: false, // we want the user to not be able to close it
      });

      var finish = (message, fail) => {
        console.log("fail", fail);
        dialog.update({
          title: fail ? "Ops... aconteceu algo errado" : "Processo concluído!",
          message: message,
          progress: false,
          ok: true,
        });
      };

      return finish;
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
