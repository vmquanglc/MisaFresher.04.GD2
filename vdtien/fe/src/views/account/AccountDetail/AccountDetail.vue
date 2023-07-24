<script setup>
import {
  computed,
  ref,
  reactive,
  toRefs,
  watchEffect,
  onBeforeMount,
  onMounted,
  onBeforeUnmount,
} from "vue";
import {
  DialogAction,
  DialogType,
  MaxLength,
  PopupType,
  TypeStore,
} from "@/enums";
import { useStore } from "vuex";
import BaseComboboxV1 from "@/components/bases/BaseComboboxV1.vue";
import accountService from "@/api/services/accountService";
import { ErrValidator, AccountCol, DialogTitle } from "@/resources";
/**state */
const store = useStore();
const isExpandPopup = ref(false);
const accountDetailRef = ref(null);
const popupStatus = computed(() => store.state.global.popupStatus);
const accountDetail = computed(() => store.state.account.accountDetail);
const accountInfo = ref({});
const dialog = computed(() => store.state.global.dialog);
const errsValidate = computed(() => store.state.global.errsValidate);
const errRefs = toRefs(
  reactive({
    AccountCode: null,
    AccountName: null,
    AccountFeature: null,
  })
);
const errValidator = reactive({});
const dataAccountFeature = ref([
  {
    id: 1,
    value: "Dư nợ",
  },
  {
    id: 2,
    value: "Dư có",
  },
  {
    id: 3,
    value: "Lưỡng tính",
  },
  {
    id: 4,
    value: "Không có số dư",
  },
]);
//---start fake data combobox------

const dataAccountSynthetic = ref([]);
const dataAccountPaging = ref([]);

const fields = ref({
  AccountCode: {
    text: "Mã tài khoản",
    width: 120,
  },
  AccountName: {
    text: "Tên tài khoản",
    width: 250,
  },
});
const fieldSelect = ref("AccountId");
const fieldShow = ref("AccountCode");
const pageAccountSynthetic = ref(0);
// let searchAccountSynthetic = ref("");
const valueAccountSynthetic = ref({});
//---end fake data combobox-----
const isExpandDetailTracking = ref(true);
const toggleAttrDetailTracking = ref({
  UserObject: false,
});

const fieldsUserObject = [
  {
    field: "text",
  },
];
const fieldSelectUserObject = "value";
const fieldShowUserObject = "text";
const dataUserObject = [
  {
    value: 1,
    text: "Nhà cung cấp",
  },
  {
    value: 2,
    text: "Khách hàng",
  },
  {
    value: 3,
    text: "Nhân viên",
  },
];
/** -----------------------watch-------------------- */

/** start lifesycle */

onBeforeMount(async () => {
  store.dispatch("getErrsValidate", {});
  if (popupStatus.value.type === PopupType.update) {
    if (accountDetail.value?.ParentId) {
      try {
        let res = await accountService.getById(accountDetail.value.ParentId);
        console.log(res);
        if (res && Object.keys(res).length > 0) {
          // searchAccountSynthetic.value = res.AccountCode;
          valueAccountSynthetic.value = { ...res };
        }
      } catch (error) {
        console.log(error);
      }
    }
  }
});

onMounted(() => {
  // thêm sự kiện keydowns cho document
  accountDetailRef.value.addEventListener("keydown", handleKeyDownDocument);
});

onBeforeUnmount(() => {
  accountDetailRef.value.removeEventListener("keydown", handleKeyDownDocument);
});

watchEffect(() => [(accountInfo.value = { ...accountDetail.value })]);
/** end lifecycle */

/**------ start methods ------------------*/

/**
 * Mô tả: lazy load khi scroll tai khoan tong hop
 * created by : vdtien
 * created date: 24-07-2023
 * @param {type} param -
 * @returns
 */
