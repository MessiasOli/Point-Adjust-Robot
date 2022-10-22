<template>
  <div padding>
    <p>Ajuste de Marcação</p>
    <VGrid theme="compact" :source="rows" :columns="columns"></VGrid>
  </div>
</template>

<script>
import VGrid from "@revolist/vue3-datagrid";

export default {
  name: "AdjustWorkshift",

  components: {
    VGrid,
  },

  data() {
    return {
      columns: [{ prop: "name" }, { prop: "details" }],
      rows: [
        {
          name: "1",
          details: "Item 1",
        },
      ],
    };
  },
};
</script>

<style scoped></style>
