<template lang="">
  <div class="flex items-center header">
    <div
      v-if="!isSidebarShrink"
      class="toggle-sidebar flex justify-center items-center mi-24"
      @click="toggleSdibarShrink">
      <div class="icon icon--three-stripes"></div>
    </div>
    <div class="header-branch pointer flex">
      <div class="header-branch__name" title="CÔNG TY CỔ PHẦN MISA">
        CÔNG TY CỔ PHẦN MISA
      </div>
      <a href="#" class="flex items-center">
        <div class="icon icon--chevron-down mx-2-5"></div>
      </a>
    </div>
    <div
      class="notify flex items-center justify-center pointer"
      title="Thông báo">
      <div class="icon icon--notify"></div>
    </div>
    <div class="account-info flex items-center">
      <div class="user-ava flex justify-center items-center">
        <div class="icon icon--no-ava"></div>
      </div>
      <div class="user-name">
        <div class="display-user">Vu Dinh Tien</div>
      </div>
      <div class="account-info__more flex justify-center items-center pointer">
        <div class="icon icon--chevron-down-small"></div>
      </div>
    </div>
  </div>
</template>
<script>
import { computed } from "vue";
import { useStore } from "vuex";
export default {
  setup(props) {
    const store = useStore();
    const isSidebarShrink = computed(() => store.state.global.isSidebarShrink);

    return {
      isSidebarShrink,
      toggleSdibarShrink: () => store.dispatch("toggleSidebarShrink"),
    };
  },
};
</script>
<style>
@import url("./header.css");
</style>
