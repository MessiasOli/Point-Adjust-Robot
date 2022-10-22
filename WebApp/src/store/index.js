import { createStore } from "vuex";
import axios from "axios";

// Create a new store instance.
const store = createStore({
  state() {
    return {
      count: 0,
      packageVersion: "0",
    };
  },
  mutations: {
    increment(state) {
      state.count++;
    },
    appVersion(state, value) {
      state.packageVersion = value;
    },
  },
  getters: {
    appVersion: (state) => {
      return state.packageVersion;
    },
  },
});

axios.get("../../package.json").then((res) => {
  store.commit("appVersion", res.data.version + "v");
});

export default store;