const loadDataAccountSyntheticLazy = async (searchAccountSynthetic) => {
  try {
    // console.log(searchAccountSynthetic);
    store.dispatch("toggleLoading");
    // tang offset
    pageAccountSynthetic.value += 1;
    // call danh sach tai khoan tong hop
    let res = await accountService.getList({
      keySearch: searchAccountSynthetic,
      pageSize: 10,
      pageNumber: pageAccountSynthetic.value,
    });
    // de quy xep cay

    if (res?.Data?.length > 0) {
      // dataAccountPaging.value = [...res.Data];
      // let listParentId = res.Data.map((acc) => acc.AccountId);
      // await getListChildsByListParentId(listParentId);
      dataAccountSynthetic.value = [...dataAccountSynthetic.value, ...res.Data];
    }
  } catch (error) {
    console.log(error);
  } finally {
    store.dispatch("toggleLoading");
  }
};

// const getListChildsByListParentId = async (listParentId) => {
//   try {
//     let res = await accountService.getListAccountByListParentId(listParentId);
//     if (res && res.length > 0) {
//       // tien xu ly res them truong showChild
//       listParentId = [...res.map((obj) => obj.AccountId)];

//       let tmpAccoutsList = [...dataAccountPaging.value];

//       for (const account of res) {
//         const parentIndex = tmpAccoutsList.findIndex(
//           (acc) => acc.AccountId === account.ParentId
//         );
//         if (parentIndex !== -1) {
//           tmpAccoutsList.splice(parentIndex + 1, 0, account);
//         }
//       }

//       dataAccountPaging.value = [...tmpAccoutsList];
//       // de quy
//       await getListChildsByListParentId(listParentId);
//     }
//     return [...parentList];
//     return [];
//   } catch (error) {}
// };

const loadDataAccountSyntheticFilter = async (searchAccountSynthetic) => {
  try {
    store.dispatch("toggleLoading");
    // tang offset
    pageAccountSynthetic.value = 1;
    // call danh sach tai khoan tong hop
    let res = await accountService.getList({
      keySearch: searchAccountSynthetic,
      pageSize: 10,
      pageNumber: pageAccountSynthetic.value,
    });
    // de quy xep cay

    if (res?.Data) {
      dataAccountSynthetic.value = [...res?.Data];
    }
  } catch (error) {
    console.log(error);
  } finally {
    store.dispatch("toggleLoading");
  }
};

/**
 * Mô tả: bat su kien keydown cho element wrapper
 * created by : vdtien
 * created date: 20-07-2023
 * @param {type} param -
 * @returns
 */
const handleKeyDownDocument = (e) => {};

/**
 * Mô tả: đóng form
 * created by : vdtien
 * created date: 20-07-2023
 * @param {type} param -
 * @returns
 */
const onClosePopup = () => {
  store.dispatch("getPopupStatus", {
    isShowPopup: false,
    type: "",
  });
};

/**
 * validate các field xem có lỗi hay không
 * Author:vdtien(22/7/2023)
 */
const validatorAccount = () => {
  // xóa lỗi trước đó
  // Xóa tất cả các trường của reactive object
  Object.keys(errValidator).forEach((key) => {
    delete errValidator[key];
  });
  store.dispatch("getErrsValidate", {});

  // Kiểm tra các trường

  // Mã không được để trống
  let isAccountCodeEmpty = !accountInfo.value?.AccountCode?.trim();

  if (isAccountCodeEmpty) {
    errValidator.AccountCode = [
      ...(errValidator?.AccountCode ?? []),
      ErrValidator.fieldEmplty(AccountCol.AccountCode.text),
    ];
  }
  // Tên không được để trống
  let isAccountNameEmpty = !accountInfo.value?.AccountName?.trim();

  if (isAccountNameEmpty) {
    errValidator.AccountName = [
      ...(errValidator?.AccountName ?? []),
      ErrValidator.fieldEmplty(AccountCol.AccountName.text),
    ];
  }

  // tính chất tài khoản không được để trống
  let isAccountFeatureEmpty =
    !accountInfo.value?.AccountFeature ||
    isNaN(accountInfo.value?.AccountFeature);

  if (isAccountFeatureEmpty) {
    errValidator.AccountFeature = [
      ...(errValidator?.AccountFeature ?? []),
      ErrValidator.fieldEmplty(AccountCol.AccountFeature.text),
    ];
  }

  // Kiểm tra xem reactive object có rỗng hay không
  const isEmpty = Object.keys(errValidator).length === 0;
  if (isEmpty) {
    // console.log("isEmpty", isEmpty);
    return true;
  } else {
    const errMsgArray = Object.values(errValidator).flat();
    // console.log(errMsgArray);
    store.dispatch("getErrsValidate", { ...errValidator });
    store.dispatch("getDialog", {
      isShow: true,
      type: DialogType.error,
      title: DialogTitle.inValidInput,
      content: Object.values(errMsgArray),
      action: DialogAction.confirmValidate,
    });
  }
  return false;
};
/**
 * Mô tả: xử lý hành động bấm cất
 * created by : vdtien
 * created date: 22-07-2023
 * @param {type} param -
 * @returns
 */
