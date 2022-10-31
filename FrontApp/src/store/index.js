import { createStore } from "vuex";
import axios from "axios";

// Create a new store instance.
const store = createStore({
  state() {
    return {
      count: 0,
      packageVersion: "0",
      lastAdjustWorkshift: "-",
      lastCoverWorkshift: "-",
      callStopJob: false,
    };
  },
  mutations: {
    increment(state) {
      state.count++;
    },
    appVersion(state, value) {
      state.packageVersion = value;
    },
    callStopJob(state, value){
      state.callStopJob = value;
    },
    lastAdjustWorkshift(state, value) {
      state.lastAdjustWorkshift = value;
    },
    lastCoverWorkshift(state, value) {
      state.lastCoverWorkshift = value;
    },
  },

  getters: {
    appVersion: (state) => {
      return state.packageVersion;
    },
    callStopJob: (state) => {
      return state.callStopJob;
    },
    lastAdjustWorkshift: (state) => {
      return state.lastAdjustWorkshift;
    },
  },
});

axios.get("../../package.json").then((res) => {
  store.commit("appVersion", res.data.version + "v");
});

export default store;
