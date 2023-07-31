var Auth = {
  set(user, password, showChrome) {
    let config = btoa(
      JSON.stringify({
        user,
        password,
        showChrome,
      })
    );

    localStorage._resu = config;
  },

  get() {
    let user;
    let password;
    let showChrome;

    if (localStorage._resu) {
      let config = JSON.parse(atob(localStorage._resu));
      user = config.user;
      password = config.password;
      showChrome = config.showChrome;
    }
    return { user, password, showChrome };
  },

  getEncrypted() {
    let data = this.get();

    if (!data.user || !data.password) throw "Não há usuário NEXTI cadastrado.";

    if (localStorage._resu) return localStorage._resu;
  },
};

export { Auth };
