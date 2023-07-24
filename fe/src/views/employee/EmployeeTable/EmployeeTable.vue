<script>
import { PopupType, DialogType, DialogAction } from "@/enums";
import {
  DialogTitle,
  EmployeeCol,
  ButtonTitle,
  FreeText,
  DialogContent,
} from "@/resources";
import { useStore } from "vuex";
import {
  computed,
  nextTick,
  onBeforeMount,
  onMounted,
  onUpdated,
  reactive,
  ref,
  toRefs,
  watch,
  watchEffect,
} from "vue";
import {
  areAllElementsInArray,
  converGender,
  removeEmptyFields,
  convertToDDMMYYYY,
} from "@/utils/helper";
export default {
  props: {},
  setup(props) {
    const store = useStore();
    const employeeList = computed(() => store.state.employee.employeeList);
    const employeeIdListChecked = computed(
      () => store.state.employee.employeeIdListChecked
    );
    const checkAll = ref(false);

    const rowSelected = ref("");
    const tableEmployeeRef = ref(null);
    const btnTableRefs = ref([]);
    const btnTableDirectUp = ref(false);
    watchEffect(() => {
      let employeIdList = ref(
        employeeList.value.map((item) => item.EmployeeId)
      );
      if (
        areAllElementsInArray(
          employeIdList.value,
          employeeIdListChecked.value
        ) &&
        employeeIdListChecked.value?.length > 0
      ) {
        checkAll.value = true;
      } else {
        checkAll.value = false;
      }
      // Đánh lại ref khi danh sách items thay đổi
      btnTableRefs.value = employeeList.value.map(() => ref(null));
      // console.log(employeeIdListChecked.value);
    });
    onBeforeMount(() => {
      store.dispatch("getEmployeeList");
    });
    onMounted(() => {
      btnTableRefs.value = employeeList.value.map(() => ref(null));
    });

    /**
     * toggle more action ở cột chức năng của table
     * Author: vdtien (27/5/2023)
     */
    const toggleTableAction = (item, index) => {
      // console.log(item);
      if (!item) {
        rowSelected.value = "";
        btnTableDirectUp.value = false;
        return;
      }
      if (rowSelected.value !== item.EmployeeId) {
        rowSelected.value = item.EmployeeId;
        const tablePositionBottom =
          tableEmployeeRef.value.getBoundingClientRect().bottom;
        const btnTableActionPositionBottom =
          btnTableRefs.value[index].value[0].getBoundingClientRect().bottom;
        if (tablePositionBottom - btnTableActionPositionBottom <= 100) {
          btnTableDirectUp.value = true;
        }
      } else {
        rowSelected.value = "";
        btnTableDirectUp.value = false;
      }
      // console.log(rowSelected);
    };
    /**
     * mở popup cập nhật nhân viên
     * Author:vdtien (28/5/2023)
     */
    const onOpenPopupUpdate = (item) => {
      const itemRemoveNull = removeEmptyFields(item);
      store.dispatch("getEmployeeDetail", itemRemoveNull);
      store.dispatch("getPopupStatus", {
        isShowPopup: true,
        type: PopupType.update,
      });
    };

    /**
     *
     * @param {*} item
     * open employee dialog cảnh báo xóa
     * Author: vdtien(28/5/2023)
     */
    const onClickDeleteEmployee = (item) => {
      if (employeeIdListChecked.value.includes(item.EmployeeId)) {
        onClickCheckRecord([item.EmployeeId]);
      }
      // console.log(item);
      toggleTableAction(item);
      store.dispatch("getEmployeeDetail", item);
      store.dispatch("getDialog", {
        isShow: true,
        type: DialogType.warning,
        title: DialogTitle.delete,
        content: [`${DialogContent.confirmDeleteEmployee(item?.EmployeeCode)}`],
        action: DialogAction.confirmDelete,
      });
    };

    /**
     *
     * @param {*} item
     * check or uncheck list employee
     * Author: vdtien(28/5/2023)
     */
    const onClickCheckAllRecord = () => {
      let employeIdList = employeeList.value.map((item) => item.EmployeeId);

      store.dispatch("getEmployeeIdListCkecked", employeIdList);
    };

    /**
     *
     * @param {*} item
     * check or uncheck 1 employee
     * Author: vdtien(28/5/2023)
     */
    const onClickCheckRecord = (itemId) => {
      store.dispatch("getEmployeeIdListCkecked", itemId);
    };

    /**
     *
     * @param {*} item
     * nhan ban nhan vien theo nhan vien duoc click
     * Author: vdtien(28/5/2023)
     */
    const onClickCloneRecord = async (item) => {
      // console.log(item);
      toggleTableAction(item);
      const itemRemoveNull = removeEmptyFields({ ...item });
      delete itemRemoveNull.EmployeeId;
      store.dispatch("getEmployeeDetail", itemRemoveNull);
      await store.dispatch("getNewEmployeecode");
      store.dispatch("getPopupStatus", {
        isShowPopup: true,
        type: PopupType.create,
      });
    };
    return {
      EmployeeCol,
      ButtonTitle,
      FreeText,
      employeeList,
      toggleTableAction,
      rowSelected,
      checkAll,
      onOpenPopupUpdate,
      onClickDeleteEmployee,
      tableEmployeeRef,
      btnTableRefs,
      btnTableDirectUp,
      converGender,
      employeeIdListChecked,
      onClickCheckAllRecord,
      onClickCheckRecord,
      convertToDDMMYYYY,
      onClickCloneRecord,
    };
  },
};
</script>