const storeAccount = () => {
  let isValid = validatorAccount();

  if (isValid && popupStatus.value.type === PopupType.create) {
    // lưu thông tin nhân viên
    store.dispatch("createAccount", {
      account: accountInfo.value,
      typeStore: TypeStore.store,
    });
  } else if (isValid && popupStatus.value.type === PopupType.update) {
    // sửa thông tin nhân viên
    store.dispatch("updateAccount", {
      account: accountInfo.value,
      typeStore: TypeStore.store,
    });
  }
  // có lỗi thì không làm gì cả
};
/**
 * Mô tả: xử lý hành động bấm cất và thêm
 * created by : vdtien
 * created date: 22-07-2023
 * @param {type} param -
 * @returns
 */
const storeAndAddAccount = () => {
  let isValid = validatorAccount();
  if (isValid && popupStatus.value.type === PopupType.create) {
    // lưu thông tin nhân viên
    store.dispatch("createAccount", {
      account: accountInfo.value,
      typeStore: TypeStore.storeAndAdd,
    });
  } else if (isValid && popupStatus.value.type === PopupType.update) {
    // sửa thông tin nhân viên
    store.dispatch("updateAccount", {
      account: accountInfo.value,
      typeStore: TypeStore.storeAndAdd,
    });
  }
  // có lỗi thì không làm gì cả
};

