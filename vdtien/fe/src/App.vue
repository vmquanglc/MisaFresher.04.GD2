<template>
  <component :is="layout">
    <router-view />
  </component>
</template>

<script>
import { useRoute } from "vue-router";
import { computed } from "vue";
import { PUB_LAYOUT } from "./enums";
export default {
  name: "App",
  components: {},
  setup() {
    const route = useRoute();
    return {
      layout: computed(() => (route.meta?.layout || PUB_LAYOUT) + "-layout"),
    };
  },
};
</script>

<style>
@import url("@/assets/styles/global.css");
</style>
