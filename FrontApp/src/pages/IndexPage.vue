<template>
  <q-page padding>
    <q-card class="my-card">
      <q-card-section class="bg-primary text-white">
        <div class="text-h6">Automatize seus processos</div>
        <div class="text-subtitle2">Escolha uma das opções</div>
      </q-card-section>
      <q-separator />

      <q-card-actions align="left">
        <q-btn to="AdjustWorkShift" flat>Ajuste de Marcação</q-btn>
        <q-btn to="CoverWorkShift" flat>Cobertura de Posto</q-btn>
        <q-btn to="Absence" flat>Lançar Ausência</q-btn>
      </q-card-actions>
    </q-card>
  </q-page>
</template>

<script>
import { defineComponent } from "vue";

export default defineComponent({
  name: "IndexPage",
});
</script>
