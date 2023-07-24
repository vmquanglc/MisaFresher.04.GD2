<template lang="">
  <div ref="comboboxRef" class="combobox">
    <label class="combobox-wrapper block m-0 w-full">
      {{ labelTitle }}
      <span v-show="isRequired" class="text-red">(*)</span>

      <div
        class="combobox-container flex items-center"
        :class="{
          'mt-2': labelTitle,
          'border--focus': isShowCombobox,
          'border--red': errMsg,
        }">
        <input
          ref="inputRef"
          type="text"
          class="input combobox-input flex-1 m-0"
          :title="errMsg"
          :tabindex="tabIndex"
          :placeholder="placeHolder"
          :value="searchValue"
          @input="handleChangeInput"
          @keydown="handleKeyDown" />
        <div
          class="icon-wrapper combobox-icon"
          :class="{ active: isShowCombobox }"
          @click="toggleCombobox">
          <div class="icon icon--chevron-down"></div>
        </div>
      </div>
    </label>
    <div
      v-show="isShowCombobox"
      class="combobox-list-wrapper"
      style="z-index: 1">
      <ul class="combobox-list" :class="classList">
        <li
          v-for="(item, index) in dataListFilter"
          :ref="itemRefs[index]"
          :key="index"
          class="combobox-item"
          :class="{
            'combobox-item--selected': idSelected === item.id,
            'combobox-item--hover': selectedIndex === index,
          }"
          @click="() => onClickComboboxItem(item, index)">
          {{ item.value }}
        </li>
      </ul>
    </div>
  </div>
