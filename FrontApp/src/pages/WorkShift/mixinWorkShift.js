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
          console.log("🦾🤖 >> stoped", stoped)
          console.log("🦾🤖 >> concluded", concluded)

          if (res.status == 200 && !concluded) {
            callback(res.data.msg);
            setTimeout(() => {this.getStatus(key, callback)}, 2000);
          } else if (res.status == 200 && concluded && !stoped) {
            callback(res.data.msg, "success");
            this.getJob(key)
          } else if(stoped){
            this.showMessage("Processo interrompido", "success");
            this.$store.commit("callStopJob", false)
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
  },

  mounted() {

  },

  destroyed() {  },
};
