<template>
  <div class="flex flex-col main-wrapper w-full h-full">
    <div class="content__header flex justify-between items-center pt-6 pb-6">
      <div class="content-header__title">
        <h3 class="font-24">{{ FreeText.employee }}</h3>
      </div>
    </div>
    <div class="content__body flex flex-col flex-1">
      <div class="content-body__tools flex justify-between items-center mb-4">
        <div class="content-body-tools__left">
          <div
            class="batch-actions flex items-center pointer"
            :class="{ 'no-pointer': employeeIdListChecked?.length === 0 }">
            <div
              ref="actionMultiRef"
              class="flex items-center px-2 w-full h-full"
              @click="toggleActionMulti">
              <div class="batch-actions__title">
                {{ ButtonTitle.multiAction }}
              </div>
              <div class="batch-actions__icon">
                <div class="icon icon--down-small-black"></div>
              </div>
            </div>
            <div
              v-show="isShowActionMulti"
              class="dropdown-list batch-actions__list">
              <div class="dropdown-item" @click="onClickDeleteMulti">
                {{ ButtonTitle.delete }}
              </div>
            </div>
          </div>
        </div>
        <div class="content-body-tools__right ml-auto flex items-center">
          <b-textfield
            v-model="employeeSerch"
            class-input="m-0"
            :place-holder="InputPlaceholder.findEmployee"
            class-icon="icon icon--search" />

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
            :style="{ borderRadius: '30px' }"
            :button-type="ButtonType.combo"
            :title="'Thêm'"
            :on-click="onOpenPopupCreate">
          </b-button>
        </div>
      </div>
      <div class="content-body__table flex-1">
        <EmployeeTable />
      </div>
      <div class="content-body__paging">
        <EmployeePaging />
      </div>
    </div>
  </div>
  <EmployeeDetail v-if="popupStatus.isShowPopup" />
  <EmployeeDialog v-if="dialog.isShow" />
  <b-toast-message v-if="toast?.isShow" />
  <b-loading v-if="isLoading" />
</template>
<script>
import { computed, ref, onMounted, onUpdated, watchEffect, watch } from "vue";
import { useStore } from "vuex";
import {
  ButtonType,
  ToastType,
  PopupType,
  DialogAction,
  DialogType,
} from "@/enums";
import EmployeeTable from "../EmployeeTable/EmployeeTable.vue";
import EmployeePaging from "../EmployeePaging/EmployeePaging.vue";
import EmployeeDetail from "../EmployeeDetail/EmployeeDetail.vue";
import EmployeeDialog from "../EmployeeDialog/EmployeeDialog.vue";
import { useClickOutside, useDebounce } from "@/hooks";
import {
  DialogTitle,
  ButtonTitle,
  FreeText,
  InputPlaceholder,
  DialogContent,
} from "@/resources";

export default {
  components: {
    EmployeePaging,
    EmployeeTable,
    EmployeeDetail,
    EmployeeDialog,
  },
  setup(props) {
    const store = useStore();
    const popupStatus = computed(() => store.state.global.popupStatus);
    const isLoading = computed(() => store.state.global.isLoading);
    const toast = computed(() => store.state.global.toast);
    const dialog = computed(() => store.state.global.dialog);
    const filterAndPaging = computed(() => store.state.global.filterAndPaging);
    const employeeIdListChecked = computed(
      () => store.state.employee.employeeIdListChecked
    );
    const employeeSerch = ref("");
    const debounceEmployeeSearch = useDebounce(employeeSerch, 600);
    const isShowActionMulti = ref(false);
    const actionMultiRef = ref(null);
    const isOutsideActionMulti = useClickOutside(actionMultiRef);
    // kiểm tra sự thay đổi của keySearch tìm kiếm nhân viên
    watch(debounceEmployeeSearch, () => {
      store.dispatch("getFilterAndPaging", {
        ...filterAndPaging.value,
        keySearch: debounceEmployeeSearch,
        pageNumber: 1,
      });
      store.dispatch("getEmployeeIdListCkecked");
      store.dispatch("getEmployeeList");
    });
    watchEffect(() => {
      if (employeeIdListChecked.value?.length === 0) {
        isShowActionMulti.value = false;
      }
    });
    watchEffect(() => {
      console.log();
      if (isOutsideActionMulti.value === true) {
        isShowActionMulti.value = false;
      }
    });

    // onMounted(async () => {
    //   await store.dispatch("getEmployeeList");
    // });
    onUpdated(() => {
      // console.log(dialog);
    });

    /**
     * Mô tả: xử lý sự kiện bấm nút thêm mới nhân viên
     * created by : vdtien
     * created date: 05-06-2023
     * @param {type} param -
     * @returns
     */
    const onOpenPopupCreate = async () => {
      store.dispatch("getEmployeeDetail", {});
      await store.dispatch("getNewEmployeecode");
      store.dispatch("getPopupStatus", {
        isShowPopup: true,
        type: PopupType.create,
      });
    };

    /**
     * Mô tả: toogle thuc hien hang loat
     * created by : vdtien
     * created date: 24-06-2023
     * @param {type} param -
     * @returns
     */
    const toggleActionMulti = () => {
      isShowActionMulti.value = !isShowActionMulti.value;
    };

    /**
     * Mô tả: click vao xoa nieu
     * created by : vdtien
     * created date: 24-06-2023
     * @param {type} param -
     * @returns
     */
    const onClickDeleteMulti = () => {
      toggleActionMulti();
      store.dispatch("getDialog", {
        isShow: true,
        type: DialogType.warning,
        title: DialogTitle.delete,
        content: [
          DialogContent.confirmDeleteMultiEmployee(
            employeeIdListChecked.value?.length ?? 0
          ),
        ],
        action: DialogAction.confirmDeleteMulti,
      });
    };

    /**
     * Mô tả: click vao refresh
     * created by : vdtien
     * created date: 24-06-2023
     * @param {type} param -
     * @returns
     */
    const onClickRefreshPage = () => {
      store.dispatch("getFilterAndPaging", {
        pageSize: 10,
        pageNumber: 1,
        keySearch: "",
      });
      store.dispatch("getEmployeeList");
    };

    /**
     * Mô tả: export excel danh sach nhan vien theo key search
     * created by : vdtien
     * created date: 30-06-2023
     * @param {type} param -
     * @returns
     */
    const exportExcelEmployeeList = () => {
      store.dispatch("exportExcelEmployeeList");
    };

    return {
      ButtonType,
      employeeSerch,
      isLoading,
      toast,
      onOpenPopupCreate,
      popupStatus,
      dialog,
      isShowActionMulti,
      employeeIdListChecked,
      toggleActionMulti,
      onClickDeleteMulti,
      onClickRefreshPage,
      actionMultiRef,
      exportExcelEmployeeList,
      ButtonTitle,
      FreeText,
      InputPlaceholder,
    };
  },
};
</script>
<style>
@import url(./EmployeeList.css);
</style>
