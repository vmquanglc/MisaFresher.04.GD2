<template lang="">
  <div class="pagination flex-0">
    <div
      class="pagination-container flex justify-between items-center px-4 py-2 w-full">
      <div class="pagination__left flex items-center justify-center">
        <div class="pagination-left__total-record">
          Tổng số:
          <b class="">{{ totalRecords }}</b>
          bản ghi
        </div>
      </div>
      <div class="pagination__right flex items-center justify-center">
        <div class="record-in-page">
          <b-dropdown
            class="record-in-page-dropdown"
            :data="dataDropdown"
            :fields="fields"
            :field-select="fieldSelect"
            :field-show="fieldShow"
            :item-selected="pageSize"
            title-dropdown-list="--Chọn số bản ghi / trang--"
            direct="up"
            @on-click-select-item="onClickPageSize" />
        </div>
        <div class="list-number-page flex items-center">
          <div
            class="pointer pagination__prev mr-3"
            :class="{ 'no-click-page': pageNumber === 1 }"
            @click="onClickPrevPage">
            Trước
          </div>
          <div v-if="totalPages <= 7" class="page-list flex items-center">
            <div
              v-for="(item, index) in totalPages"
              :key="index"
              class="page-item sd"
              :class="{ 'page-item--current': index + 1 === pageNumber }"
              @click="() => onClickPageNumber(index + 1)">
              {{ index + 1 }}
            </div>
          </div>
          <div v-else class="page-list flex items-center">
            <div
              class="page-item"
              :class="{ 'page-item--current': pageNumber === 1 }"
              @click="() => onClickPageNumber(1)">
              1
            </div>
            <div v-show="pageNumber > 4" class="page-item no-pointer">...</div>

            <div v-if="pageNumber <= 4" class="flex items-center">
              <div
                class="page-item"
                :class="{ 'page-item--current': pageNumber === 2 }"
                @click="() => onClickPageNumber(2)">
                2
              </div>
              <div
                class="page-item"
                :class="{ 'page-item--current': pageNumber === 3 }"
                @click="() => onClickPageNumber(3)">
                3
              </div>
              <div
                class="page-item"
                :class="{ 'page-item--current': pageNumber === 4 }"
                @click="() => onClickPageNumber(4)">
                4
              </div>
              <div class="page-item" @click="() => onClickPageNumber(5)">5</div>
            </div>
            <div
              v-else-if="pageNumber >= totalPages - 3"
              class="flex items-center">
              <div
                class="page-item"
                @click="() => onClickPageNumber(totalPages - 4)">
                {{ totalPages - 4 }}
              </div>
              <div
                class="page-item"
                :class="{ 'page-item--current': pageNumber === totalPages - 3 }"
                @click="() => onClickPageNumber(totalPages - 3)">
                {{ totalPages - 3 }}
              </div>
              <div
                class="page-item"
                :class="{ 'page-item--current': pageNumber === totalPages - 2 }"
                @click="() => onClickPageNumber(totalPages - 2)">
                {{ totalPages - 2 }}
              </div>
              <div
                class="page-item"
                :class="{ 'page-item--current': pageNumber === totalPages - 1 }"
                @click="() => onClickPageNumber(totalPages - 1)">
                {{ totalPages - 1 }}
              </div>
            </div>
            <div v-else class="flex items-center">
              <div
                class="page-item"
                @click="() => onClickPageNumber(pageNumber - 1)">
                {{ pageNumber - 1 }}
              </div>
              <div
                class="page-item page-item--current"
                @click="() => onClickPageNumber(pageNumber)">
                {{ pageNumber }}
              </div>
              <div
                class="page-item"
                @click="() => onClickPageNumber(pageNumber + 1)">
                {{ pageNumber + 1 }}
              </div>
            </div>

            <div
              v-show="pageNumber < totalPages - 3"
              class="page-item no-pointer">
              ...
            </div>
            <div
              class="page-item"
              :class="{ 'page-item--current': pageNumber === totalPages }"
              @click="() => onClickPageNumber(totalPages)">
              {{ totalPages }}
            </div>
          </div>

          <div
            class="pointer pagination__prev ml-3"
            :class="{ 'no-click-page': pageNumber >= totalPages }"
            @click="onClickNextPage">
            Sau
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import { computed, onBeforeMount, ref } from "vue";
export default {
  props: {
    dataDropdown: {
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
    pageSize: {
      type: Number,
      default: 10,
    },
    pageNumber: {
      type: Number,
      default: 1,
    },
    totalRecords: {
      type: Number,
      default: 1,
    },
    totalPages: {
      type: Number,
      default: 1,
    },
    onClickPrevPage: {
      type: Function,
      default: () => {},
    },
    onClickNextPage: {
      type: Function,
      default: () => {},
    },
  },
  emits: ["onClickPageSize", "onClickPageNumber"],
  setup(props, ctx) {
    const itemSelect = ref(null);
    onBeforeMount(() => {
      itemSelect.value = props.dataDropdown.find(
        (item) => item[props.fieldSelect] === props.pageSize
      );
    });
    const onClickPageNumber = (item) => {
      ctx.emit("onClickPageNumber", item);
    };
    const onClickPageSize = (item) => {
      ctx.emit("onClickPageSize", item.value);
    };
    return {
      onClickPageNumber,
      onClickPageSize,
    };
  },
};
</script>
<style>
.record-in-page-dropdown {
  width: 240px;
}
</style>