</template>
<script>
import {
  ref,
  watch,
  computed,
  onMounted,
  watchEffect,
  onBeforeMount,
} from "vue";
import { useClickOutside, useDebounce } from "@/hooks";
import { removeDiacritics } from "@/utils/helper";
export default {
  props: {
    labelTitle: {
      type: String,
      default: "",
    },
    tabIndex: {
      type: Number,
      default: -1,
    },
    placeHolder: {
      type: String,
      default: "",
    },
    dataList: {
      type: Array,
      default: () => [],
    },
    direct: {
      type: String,
      default: "show",
    },
    classList: {
      type: String,
      default: "",
    },
    isRequired: {
      type: Boolean,
      default: false,
    },
    idSelected: {
      type: String,
      default: "",
    },
    errMsg: {
      type: String,
      default: "",
    },
    isReload: {
      type: Boolean,
      default: false,
    },
  },
  emits: [
    "update:modelValue",
    "onClickIdSelected",
    "emptyErrMsg",
    "addValueSelected",
  ],

  setup(props, { emit }) {
    const inputRef = ref(null);
    const comboboxRef = ref(null);
    const isShowCombobox = ref(false);
    const searchValue = ref("");
    const debounceSearch = useDebounce(searchValue, 500);
    const isOutsideCombobox = useClickOutside(comboboxRef);
    // console.log(props.dataList);
    const dataListFilter = ref([]);
    const itemRefs = ref([]);
    const itemSelected = ref({});
    const selectedIndex = ref(
      computed(() =>
        props.dataList.findIndex((item) => item.id === props.idSelected)
      )?.value ?? -1
    );

    //kiểm tra sự thay đổi của debounceSearch
    watch(debounceSearch, () => {
      if (props.isReload) {
        // emit ra cha để call api
      } else {
        // filter ở client

        dataListFilter.value = props.dataList.filter((item) =>
          removeDiacritics(item.value?.toLowerCase()).includes(
            removeDiacritics(debounceSearch.value.toLowerCase())
          )
        );
      }
      selectedIndex.value = -1;
    });
    // cập nhật dataList
    watchEffect(() => {
      // console.log("watch effect");
      dataListFilter.value = props.dataList;
    });

    watchEffect(() => {});

    watchEffect(() => {
      // if (dataListFilter.value.length > 0) {
      //   selectedIndex.value = 0;
      // } else {
      // }
      selectedIndex.value = -1;
      itemRefs.value = dataListFilter.value.map(() => ref(null));
    });
    //kiểm tra sự thay đổi của idSelected
    watchEffect(() => {
      if (props.idSelected) {
        const foundItem = props.dataList.find(
          (obj) => obj.id === props.idSelected
        );
        if (foundItem) {
          itemSelected.value = { ...foundItem };
          searchValue.value = foundItem.value;
        }
      } else {
        itemSelected.value = {};
        searchValue.value = "";
      }
    });

    watchEffect(() => {
      if (selectedIndex.value != -1) {
        if (itemRefs.value[selectedIndex.value].value) {
          itemRefs.value[selectedIndex.value].value[0].scrollIntoView();
        }
      }
    });

    watchEffect(() => {
      if (isOutsideCombobox.value === true) {
        isShowCombobox.value = false;
      }
    });
    onMounted(() => {
      itemRefs.value = dataListFilter.value.map(() => ref(null));
    });

    // focus input
    const focus = () => {
      inputRef.value.focus();
    };
    // ẩn hiện combolist khi click chuột
    const toggleCombobox = () => {
      isShowCombobox.value = !isShowCombobox.value;
      selectedIndex.value = -1;
    };

    // chọn item được selected
    const onClickComboboxItem = (item) => {
      itemSelected.value = item;
      searchValue.value = item.value;
      // emit idSelected ra cho component cha
      emit("onClickIdSelected", item.id);
      emit("addValueSelected", item.value);
      emit("emptyErrMsg");

      // console.log(item);
      // đóng combox-list
      isShowCombobox.value = false;
      selectedIndex.value = -1; //
    };

    // bắt sự kiện khi change input
    const handleChangeInput = (e) => {
      // if(e.key === "ArrowUp" ||e.key === "ArrowDown" )
      isShowCombobox.value = true;
      searchValue.value = e.target.value;
      // emit("update:modelValue", e.target.value);
      emit("emptyErrMsg");
    };

    // bắt sự kiện lên xuống enter tab
    const handleKeyDown = (e) => {
      if (e.key === "ArrowUp") {
        e.preventDefault();
        isShowCombobox.value = true;

        if (selectedIndex.value > 0) {
          selectedIndex.value--;
        } else if (selectedIndex.value === 0) {
          selectedIndex.value = dataListFilter.value.length - 1;
        }
      } else if (e.key === "ArrowDown") {
        e.preventDefault();
        isShowCombobox.value = true;

        if (selectedIndex.value < dataListFilter.value.length - 1) {
          selectedIndex.value++;
        } else if (selectedIndex.value === dataListFilter.value.length - 1) {
          selectedIndex.value = 0;
        }
      } else if (e.key === "Enter") {
        e.preventDefault();
        if (isShowCombobox.value) {
          // nếu đã di chuyển phím để chọn item
          if (selectedIndex.value != -1) {
            const itemSelect = dataListFilter.value[selectedIndex.value];
            if (itemSelect) {
              onClickComboboxItem(itemSelect);
            }
          } else {
            // nếu không chọn thì trả về item selected trước đó nếu có
            if (itemSelected.value) {
              searchValue.value = itemSelected.value.value;
            } else {
              searchValue.value = "";
            }

            isShowCombobox.value = !isShowCombobox.value;
          }
        } else {
          isShowCombobox.value = !isShowCombobox.value;
        }
      } else if (e.key === "Tab") {
        // tìm value input theo idSelected trước đó
        // nếu không chọn thì trả về item selected trước đó
        if (itemSelected.value.value) {
          searchValue.value = itemSelected.value.value;
        }
        isShowCombobox.value = false;
        selectedIndex.value = -1;
      }
    };

    return {
      inputRef,
      isShowCombobox,
      selectedIndex,
      toggleCombobox,
      onClickComboboxItem,
      handleChangeInput,
      handleKeyDown,
      dataListFilter,
      searchValue,
      debounceSearch,
      itemSelected,
      itemRefs,
      comboboxRef,
      focus,
    };
  },
};
</script>
<style></style>
