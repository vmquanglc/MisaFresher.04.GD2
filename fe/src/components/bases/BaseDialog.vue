<template>
  <div ref="dialog" tabindex="0" class="dialog-wrapper" @keydown.stop="">
    <div class="dialog-container flex flex-col">
      <div class="dialog__header flex items-center justify-between">
        <div class="dialog-header__title">{{ title }}</div>
        <div class="dialog-header__close">
          <div ref="close" class="icon-wrapper" @click="onClose">
            <div class="icon icon--close-small"></div>
          </div>
        </div>
      </div>
      <div class="dialog__content flex justify-start items-center gap-0-16">
        <div
          class="dialog-content__icon flex items-center justify-center w-12 h-12">
          <div
            class="icon"
            :class="{
              'icon--warning-big': type === DialogType.warning,
              'icon--info-big': type === DialogType.info,
              'icon--error-big': type === DialogType.error,
            }"></div>
        </div>
        <div class="dialog-content__text">
          <ul
            :class="{ 'pl-4': content?.length ?? 0 > 1 }"
            :style="{
              'list-style': content && content.length <= 1 ? 'none' : '',
            }">
            <li v-for="(item, index) in content" :key="index">{{ item }}</li>
          </ul>
        </div>
      </div>
      <div
        class="dialog__bottom flex items-center mt-6"
        :class="{
          'justify-between':
            type === DialogType.warning || type === DialogType.info,
          'justify-center': type === DialogType.error,
        }">
        <template v-if="type === DialogType.warning">
          <b-button
            ref="closeWarning"
            class="btn--sub"
            :tab-index="102"
            title="Không"
            @keydown.tab.stop.prevent="acceptWarning.focus()"
            @keydown.enter="onClose"
            @click="onClose" />
          <b-button
            ref="acceptWarning"
            v-auto-focus
            class="btn--pri"
            :tab-index="101"
            title="Có"
            @keydown.tab.stop=""
            @keydown.shift.tab.prevent="closeWarning.focus()"
            @keydown.enter="onAccept"
            @click="onAccept" />
        </template>
        <template v-if="type === DialogType.error">
          <b-button
            ref="closeError"
            v-auto-focus
            class="btn--pri"
            title="Đóng"
            :tab-index="101"
            @keydown.tab.stop.prevent=""
            @keydown.enter="onClose"
            @click="onClose" />
        </template>
        <template v-if="type === DialogType.info">
          <div class="popup-botoom__left">
            <b-button
              ref="closeInfo"
              class="btn btn--sub"
              title="Hủy"
              :tab-index="103"
              @keydown.tab.stop.prevent="acceptInfo.focus()"
              @keydown.shift.tab.prevent="closeAllInfo.focus()"
              @keydown.enter="onClose"
              @click="onClose" />
          </div>
          <div class="popup-bottom__right flex items-center">
            <b-button
              ref="closeAllInfo"
              class="btn--sub"
              title="Không"
              :tab-index="102"
              @keydown.tab.stop=""
              @keydown.enter="onCloseAll"
              @click="onCloseAll" />
            <b-button
              ref="acceptInfo"
              v-auto-focus
              :tab-index="101"
              class="btn--pri"
              title="Có"
              @keydown.tab.stop=""
              @keydown.shift.tab.stop.prevent="closeInfo.focus()"
              @keydown.enter="onAccept"
              @click="onAccept" />
          </div>
        </template>
      </div>
    </div>
  </div>
</template>
<script>
import { onBeforeMount, onMounted, ref } from "vue";
import { DialogType } from "@/enums";

export default {
  props: {
    // isShow: {
    //   type: Boolean,
    //   default: false,
    // },
    type: {
      type: String,
      default: "",
    },
    title: {
      type: String,
      default: "",
    },
    content: {
      type: Array,
      default: () => [],
    },
    onCloseAll: {
      type: Function,
      default: () => {},
    },
    onClose: {
      type: Function,
      default: () => {},
    },
    onAccept: {
      type: Function,
      default: () => {},
    },
  },
  setup(props) {
    const dialog = ref(null);
    const close = ref(null);
    const closeWarning = ref(null);
    const acceptWarning = ref(null);
    const closeError = ref(null);
    const closeInfo = ref(null);
    const closeAllInfo = ref(null);
    const acceptInfo = ref(null);
    onMounted(() => {
      // thêm sự kiện keydowns cho document
      dialog.value.addEventListener("keydown", handleKeydownDialog);
    });
    /**
     * Mô tả: bat su kien keydown cua dialog
     * created by : vdtien
     * created date: 29-06-2023
     * @param {type} param -
     * @returns
     */
    const handleKeydownDialog = (e) => {
      if (e.keyCode === 9) {
        e.preventDefault();
        if (acceptInfo.value) {
          acceptInfo.value.focus();
        } else if (closeError.value) {
          closeError.value.focus();
        } else if (acceptWarning.value) {
          acceptWarning.value.focus();
        }
      } else if (e.keyCode === 27) {
        props.onClose();
      }
    };

    return {
      DialogType,
      dialog,
      handleKeydownDialog,
      close,
      closeWarning,
      acceptWarning,
      closeError,
      closeInfo,
      closeAllInfo,
      acceptInfo,
    };
  },
};
</script>
<style></style>
