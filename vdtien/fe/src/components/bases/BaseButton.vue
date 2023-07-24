<template>
  <div
    v-if="buttonType === ButtonType.combo"
    ref="btn"
    class="btn--combobox"
    @keydown.tab.stop="">
    <div class="btn--combobox__button btn btn--pri" @click="onClick">
      {{ title }}
    </div>
    <div class="vertical-border"></div>
    <div class="btn--combobox__icon icon-wrapper">
      <div class="icon icon--chevron-left-medium-white"></div>
    </div>
  </div>
  <div
    v-else
    ref="btn"
    class="btn"
    :tabindex="tabIndex"
    @keydown.tab.stop=""
    @click="onClick">
    {{ title }}
  </div>
  <!-- sử dụng ít nêu không cho tái sử dụng nữa -->
  <!-- <div v-if="buttonType === ButtonType.pri">
  
  </div>
  <div
    v-else-if="buttonType === ButtonType.icon"
    class="btn-icon flex items-center justify-center">
    <div class="icon icon--plus"></div>
  </div>
  <div v-else-if="buttonType === ButtonType.combo" class="btn--combobox">
    <div class="btn--combobox__button btn btn--pri">{{ title }}</div>
    <div class="vertical-border"></div>
    <div class="btn--combobox__icon icon-wrapper">
      <div class="icon icon--chevron-left-medium-white"></div>
    </div>
  </div>
  <div v-else-if="buttonType === ButtonType.wIcon" class="btn-w-icon">
    <div class="icon-wrapper">
      <div class="icon icon--warning-medium"></div>
    </div>
    <span class="text">{{ title }}</span>
  </div> -->
</template>
<script>
import { ref } from "vue";
import { ButtonType } from "@/enums";
export default {
  props: {
    title: {
      type: String,
      default: "",
    },
    tabIndex: {
      type: Number,
      default: 0,
    },
    buttonType: {
      type: String,
      default: "primary",
    },
    onClick: {
      type: Function,
      default: () => {},
    },
  },
  setup(props) {
    const btn = ref(null);

    const focus = () => {
      btn.value.focus();
    };
    return {
      btn,
      focus,
      ButtonType,
    };
  },
};
</script>
<style lang=""></style>
