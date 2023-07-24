<template lang="">
  <div class="toast-message">
    <div class="toast__icon icon-wrapper">
      <div
        class="icon"
        :class="{
          'icon--success': toast?.type === ToastType.success,
          'icon--warning': toast?.type === ToastType.warning,
          'icon--error': toast?.type === ToastType.error,
        }"></div>
    </div>
    <div class="toast__content">
      <span
        v-show="toast?.type === ToastType.success"
        class="toast__title toast__title--success"
        >Thành công!</span
      >
      <span
        v-show="toast?.type === ToastType.warning"
        class="toast__title toast__title--warning"
        >Cảnh báo!</span
      >
      <span
        v-show="toast?.type === ToastType.error"
        class="toast__title toast__title--error"
        >Lỗi</span
      >
      <span class="content">{{ toast?.content }}</span>
    </div>
    <div class="toast__close icon icon--close-small" @click="closeToast"></div>
  </div>
</template>
<script>
import { useStore } from "vuex";
import { ToastType } from "@/enums";
import { computed, onMounted } from "vue";
export default {
  setup(props) {
    const store = useStore();
    const toast = computed(() => store.state.global.toast);
    onMounted(() => {
      setTimeout(() => {
        store.dispatch("getToast");
      }, 3000);
    });
    const closeToast = () => {
      store.dispatch("getToast");
    };
    return {
      ToastType,
      toast,
      closeToast,
    };
  },
};
</script>
<style lang=""></style>