<template>
  <div ref="tableEmployeeRef" class="table-wrapper">
    <div class="table-container">
      <table>
        <thead>
          <tr>
            <th
              v-for="(col, key, index) in EmployeeCol"
              :key="index"
              :class="{
                'th-anchor': key === 'checkbox',
                'th-anchor th-anchor--end': key === 'action',
                'text-center': key === 'DateOfBirth',
              }"
              :style="{ minWidth: key === 'checkbox' ? '40px' : '160px' }"
              :title="col?.title">
              <input
                v-if="key === 'checkbox'"
                type="checkbox"
                style="width: 24px; height: 24px"
                :checked="checkAll"
                @click="onClickCheckAllRecord" />
              <span v-else>{{ col?.text }}</span>
            </th>
          </tr>
        </thead>
        <tbody>
          <template v-if="employeeList?.length > 0">
            <tr
              v-for="(item, index) in employeeList"
              :key="item.EmployeeId"
              class="pointer"
              :class="{
                'tr--checked': employeeIdListChecked.includes(item.EmployeeId),
              }"
              @dblclick="() => onOpenPopupUpdate(item)">
              <td
                v-for="(col, key, indexCol) in EmployeeCol"
                :key="indexCol"
                :class="{
                  'td-anchor td-anchor--start': key === 'checkbox',
                  'td-anchor td-anchor--end td-action': key === 'action',
                  'text-center':
                    key === 'DateOfBirth' || key === 'IdentityDateRelease',
                  'before-last':
                    indexCol === Object.keys(EmployeeCol).length - 2,
                }"
                :style="{
                  'z-index':
                    rowSelected === item.EmployeeId && key === 'action' ? 1 : 0,
                }"
                @click="() => onClickCheckRecord([item.EmployeeId])">
                <input
                  v-if="key === 'checkbox'"
                  type="checkbox"
                  style="width: 24px; height: 24px"
                  :checked="employeeIdListChecked.includes(item.EmployeeId)"
                  @dblclick.stop=""
                  @click.stop=""
                  @click="() => onClickCheckRecord([item.EmployeeId])" />
                <span v-else-if="key === 'Gender'">
                  {{ converGender(item[key]) }}
                </span>
                <span v-else-if="key == 'DateOfBirth'">
                  {{ convertToDDMMYYYY(item[key]) }}
                </span>
                <span v-else-if="key == 'IdentityDateRelease'">
                  {{ convertToDDMMYYYY(item[key]) }}
                </span>
                <span v-else-if="key !== 'action'">{{ item[key] }}</span>
                <div
                  v-else
                  class="flex items-center justify-center h-full"
                  @dblclick.stop=""
                  @click.stop="">
                  <a
                    href="#"
                    class="text-blue font-semi-bold h-full flex items-center px-4"
                    @click="() => onOpenPopupUpdate(item)"
                    >{{ ButtonTitle.edit }}</a
                  >

                  <div
                    class="td-action__icon"
                    :style="{
                      'z-index':
                        rowSelected === item.EmployeeId && key === 'action'
                          ? 2
                          : 0,
                    }">
                    <div
                      :ref="btnTableRefs[index]"
                      class="icon-wrapper w-8 h-8"
                      :class="{
                        'border--blue': rowSelected === item.EmployeeId,
                      }"
                      @click="() => toggleTableAction(item, index)">
                      <div class="icon icon--down-small-blue"></div>
                    </div>

                    <div
                      v-if="rowSelected === item.EmployeeId"
                      class="dropdown-list td-action-list"
                      :class="{ 'td-action-list--up': btnTableDirectUp }">
                      <div
                        class="dropdown-item td-action-item td-action-item--remove"
                        @click="() => onClickDeleteEmployee(item)">
                        {{ ButtonTitle.delete }}
                      </div>
                      <div
                        class="dropdown-item td-action-item"
                        @click="() => onClickCloneRecord(item)">
                        {{ ButtonTitle.duplicate }}
                      </div>
                    </div>
                  </div>
                  <div
                    v-if="rowSelected"
                    class="overlay"
                    @click="() => toggleTableAction()" />
                </div>
              </td>
            </tr>
          </template>
          <template v-else>
            <tr>
              <td
                :colspan="Object.keys(EmployeeCol).length"
                class="font-italic">
                {{ FreeText.notFoundRecord }}
              </td>
            </tr>
          </template>
        </tbody>
      </table>
    </div>
  </div>
</template>
<style></style>