const onClickSelectAttrDetailTracking = (fieldName, item) => {
  accountInfo.value[fieldName] = item[fieldSelectUserObject];
};
/** ------ end methods -----------------*/
</script>
<template>
  <div
    ref="accountDetailRef"
    class="popup-wrapper outline-none"
    :tabindex="0"
    @keydown.stop="">
    <div class="popup-container flex flex-col">
      <div class="popup__header flex items-center">
        <div class="popup-header__title">
          {{
            popupStatus?.type === PopupType.create
              ? "Thêm tài khoản"
              : "Sửa tài khoản"
          }}
        </div>
        <div class="popup-header__actions flex items-center ml-auto">
          <div class="popup-header-actions__question">
            <div class="icon-wrapper" title="HELP">
              <div class="icon icon--question"></div>
            </div>
          </div>
          <div
            ref="btnClose"
            class="popup-header-actions__close"
            title="Đóng - ESC"
            :tabindex="21"
            @keydown.enter="onClosePopup"
            @click="onClosePopup"
            @keydown="hanldeFocusFirst"
            @keydown.tab.stop="">
            <div class="icon-wrapper">
              <div class="icon icon--close"></div>
            </div>
          </div>
        </div>
      </div>
      <form id="accountForm" action="" class="popup__body flex-1 pr-2">
        <div class="form-content flex flex-col gap-24-0">
          <div class="form-content__top flex flex-col gap-16-0">
            <div class="w-full flex gap-0-8 justify-start">
              <b-textfield
                :ref="errRefs.AccountCode"
                v-model="accountInfo.AccountCode"
                required
                label="Số tài khoản"
                :max-length="MaxLength.code"
                class-label="w-1/4"
                :err-msg="errsValidate?.AccountCode?.join('') ?? ''"
                @empty-err-msg="
                  () => {
                    delete errsValidate.AccountCode;
                  }
                " />
            </div>
            <div class="w-full flex gap-0-8 justify-start">
              <b-textfield
                :ref="errRefs.AccountName"
                v-model="accountInfo.AccountName"
                required
                label="Tên tài khoản"
                :max-length="MaxLength.name"
                class-label="w-1/2"
                :err-msg="errsValidate?.AccountName?.join('') ?? ''"
                @empty-err-msg="
                  () => {
                    delete errsValidate.AccountName;
                  }
                " />
              <b-textfield
                v-model="accountInfo.AccountNameEnglish"
                label="Tên tiếng anh"
                :max-length="MaxLength.name"
                class-label="w-1/2" />
            </div>
            <div class="w-full flex justify-start gap-0-8">
              <div class="flex justify-start gap-0-8 w-1/2">
                <BaseComboboxV1
                  class="w-1/2"
                  label-title="Tài khoản tổng hợp"
                  :max-length="MaxLength.default"
                  is-reload-scroll
                  is-reload
                  :fields="fields"
                  :field-select="fieldSelect"
                  :field-show="fieldShow"
                  :data-list="dataAccountSynthetic"
                  :id-selected="accountInfo.ParentId"
                  :value-selected="valueAccountSynthetic"
                  @load-data-lazy="
                    (searchAccountSynthetic) =>
                      loadDataAccountSyntheticLazy(searchAccountSynthetic)
                  "
                  @load-data-filter="
                    (searchAccountSynthetic) =>
                      loadDataAccountSyntheticFilter(searchAccountSynthetic)
                  "
                  @on-click-id-selected="(id) => (accountInfo.ParentId = id)"
                  @keydown.tab.stop="" />
                <b-combobox
                  :ref="errRefs.AccountFeature"
                  is-required
                  class="w-1/2"
                  class-list="mh-fit"
                  label-title="Tính chất"
                  :max-length="MaxLength.default"
                  place-holder="-- Chọn tính chất --"
                  :data-list="dataAccountFeature"
                  :id-selected="accountInfo.AccountFeature"
                  :err-msg="errsValidate?.AccountFeature?.join('') ?? ''"
                  @on-click-id-selected="
                    (id) => (accountInfo.AccountFeature = id)
                  "
                  @empty-err-msg="
                    () => {
                      delete errsValidate.AccountFeature;
                    }
                  "
                  @keydown.tab.stop="" />
              </div>
            </div>
            <div class="w-full flex justify-start">
              <label class="w-full" @keydown.tab.stop="">
                Diễn giải
                <textarea class="textarea" rows="2"></textarea>
              </label>
            </div>
            <div class="w-full flex justify-start">
              <label
                class="checkbox-wrapper w-auto flex flex-row items-center pointer m-0">
                <input class="input-checkbox m-0 mr-1" type="checkbox" />
                <span> Có hạch toán kinh tế </span>
              </label>
            </div>
          </div>
          <div class="form-content__bottom flex flex-col gap-16-0">
            <div class="form-content-bottom__header pointer">
              <div
                class="flex items-center justify-start"
                @click="isExpandDetailTracking = !isExpandDetailTracking">
                <div class="flex items-center justify-center w-4 h-4">
                  <div
                    class="icon icon-down-medium transition"
                    :class="{
                      'animatation-showMoreDetail': !isExpandDetailTracking,
                    }"></div>
                </div>
                <span class="title-showMoreDetail">Theo dõi chi tiết theo</span>
              </div>
            </div>
            <div
              v-show="isExpandDetailTracking"
              class="form-content-bottom__body transition flex flex-1 flex-col gap-8-0 px-3"
              :class="{ 'hidden-detail-tracking': !isExpandDetailTracking }">
              <div class="w-full flex items-center">
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input
                        class="input-checkbox m-0 mr-1"
                        type="checkbox"
                        :checked="toggleAttrDetailTracking?.UserObject"
                        @click="
                          toggleAttrDetailTracking.UserObject =
                            !toggleAttrDetailTracking.UserObject
                        " />
                      <span class="font-regular"> Đối tượng </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="!toggleAttrDetailTracking?.UserObject"
                      :data="dataUserObject"
                      :fields="fieldsUserObject"
                      :field-select="fieldSelectUserObject"
                      :field-show="fieldShowUserObject"
                      :item-selected="pageSize"
                      @on-click-select-item="
                        (item) =>
                          onClickSelectAttrDetailTracking('UserObject', item)
                      "
                      @keydown.tab.stop="" />
                  </div>
                </div>
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Tài khoản ngân hàng </span>
                    </label>
                  </div>
                </div>
              </div>

              <div class="w-full flex items-center">
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Đối tượng THCP </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Công trình </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
              </div>

              <div class="w-full flex items-center">
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Đơn đặt hàng </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Hợp đồng bán </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
              </div>

              <div class="w-full flex items-center">
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Hợp đồng mua </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Khoản mục CP </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
              </div>

              <div class="w-full flex items-center">
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Đơn vị </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
                <div class="w-1/2 flex items-center justify-start">
                  <div class="w-5/6 flex items-center justify-start">
                    <label
                      class="checkbox-wrapper w-1/2 flex flex-row items-center pointer m-0 gap-0-8">
                      <input class="input-checkbox m-0 mr-1" type="checkbox" />
                      <span class="font-regular"> Mã thống kế </span>
                    </label>
                    <BDropdown
                      class="w-1/2"
                      :disable="true"
                      @keydown.tab.stop="" />
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </form>
      <div class="popup__bottom flex justify-between items-center mt-6">
        <div class="popup-bottom__left">
          <b-button
            ref="btnCancel"
            class="btn--sub"
            title="Hủy"
            :tab-index="20"
            @keydown.enter="onClosePopup"
            @click="onClosePopup" />
        </div>
        <div class="popup-bottom__right flex justify-center gap-0-8">
          <b-button
            ref="btnStore"
            class="btn--sub"
            title="Cất"
            :tab-index="18"
            @keydown.enter="storeAccount"
            @click="storeAccount" />
          <b-button
            ref="btnStoreAndAdd"
            class="btn--pri"
            title="Cất và thêm"
            :tab-index="19"
            @keydown.enter="storeAndAddAccount"
            @click="storeAndAddAccount" />
        </div>
      </div>
    </div>
  </div>
