<template lang="">
  <div
    ref="dropdowContainerRef"
    class="dropdown-wrapper"
    :class="{ 'disable-dropdown': disable }">
    <label class="w-auto">
      {{ label }}
      <span v-show="required && label" class="text-red">(*)</span>
      <div
        class="dropdown-container flex items-center"
        :class="{
          'mt-2': label,
          'border--focus': isShowDropdown,
          disabled: disable,
        }"
        @click="toggleDrodown">
        <input
          type="text"
          class="input m-0 flex-1 border-radius-none"
          :class="{}"
          :style="{ opacity: disable ? 0 : 1 }"
          readonly
          :value="valueInput" />
        <div
          class="icon-wrapper"
          :class="{ active: isShowDropdown }"
          @click="toggleDrodown">
          <div class="icon icon--down-small-black"></div>
        </div>
      </div>
    </label>
    <div
      v-if="isShowDropdown"
      class="dropdown-list w-full"
      :class="{ 'dropdown-list--up': direct === 'up' }">
      <div v-show="titleDropdownList" class="dropdown-item dropdown-item-title">
        {{ titleDropdownList }}
      </div>
      <div
        v-for="(item, index) in data"
        :key="index"
        class="dropdown-item"
        :class="item?.text === valueInput ? 'dropdown-item--selected' : ''"
        @click="onClickSelectItem(item)">
        <span v-for="(field, indexField) in fields" :key="indexField">
          {{ item?.[field.field] }}
        </span>
      </div>
    </div>
  </div>
</template>
<script>
import { ref, onMounted, watchEffect } from "vue";
import { useClickOutside } from "@/hooks";
export default {
  props: {
    label: {
      type: String,
      default: "",
    },
    required: {
      type: Boolean,
      defalut: false,
    },
    data: {
      type: Array,
      default: () => [],
    },
    fields: {
      type: Array,
      required: true,
    },
    fieldSelect: {
      type: String,
      required: true,
    },
    fieldShow: {
      type: String,
      required: true,
    },
    // eslint-disable-next-line vue/require-default-prop
    itemSelected: {
      type: Object,
      defalut: null,
    },
    titleDropdownList: {
      type: String,
      default: "",
    },
    direct: {
      type: String,
      default: "down",
    },
    disable: {
      type: Boolean,
      default: false,
    },
  },
  emits: ["onClickSelectItem"],
  setup(props, ctx) {
    const valueInput = ref(props.titleDropdownList);
    const isShowDropdown = ref(false);
    const toggleDrodown = () => {
      isShowDropdown.value = !isShowDropdown.value;
    };
    const dropdowContainerRef = ref(false);
    const isOutsideDropdown = useClickOutside(dropdowContainerRef);

    watchEffect(() => {
      if (isOutsideDropdown.value === true) {
        isShowDropdown.value = false;
      }
    });
    onMounted(() => {
      if (props.itemSelected) {
        const foundItem = props.data.find(
          (obj) =>
            obj[props.fieldSelect] === props.itemSelected[props.fieldSelect]
        );
        if (foundItem) {
          valueInput.value = foundItem[props.fieldShow];
        }
      }
    });
    const onClickSelectItem = (item) => {
      // add text cho input
      valueInput.value = item[props.fieldShow];
      // emit event
      ctx.emit("onClickSelectItem", item);

      // đóng dropdown
      toggleDrodown();
    };
    return {
      valueInput,
      isShowDropdown,
      onClickSelectItem,
      toggleDrodown,
      dropdowContainerRef,
    };
  },
};
</script>
<style scoped>
.disabled {
  background-color: #eff0f2 !important;
}
</style>
