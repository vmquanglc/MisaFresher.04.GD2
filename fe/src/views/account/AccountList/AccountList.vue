<template>
  <div class="flex flex-col main-wrapper w-full h-full">
    <div class="content__header flex justify-between items-center pt-6 pb-6">
      <div class="content-header__title">
        <h3 class="font-24 mb-3">{{ FreeText.accountSystem }}</h3>
        <div class="back-to-general">
          <router-link
            :to="{ name: 'Directory', params: {} }"
            class="text-blue flex items-center gap-0-4">
            <div class="icon icon--chevron-left-blue"></div>
            <span>Tất cả danh mục</span>
          </router-link>
        </div>
      </div>
    </div>
    <div class="content__body flex flex-col flex-1 height-body">
      <div class="content-body__tools flex justify-between items-center mb-4">
        <div class="content-body-tools__left">
          <b-textfield
            v-model="employeeSerch"
            class-input="m-0"
            :place-holder="InputPlaceholder.findAccount"
            class-icon="icon icon--search" />
        </div>
        <div
          class="content-body-tools__right ml-auto flex items-center gap-0-8">
          <div class="text-blue pointer" @click="toggleExpand">
            {{ !isExpand ? "Mở rộng" : "Thu gọn" }}
          </div>
          <div
            class="icon-wrapper content-body-tools__refresh"
            title="Tải lại"
            @click="onClickRefreshPage">
            <div class="icon icon--refresh"></div>
          </div>
          <div
            class="icon-wrapper content-body-tools__export-excel"
            title="Xuất file"
            @click="exportExcelEmployeeList">
            <div class="icon icon--excel"></div>
          </div>
          <b-button
            :button-type="ButtonType.combo"
            :title="'Thêm'"
            :on-click="onOpenPopupCreate">
          </b-button>
        </div>
      </div>
      <div class="content-body__table flex-1">
        <!-- {{ accountsList }} -->
        <AccountTable />
      </div>
      <div class="content-body__paging">
        <AccountPaging />
      </div>
    </div>
  </div>
  <AccountDetail v-if="popupStatus.isShowPopup" />
  <AccountDialog v-if="dialog?.isShow" />
  <b-toast-message v-if="toast?.isShow" />
  <b-loading v-if="isLoading" />
</template>
<script setup>
import { useStore } from "vuex";
import { computed, onBeforeMount, ref } from "vue";

import AccountTable from "../AccountTable/AccountTable.vue";
import AccountPaging from "../AccountPaging/AccountPaging.vue";
import AccountDetail from "../AccountDetail/AccountDetail.vue";
import AccountDialog from "../AccountDialog/AccountDialog.vue";
import { FreeText, InputPlaceholder } from "@/resources";
import { ButtonType, PopupType } from "@/enums";

//---------------start state-----------------
const store = useStore();
const accountsList = computed(() => store.state.account.accountsList);
const popupStatus = computed(() => store.state.global.popupStatus);
const isLoading = computed(() => store.state.global.isLoading);
const toast = computed(() => store.state.global.toast);
const dialog = computed(() => store.state.global.dialog);
const isExpand = ref(false);
const isLoadedAll = ref(false);
//-------------end state-----------------

//-----------------start method------------------------

/**
 * Mô tả: mo form them moi tai khoan
 * created by : vdtien
 * created date: 20-07-2023
 * @param {type} param -
 * @returns
 */
const onOpenPopupCreate = () => {
  store.dispatch("getAccountDetail", {});
  store.dispatch("getPopupStatus", {
    isShowPopup: true,
    type: PopupType.create,
  });
};

/**
 * Mô tả: toggle table hien thi het cac cap hay chi hien thi nut goc
 * created by : vdtien
 * created date: 20-07-2023
 * @param {type} param -
 * @returns
 */
const toggleExpand = async () => {
  if (isExpand.value === false) {
    if (isLoadedAll.value === false) {
      // chua goi lan nao
      // goi cac node goc
      await store.dispatch("getAccountsListTree");
      let listParentId = accountsList.value.map((obj) => obj.AccountId);
      // goi cac node con lai
      await store.dispatch(
        "getListAccountChildrenByListParentId",
        listParentId
      );
      isLoadedAll.value = true;
    }
    // goi it nhat 1 lan roi
    // cap nhat lai cac thuoc tinh showChild = true
    store.dispatch("toggleShowAll", true);
  } else {
    // cap nhat lai cac thuoc tinh showChild = false
    store.dispatch("toggleShowAll", false);
  }
  isExpand.value = !isExpand.value;
};
//---------------end method------------------
</script>
<style scoped>
.height-body {
  height: calc(100% - 112px);
}
</style>