</template>
<style scoped>
.popup-container {
  position: fixed;
  top: 0;
  bottom: 0;
  right: 0;
  left: auto;
  width: auto;
  min-width: 900px;
  max-height: 100%;
  border-radius: var(--border-radius);
  border-top-right-radius: 0 !important;
  border-bottom-right-radius: 0 !important;
  transition: all 0.2s;
  padding: 16px;
}
.textarea {
  resize: none;
  border-radius: 3px;
  border: 1px solid #babec5;
  font-family: inherit;
  display: block;
  padding: 9px;
  color: #000;
  width: 100%;
  outline: unset;
  margin-top: 8px;
}
.textarea:hover {
  background-color: #e6e6e6;
  cursor: pointer;
}

.textarea:focus {
  border: 1px solid #50b83c;
  outline: none;
}
.animatation-showMoreDetail {
  transform: rotate(-90deg);
  transition: all linear 0.3s;
}
.transition {
  transition: all linear 0.3s;
}
.form-content-bottom__header:hover {
  color: #50b83c;
}
.form-content-bottom__header .title-showMoreDetail {
  font-size: 1rem;
}
/* .form-content-bottom__header .title-showMoreDetail:hover {
  color: #50b83c;
} */
.form-content-bottom__body {
  opacity: 1;
  animation: show-detail-tracking 0.3s linear forwards;
}
.hidden-detail-tracking {
  animation: hidden-detail-tracking 0.3s linear forwards;
}
@keyframes show-detail-tracking {
  from {
    max-height: 0px;
    opacity: 0;
  }
  to {
    opacity: 1;
    max-height: fit-content;
  }
}
@keyframes hidden-detail-tracking {
  from {
    opacity: 1;
  }
  to {
    max-height: 0px;
    opacity: 0;
  }
}
</style>
