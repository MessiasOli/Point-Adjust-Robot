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
      showWarning: this.warning,
      dialog: this.dialog,
      working: this.working,
    };
  },

  methods: {
    show(message, spinnerInfo) {
      let type = spinnerInfo == "success" ? "positive" : null;

      this.$q.notify({
        type,
        spinner: icons(spinnerInfo),
        message: message,
        position: "bottom-left",
        color: "purple",
        timeout: 3000,
        progress: true,
      });
    },

    warning(message) {
      this.$q.notify({
        type: "warning",
        message: message,
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
      var allReady = true;
      const dialog = this.$q
        .dialog({
          title: message,
          dark: true,
          progress: {
            spinner: QSpinnerGears,
            color: "amber",
          },
          persistent: true, // we want the user to not be able to close it
          ok: false, // we want the user to not be able to close it
          cancel: {
            label: "Parar",
            push: true,
            color: "negative",
          },
        })
        .onCancel((event) => {
          console.log("🦾🤖 >> event", event)
          setTimeout(() => {
            if (allReady) {
              this.$store.commit("callStopJob", true);
              this.$api.get("stopwork");
              this.show(
                "Interrompendo o processo...",
                "report_problem"
              );
            }
          }, 500);
        });

      var finish = (message, fail) => {
        allReady = !(fail || (fail && !fail.includes("falha")));
        let success = fail == "success";

        dialog.update({
          title: !success
            ? allReady
              ? "Processando dados..."
              : "Ops... aconteceu algo errado!"
            : "Processo concluído",
          message: message,
          progress: success || !allReady ? false : true,
          ok: success ? true : false,
          cancel: !success ? {
            label: "Parar",
            push: true,
            color: "negative",
          } : false,
        });
      };

      return finish;
    },
  },
});

var icons = function (notify) {
  switch (notify) {
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
