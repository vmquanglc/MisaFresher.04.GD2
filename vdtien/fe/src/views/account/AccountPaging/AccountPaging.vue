<template>
  <b-paging
    :data-dropdown="dataDropdown"
    :fields="fields"
    :field-select="fieldSelect"
    :field-show="fieldShow"
    :page-size="filterAndPaging.pageSize"
    :page-number="filterAndPaging.pageNumber"
    :total-records="totalRecords"
    :total-pages="totalPages"
    :on-click-prev-page="onClickPrevPage"
    :on-click-next-page="onClickNextPage"
    @on-click-page-number="onClickPageNumber"
    @on-click-page-size="onClickPageSize" />
</template>
<script setup>
import { useStore } from "vuex";
import { computed, ref } from "vue";

const store = useStore();
const filterAndPaging = computed(() => store.state.global.filterAndPaging);
const totalRecords = computed(() => store.state.global.totalRecords);
const totalRoots = computed(() => store.state.global.totalRoots);
const accountsList = computed(() => store.state.account.accountsList);
// console.log(filterAndPaging.value);
const totalPages = computed(() => {
  if (filterAndPaging.value.pageSize) {
    return Math.ceil((totalRoots.value ?? 1) / filterAndPaging.value.pageSize);
  } else {
    return 1;
  }
});
const dataDropdown = ref([
  { text: "10 bản ghi trên một trang", value: 10 },
  { text: "20 bản ghi trên một trang", value: 20 },
  { text: "50 bản ghi trên một trang", value: 50 },
  { text: "100 bản ghi trên một trang", value: 100 },
]);

const fields = [
  {
    field: "text",
  },
];
const filedSelect = "value";
const fieldShow = "text";

/**
 * Mô tả: lui trang
 * created by : vdtien
 * created date: 24-06-2023
 * @param {type} param -
 * @returns
 */
const onClickPrevPage = () => {
  // nếu đang là trang 1 thì return
  if (filterAndPaging.value.pageNumber === 1) return;

  // nếu không thì lùi trang
  store.dispatch("getFilterAndPaging", {
    ...filterAndPaging.value,
    pageNumber: filterAndPaging.value.pageNumber - 1,
  });
  store.dispatch("getAccountsListTree");
};

/**
 * Mô tả: tien len 1 trang
 * created by : vdtien
 * created date: 24-06-2023
 * @param {type} param -
 * @returns
 */
const onClickNextPage = () => {
  // nếu đang là trang cuối thì return
  if (filterAndPaging.value.pageNumber >= totalPages.value) return;

  // nếu không thì lùi trang
  store.dispatch("getFilterAndPaging", {
    ...filterAndPaging.value,
    pageNumber: filterAndPaging.value.pageNumber + 1,
  });
  store.dispatch("getAccountsListTree");
};

/**
 * Mô tả: click page chi dinh
 * created by : vdtien
 * created date: 24-06-2023
 * @param {type} param -
 * @returns
 */
const onClickPageNumber = (pageNumber) => {
  store.dispatch("getFilterAndPaging", {
    ...filterAndPaging.value,
    pageNumber: pageNumber,
  });
  store.dispatch("getAccountsListTree");
};

/**
 * Mô tả: chon so ban ghi tren trang
 * created by : vdtien
 * created date: 24-06-2023
 * @param {type} param -
 * @returns
 */
const onClickPageSize = (pageSize) => {
  store.dispatch("getFilterAndPaging", {
    ...filterAndPaging.value,
    pageSize: pageSize,
    pageNumber: 1,
  });
  store.dispatch("getAccountsListTree");

  // console.log(pageSize);
};
</script>
<style></style>
