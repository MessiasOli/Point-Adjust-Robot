<template>
  <div class="main">
    <div class="main-header">
      <h1>Robô de Pontos</h1>
      <p>Auxilia na edição de pontos e adição de postos</p>
    </div>
    <div class="main-input">
      <label for="fileSelect">Clique aqui para importar o excell</label>
      <input
        type="file"
        ref="file"
        @change="selectedFile"
        hidden="true"
        id="fileSelect"
        accept=".csv, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
      />
    </div>
  </div>
</template>

<script>
export default {
  setup() {
    return {};
  },
};
</script>

<style scoped>
.main {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 50vh;
}

.main-header {
  width: 60%;
  text-align: center;
}

.main-input > label {
  cursor: pointer;
}

.main-input > button {
  margin-left: 4px;
}

h1 {
  font-size: 3.2 rem;
  margin: 0;
}
</style>
