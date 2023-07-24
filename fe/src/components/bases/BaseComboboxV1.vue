<template>
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
      <div ref="listDataRef" class="combobox-list-container">
        <table>
          <thead>
            <tr>
              <th
                v-for="(col, key, index) in props.fields"
                :key="index"
                :style="{
                  minWidth: `${col?.width}px`,
                  maxWidth: `${col?.width}px`,
                }"
                :title="col?.title">
                <span>{{ col?.text }}</span>
              </th>
            </tr>
          </thead>
        </table>
      </div>
      <div ref="listDataWrapperRef" class="combobox-list-container tbody">
        <table>
          <tbody>
            <template v-if="dataListFilter?.length > 0">
              <tr
                v-for="(item, index) in dataListFilter"
                :ref="itemRefs[index]"
                :key="index"
                class="combobox-item"
                :class="{
                  'combobox-item--selected':
                    idSelected === item[props.fieldSelect],
                  'combobox-item--hover': selectedIndex === index,
                }"
                @click="() => onClickComboboxItem(item, index)">
                <td
                  v-for="(col, key, indexCol) in props.fields"
                  :key="indexCol"
                  :style="{
                    minWidth: `${col?.width}px`,
                    maxWidth: `${col?.width}px`,
                  }"
                  :class="{
                    'font-bold': item?.IsParent,
                  }"
                  :title="col?.title">
                  <span
                    v-if="key == props.fieldShow"
                    :style="{ 'padding-left': `${24 * item?.Grade ?? 0}px` }">
                    {{ item[key] }}</span
                  >
                  <span v-else>{{ item[key] }}</span>
                </td>
              </tr>
            </template>
          </tbody>
        </table>
      </div>

      <!-- <ul ref="listDataRef" class="combobox-list">
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
      </ul> -->
    </div>
  </div>
</template>
<script setup>
import {
  ref,
  watch,
  computed,
  onMounted,
  watchEffect,
  onBeforeMount,
  onBeforeUnmount,
  onBeforeUpdate,
} from "vue";
import { useClickOutside, useDebounce } from "@/hooks";
import { removeDiacritics } from "@/utils/helper";

//---start props-----
const props = defineProps({
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
  fields: {
    type: Object,
    default: new Object(),
    required: true,
  },
  fieldSelect: {
    type: String,
    default: "",
    required: true,
  },
  fieldShow: {
    type: String,
    default: "",
    required: true,
  },
  idSelected: {
    // field selected
    type: String,
    default: "",
  },
  textSelect: {
    type: String,
    default: "",
  },
  valueSelected: {
    type: Object,
    default: new Object(),
  },
  dataList: {
    type: Array,
    default: () => [],
    required: true,
  },
  direct: {
    type: String,
    default: "show",
  },
  isRequired: {
    type: Boolean,
    default: false,
  },

  errMsg: {
    type: String,
    default: "",
  },
  isReload: {
    type: Boolean,
    default: false,
  },
  isReloadScroll: {
    type: Boolean,
    default: false,
  },
});
//---end props------

// --start emits----
const emit = defineEmits([
  "update:modelValue",
  "onClickIdSelected",
  "emptyErrMsg",
  "addValueSelected",
  "loadDataLazy",
  "loadDataFilter",
]);
//---end emits--------

//-------start state-----------
const inputRef = ref(null);
const comboboxRef = ref(null);
const listDataRef = ref(null);
const listDataWrapperRef = ref(null);
const isShowCombobox = ref(false);
const isLoading = ref(false);
const searchValue = ref("");
const debounceSearch = useDebounce(searchValue, 500);
const isOutsideCombobox = useClickOutside(comboboxRef);
// console.log(props.dataList);
const dataListFilter = ref([]);
const itemRefs = ref([]);
const itemSelected = ref({});
const selectedIndex = ref(
  computed(() =>
    props.dataList.findIndex(
      (item) => item[props.fieldSelect] === props.idSelected
    )
  )?.value ?? -1
);
//----------end state

//-------lifecycle

onBeforeMount(() => {
  isLoading.value = true;
  emit("loadDataLazy");
});

onMounted(() => {
  itemRefs.value = dataListFilter.value.map(() => ref(null));
  if (props.isReloadScroll) {
    listDataWrapperRef.value.addEventListener("scroll", handleScrollList);
  }
});

onBeforeUpdate(() => {});

onBeforeUnmount(() => {
  listDataWrapperRef.value.removeEventListener("scroll", handleScrollList);
});

//kiểm tra sự thay đổi của debounceSearch
watch(debounceSearch, () => {
  if (props.isReload && !isLoading.value) {
    // emit ra cha để call api
    isLoading.value = true;
    // console.log("emit");
    listDataWrapperRef.value.scrollTop = 0;
    emit("loadDataFilter", debounceSearch.value);
  } else {
    // filter ở client

    dataListFilter.value = props.dataList.filter((item) =>
      removeDiacritics(`${item[props.fieldShow]}`.toLowerCase()).includes(
        removeDiacritics(debounceSearch.value.toLowerCase())
      )
    );
  }
  selectedIndex.value = -1;
});
// cập nhật dataList
watchEffect(() => {
  dataListFilter.value = props.dataList;
});

