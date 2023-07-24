<template>
  <b-dialog
    :type="dialog.type"
    :title="dialog.title"
    :content="dialog.content"
    :on-close-all="onCloseAll"
    :on-close="onClose"
    :on-accept="onAccept" />
</template>
<script>
import { useStore } from "vuex";
import { computed } from "vue";
import { DialogAction } from "@/enums";

export default {
  setup(props) {
    const store = useStore();
    const dialog = computed(() => store.state.global.dialog);
    const employeeDetail = computed(() => store.state.employee.employeeDetail);
    // console.log(employeeDetail.value);

    /**
     * Mô tả: Đóng dialog và popup
     * created by : vdtien
     * created date: 04-06-2023
     * @param {type} param -
     * @returns
     */
    const onClose = () => {
      store.dispatch("getDialog", {});
    };

    /**
     * Mô tả: thực hiện create or update
     * created by : vdtien
     * created date: 04-06-2023
     * @param {type} param -
     * @returns
     */
    const onAccept = () => {
      switch (dialog.value.action) {
        case DialogAction.confirmCreate:
          store.dispatch("getDialog", {
            action: DialogAction.confirmCreate,
          });
          break;
        case DialogAction.confirmUpdate:
          store.dispatch("getDialog", {
            action: DialogAction.confirmUpdate,
          });
          break;
        case DialogAction.confirmDelete:
          store.dispatch("getDialog", {});
          store.dispatch("deleteEmployee", employeeDetail.value);
          break;
        case DialogAction.confirmDeleteMulti:
          store.dispatch("getDialog", {});
          store.dispatch("deleteMultiEmployee");
        default:
          store.dispatch("getDialog", {});
          break;
      }
    };

    /**
     * Mô tả: đóng dialog
     * created by : vdtien
     * created date: 04-06-2023
     * @param {type} param -
     * @returns
     */
    const onCloseAll = () => {
      store.dispatch("getDialog", {});
      store.dispatch("getPopupStatus");
      store.dispatch("getEmployeeDetail");
    };
    return { dialog, onClose, onAccept, onCloseAll, employeeDetail };
  },
};
</script>
<style></style>
