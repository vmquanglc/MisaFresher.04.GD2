<template>
  <div class="flex flex-row layout-default">
    <BaseSidebar />
    <div
      class="flex-1 content-area-default"
      :class="{ 'content-area-default--sidebar-shrink': isSidebarShrink }">
      <div class="content-wrapper">
        <div class="content-wrapper__header">
          <BaseHeader />
        </div>
        <div class="content-wrappr__body">
          <slot></slot>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { computed } from "vue";
import { useStore } from "vuex";

import BaseSidebar from "@/layouts/components/BaseSidebar/BaseSidebar.vue";
import BaseHeader from "../components/BaseHeader/BaseHeader.vue";
export default {
  components: {
    BaseSidebar,
    BaseHeader,
  },
  setup(props) {
    const store = useStore();
    const isSidebarShrink = computed(() => store.state.global.isSidebarShrink);
    return {
      isSidebarShrink,
    };
  },
};
</script>
<style>
@import url("./layout.css");
</style>
