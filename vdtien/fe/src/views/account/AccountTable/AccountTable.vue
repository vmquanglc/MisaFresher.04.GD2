<template>
  <div ref="tableAccountRef" class="table-wrapper">
    <div class="table-container">
      <table>
        <thead>
          <tr>
            <th
              v-for="(col, key, index) in AccountCol"
              :key="index"
              :class="{
                'th-anchor th-anchor--end': key === 'action',
              }"
              :style="{ minWidth: '160px' }"
              :title="col?.title">
              <span>{{ col?.text }}</span>
            </th>
          </tr>
        </thead>
        <tbody>
          <template v-if="accountsList?.length > 0">
            <template v-for="item in accountsList" :key="item.AccountId">
              <tr v-if="isshowTr(item.ParentId)" class="pointer">
                <td
                  v-for="(col, key, indexCol) in AccountCol"
                  :key="indexCol"
                  :class="{
                    'td-anchor td-anchor--end td-action': key === 'action',
                    'before-last':
                      indexCol === Object.keys(AccountCol).length - 2,
                    'font-bold': item?.IsParent,
                  }"
                  :style="{
                    'z-index':
                      rowSelected === item?.AccountId && key === 'action'
                        ? 1
                        : 0,
                  }">
                  <span
                    v-if="key == 'AccountCode'"
                    class="flex items-center gap-0-8"
                    :style="{ 'padding-left': `${24 * item?.Grade ?? 0}px` }">
                    <div class="w-4 h-4 flex items-center justify-center">
                      <div
                        v-show="item?.IsParent"
                        class="icon"
                        :class="{
                          'icon--plus-square': !item?.showChild,
                          'icon--minus-square': item?.showChild,
                        }"
                        @click="() => onClickToggleShowChild(item)"></div>
                      <div v-show="!item?.IsParent" class="icon"></div>
                    </div>
                    <span>{{ item[key] }}</span>
                  </span>
                  <span v-else-if="key === 'AccountFeature'">
                    {{ converAccountFeature(item[key]) }}</span
                  >
                  <span v-else-if="key === 'Status'">
                    {{ converStatusField(item[key]) }}
                  </span>

                  <div
                    v-else-if="key === 'action'"
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
                          rowSelected === item.AccountId && key === 'action'
                            ? 2
                            : 0,
                      }">
                      <div
                        class="icon-wrapper w-8 h-8"
                        :class="{
                          'border--blue': rowSelected === item.AccountId,
                        }"
                        @click="() => toggleTableAction(item, index)">
                        <div class="icon icon--down-small-blue"></div>
                      </div>

                      <div
                        v-if="rowSelected === item.AccountId"
                        class="dropdown-list td-action-list"
                        :class="{ 'td-action-list--up': btnTableDirectUp }">
                        <div
                          class="dropdown-item td-action-item td-action-item--remove"
                          @click="() => onClickDeleteAccount(item)">
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
                  <span v-else> {{ item[key] }}</span>
                </td>
              </tr>
            </template>
          </template>
          <template v-else>
            <tr>
              <td :colspan="Object.keys(AccountCol).length" class="font-italic">
                {{ FreeText.notFoundRecord }}
              </td>
            </tr>
          </template>
        </tbody>
      </table>
    </div>
  </div>
</template>
<script setup>
import TrTree from "./TrTree.vue";
import {
  AccountCol,
  FreeText,
  ButtonTitle,
  DialogTitle,
  DialogContent,
} from "@/resources";
import { useStore } from "vuex";
import { computed, onBeforeMount, onMounted, ref, watchEffect } from "vue";
import {
  converAccountFeature,
  converStatusField,
  removeEmptyFields,
} from "@/utils/helper";
import { DialogAction, DialogType, PopupType } from "@/enums";

const store = useStore();
const accountsList = computed(() => store.state.account.accountsList);
const rowSelected = ref("");
const tableAccountRef = ref(null);
const positionTableBottom = ref(0);
const btnTableDirectUp = ref(false);
onBeforeMount(() => {
  store.dispatch("getAccountsListTree");
});
onMounted(() => {
  positionTableBottom.value =
    tableAccountRef.value.getBoundingClientRect().bottom;
  console.log(positionTableBottom.value);
});

/**
 * toggle more action ở cột chức năng của table
 * Author: vdtien (27/5/2023)
 */
const toggleTableAction = (item) => {
  // console.log(item);
  console.log(event.clientY);
  if (!item) {
    rowSelected.value = "";
    btnTableDirectUp.value = false;
    return;
  }
  if (rowSelected.value !== item.AccountId) {
    rowSelected.value = item.AccountId;
    if (positionTableBottom.value - event.clientY < 100) {
      btnTableDirectUp.value = true;
    } else {
      btnTableDirectUp.value = false;
    }
  } else {
    rowSelected.value = "";
    // btnTableDirectUp.value = false;
  }
};

// kiểm tra điều kiện hiển thị của 1 tr
const isshowTr = (ParentId) => {
  // console.log("run is show");
  // kiem tra cha cos dong mo khong
  let parent = accountsList.value.find((obj) => obj.AccountId === ParentId);
  // console.log(parent?.AccountId);
  if (parent) {
    if (parent?.showChild === true) return isshowTr(parent?.ParentId);
    else {
      return false;
    }
  } else {
    return true;
  }
};

/**
 * Mô tả: mo rong hoac thu gon
 * created by : vdtien
 * created date: 22-07-2023
 * @param {type} param -
 * @returns
 */
const onClickToggleShowChild = (item) => {
  if (item.showChild === false) {
    // kiem tra xem no co con khong
    const child = accountsList.value.find(
      (obj) => obj.ParentId === item.AccountId
    );

    // console.log(child?.AccountId);
    if (child) {
      // neu co con roi thi khong can load api
      store.dispatch("toggleShowChild", item.AccountId);
    } else {
      store.dispatch("getAccountsListByParentId", item.AccountId);
    }
  } else {
    store.dispatch("toggleShowChild", item.AccountId);
  }
};

/**
 * Mô tả: xoa 1 ban gho
 * created by : vdtien
 * created date: 22-07-2023
 * @param {type} param -
 * @returns
 */
const onClickDeleteAccount = (item) => {
  console.log(item);
  toggleTableAction(item);
  store.dispatch("getAccountDetail", item);
  store.dispatch("getDialog", {
    isShow: true,
    type: DialogType.warning,
    title: DialogTitle.delete,
    content: [`${DialogContent.confirmDeleteAccount(item?.AccountCode)}`],
    action: DialogAction.confirmDelete,
  });
};

/**
 * mở popup cập nhật nhân viên
 * Author:vdtien (28/5/2023)
 */
const onOpenPopupUpdate = (item) => {
  const itemRemoveNull = removeEmptyFields(item);
  store.dispatch("getAccountDetail", itemRemoveNull);
  store.dispatch("getPopupStatus", {
    isShowPopup: true,
    type: PopupType.update,
  });
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
  delete itemRemoveNull.AccountId;
  delete itemRemoveNull.ParentId;
  store.dispatch("getAccountDetail", itemRemoveNull);
  store.dispatch("getPopupStatus", {
    isShowPopup: true,
    type: PopupType.create,
  });
};
</script>
<style></style>
