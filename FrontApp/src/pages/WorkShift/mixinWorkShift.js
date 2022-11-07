export var MixinWorkShift = {
  name:"EmptyMixin",
  data() {
    return {
    }
  },

  watch:{ },

  methods: {
    getStatus(key, callback) {
      this.$api.get(`/getstatus/${key}`).then((res) => {
          let concluded = res.data.msg.toLocaleLowerCase().includes("finalizado") ||
                          res.data.msg.toLocaleLowerCase().includes("processado");

          let stoped = this.$store.getters.callStopJob;

          if (res.status == 200 && !concluded) {
            callback(res.data.msg);
            setTimeout(() => {this.getStatus(key, callback)}, 2000);
          } else if (res.status == 200 && concluded && !stoped) {
            callback(res.data.msg, "success");
            this.getJob(key)
          } else if(stoped){
            this.showMessage("Processo interrompido", "success");
            this.$store.commit("callStopJob", false)
            this.getJob(key)
          }else{
            setTimeout(() => {
              this.getStatus(key, callback);
            }, 2000);
          }

        })
        .catch((error) => {
          console.error(error);
          callback("Falha ao executar a inserção dos dados!", "falha");
        });
    },

    handleError(e){
      this.showWarning(e);

      if(e.includes("usuário nexti"))
        this.$router.push({ name: "Settings" }).catch(() => {});
    },

    getAdjustWorkshift(){
      return localStorage.replacements ?  JSON.parse(localStorage.replacements) : []
    },

    saveAdjustWorkshift(adjust = Array){
      adjust = adjust.filter(a => a.matriculation);
      localStorage.replacements = JSON.stringify(adjust)
    },

    getWorkplace(){
      return localStorage.workplace ?  JSON.parse(localStorage.workplace) : []
    },

    saveWorkplace(workplace = Array){
      workplace = workplace.filter(a => a.matriculation)
      localStorage.workplace = JSON.stringify(workplace)
    },
  },

  mounted() {

  },

  destroyed() {  },
};
