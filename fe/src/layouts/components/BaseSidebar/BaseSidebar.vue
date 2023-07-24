<template>
  <div class="sidebar" :class="{ 'sidebar--shrink': isSidebarShrink }">
    <div v-if="!isSidebarShrink" class="logo-container">
      <a href="#" class="menu-container icon icon--option"> </a>
      <a href="#" class="logo">
        <img
          class="logo-icon"
          src="@/assets/images/Logo_Module_TiengViet_White.66947422.svg"
          alt="" />
      </a>
    </div>
    <div
      v-else
      class="logo-container justify-center"
      @click="toggleSdibarShrink">
      <div class="icon icon--three-stripes"></div>
    </div>
    <div class="menu-bar">
      <div class="menu-item-list flex flex-col">
        <router-link
          v-for="(value, key) in SidebarList.top"
          :key="key"
          :to="{ name: value.name, params: {} }"
          class="menu-item"
          :class="{
            'menu-item--active': route.name === value.name,
            'justify-center': isSidebarShrink,
          }"
          :title="value.title">
          <div class="menu-item__icon">
            <div class="icon" :class="value.icon"></div>
          </div>
          <div v-show="!isSidebarShrink" class="menu-item__title">
            {{ value.text }}
          </div>
        </router-link>
        <div class="horizontal-border--white mb-2"></div>
        <router-link
          v-for="(value, key) in SidebarList.bottom"
          :key="key"
          :to="{ name: value.name, params: {} }"
          class="menu-item"
          :class="{
            'menu-item--active': route.name === value.name,
            'justify-center': isSidebarShrink,
          }"
          :title="value.title">
          <div class="menu-item__icon">
            <div class="icon" :class="value.icon"></div>
          </div>
          <div v-show="!isSidebarShrink" class="menu-item__title">
            {{ value.text }}
          </div>
        </router-link>
      </div>
    </div>
  </div>
</template>
<script>
import { computed, watch } from "vue";
import { useStore } from "vuex";
import { useRoute } from "vue-router";
import { SidebarList } from "@/resources";
export default {
  setup(props) {
    const store = useStore();
    const route = useRoute();
    watch(route, () => {
      // console.log(route);
    });
    const isSidebarShrink = computed(() => store.state.global.isSidebarShrink);

    return {
      route,
      SidebarList,
      isSidebarShrink,
      toggleSdibarShrink: () => store.dispatch("toggleSidebarShrink"),
    };
  },
};
</script>
<style>
@import url("./sidebar.css");
</style>