// props.dataList thay doi thi cap nhat lai loading de co the emit load data
watchEffect(() => {
  if (props.dataList) {
    isLoading.value = false;
  }
});

watch(isShowCombobox, () => {
  if (isShowCombobox.value === false) {
    if (itemSelected.value && Object.keys(itemSelected.value).length > 0) {
      selectedIndex.value = dataListFilter.value.findIndex(
        (obj) =>
          obj[props.fieldSelect] === itemSelected.value[props.fieldSelect]
      );
      if (selectedIndex.value > -1 && itemRefs.value[selectedIndex.value]) {
        // itemRefs.value[selectedIndex.value].value[0].scrollIntoView();
        // console.log(selectedIndex.value);
        listDataWrapperRef.value.scrollTop = selectedIndex.value * 47.5;
        // console.log(listDataWrapperRef.value.scrollTop);
        handleScrollList();
      }
    } else {
      listDataWrapperRef.value.scrollTop = 0;
    }
  }
});

watchEffect(() => {
  itemRefs.value = dataListFilter.value.map(() => ref(null));
});
//kiểm tra sự thay đổi của idSelected
watch(props.idSelected, () => {
  if (props.idSelected !== "") {
    console.log("run id selected");
    const foundItem = props.dataList.find(
      (obj) => obj[props.fieldSelect] === props.idSelected
    );
    if (foundItem) {
      itemSelected.value = { ...foundItem };
      searchValue.value = foundItem[props.fieldShow];
    } else {
      searchValue.value = props.valueSelected[props.fieldShow];
    }
  }
});

watchEffect(() => {
  if (isOutsideCombobox.value === true) {
    isShowCombobox.value = false;
  }
});

// ---------end lifecycle

//----------start medthos
// focus input
const handleScrollList = async () => {
  // console.log("scrol...");
  const clientHeight = listDataWrapperRef.value.clientHeight;
  const scrollHeight = listDataWrapperRef.value.scrollHeight;
  const maxScroll = scrollHeight - clientHeight;
  const scrollTop = listDataWrapperRef.value.scrollTop;
  // console.log(
  //   "scroll:",
  //   clientHeight,
  //   scrollHeight,
  //   maxScroll,
  //   scrollTop,
  //   isShowCombobox.value
  // );
  // Khi người dùng cuộn đến cuối trang, tải thêm dữ liệu
  if (
    maxScroll - scrollTop < 48 &&
    clientHeight !== scrollHeight &&
    !isLoading.value
  ) {
    isLoading.value = true;
    emit("loadDataLazy", debounceSearch.value);
  }
};

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
  searchValue.value = item[props.fieldShow];
  isLoading.value = true;
  // emit idSelected ra cho component cha
  emit("onClickIdSelected", item[props.fieldSelect]);
  // emit("addValueSelected", item[props.fieldShow]);
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
  console.log(searchValue.value);
  isLoading.value = false;
  itemSelected.value = {};
  selectedIndex.value = -1;
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
      listDataWrapperRef.value.scrollTop -= 48;
    } else if (selectedIndex.value === 0) {
      selectedIndex.value = dataListFilter.value.length - 1;
      listDataWrapperRef.value.scrollTop =
        listDataWrapperRef.value.scrollHeight -
        listDataWrapperRef.value.clientHeight;
    }
  } else if (e.key === "ArrowDown") {
    e.preventDefault();
    isShowCombobox.value = true;
    if (selectedIndex.value < dataListFilter.value.length - 1) {
      selectedIndex.value++;
      if (selectedIndex.value > 0) {
        listDataWrapperRef.value.scrollTop += 48;
      }
    } else if (selectedIndex.value === dataListFilter.value.length - 1) {
      selectedIndex.value = 0;
      listDataWrapperRef.value.scrollTop = 0;
    }
    // scrollDow();
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
          searchValue.value = itemSelected.value[props.fieldShow];
        } else {
          searchValue.value = props.valueSelected[props.fieldShow];
        }

        isShowCombobox.value = !isShowCombobox.value;
      }
    } else {
      isShowCombobox.value = !isShowCombobox.value;
    }
  } else if (e.key === "Tab") {
    // tìm value input theo idSelected trước đó
    // nếu không chọn thì trả về item selected trước đó
    if (itemSelected.value[props.fieldShow]) {
      searchValue.value = itemSelected.value[props.fieldShow];
    }
    isShowCombobox.value = false;
    selectedIndex.value = -1;
  }
};

//-------------end methods
</script>
<style scoped>
.combobox-list-wrapper {
  width: unset;
}
th,
td {
  border: unset !important;
}
/* table {
  max-height: 100px;
} */
.tbody {
  max-height: 200px;
  height: 200px;
  overflow: auto;
}
tr:hover > td {
  background-color: unset;
}
</style>
