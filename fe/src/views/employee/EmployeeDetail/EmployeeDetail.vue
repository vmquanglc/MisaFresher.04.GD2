<script>
import {
  computed,
  onMounted,
  onUpdated,
  reactive,
  ref,
  toRefs,
  onBeforeUnmount,
  watch,
  onBeforeMount,
  watchEffect,
} from "vue";
import { useStore } from "vuex";

import { useDebounce } from "@/hooks";
import {
  ButtonType,
  PopupType,
  DialogType,
  DialogAction,
  Gender,
  TypeStore,
  MaxLength,
} from "@/enums";
import {
  DialogTitle,
  ErrValidator,
  EmployeeCol,
  DialogContent,
} from "@/resources";
import {
  containsOnlyNumbers,
  removeDiacritics,
  removeEmptyFields,
} from "@/utils/helper";
import DepartmentService from "@/api/services/departmentService";

export default {
  setup(props) {
    const store = useStore();
    const popupStatus = computed(() => store.state.global.popupStatus);
    // console.log(popupStatus);
    const employeeDetail = computed(() => store.state.employee.employeeDetail);
    const employeeDialog = computed(() => store.state.global.dialog);
    const employeeInfo = ref({ ...employeeDetail.value });
    const dataDeaprtment = ref([]);
    const dataDepartmentFilter = ref([]);

    const errRefs = toRefs(
      reactive({
        EmployeeCode: null,
        EmployeeName: null,
        DepartmentId: null,
        DateOfBirth: null,
        IdentityNumber: null,
        IdentityDateRelease: null,
        Email: null,
      })
    );
    // console.log(errRefs.EmployeeCode);
    const btnClose = ref(null);
    const btnCancel = ref(null);
    const btnStore = ref(null);
    const btnStoreAndAdd = ref(null);
    const tmpTablIndex = ref(null);
    const errValidator = reactive({});
    const errsValidate = computed(() => store.state.global.errsValidate);
    const employeeDetailRef = ref(null);

    // bắt sự thay đổi của dialog
    watch(employeeDialog, (newValue, oldValue) => {
      // ???
      // const isEmpty = Object.keys(employeeDialog.value).length === 0;
      if (
        oldValue.type === DialogType.error &&
        oldValue.action === DialogAction.confirmValidate
      ) {
        // Lấy phần tử đầu tiên của danh sách
        // console.log(errsValidate.value);
        const firstKey = Object.keys(errsValidate.value)[0];
        // console.log("firstKey:", firstKey);
        if (firstKey) {
          const firstErr = accessRef(firstKey);
          firstErr.value.focus();
        }
      } else if (
        newValue.action === DialogAction.confirmCreate &&
        oldValue.action === DialogAction.confirmCreate
      ) {
        storeEmployee();
      } else if (
        newValue.action === DialogAction.confirmUpdate &&
        oldValue.action === DialogAction.confirmUpdate
      ) {
        storeEmployee();
      } else {
        errRefs.EmployeeCode.value.focus();
      }
    });

    // cập nhật employeeInfo
    watchEffect(() => {
      employeeInfo.value = { ...employeeDetail.value };
    });

    onBeforeMount(async () => {
      // call api lấy danh sách phòng ban
      try {
        let response = await DepartmentService.getAll();

        // console.log(response);
        if (response) {
          dataDeaprtment.value = response.map((item, index) => ({
            id: item.DepartmentId,
            value: item.DepartmentName,
          }));
        }
      } catch (error) {
        console.log(error);
      }
      // set lai errvalidate
      store.dispatch("getErrsValidate", {});
    });
    onMounted(() => {
      // employeeInfo.value.DepartmentId = "2";
      errRefs.EmployeeCode.value.focus();

      // thêm sự kiện keydowns cho document
      employeeDetailRef.value.addEventListener(
        "keydown",
        handleKeyDownDocument
      );
    });
    onBeforeUnmount(() => {
      employeeDetailRef.value.removeEventListener(
        "keydown",
        handleKeyDownDocument
      );
    });

    // Truy cập vào ref dựa trên tên chuỗi
    const accessRef = (refName) => {
      return errRefs[refName] ? errRefs[refName] : null;
    };

    /**
     * Đóng popup
     * Author: vdtien (28/5/2023)
     */
    const onClosePopup = () => {
      // kiểm tra có sự thay đổi không
      employeeInfo.value = removeEmptyFields(employeeInfo.value);
      if (
        JSON.stringify(employeeInfo.value) !==
        JSON.stringify(employeeDetail.value)
      ) {
        if (popupStatus.value.type === PopupType.create) {
          store.dispatch("getDialog", {
            isShow: true,
            type: DialogType.info,
            title: DialogTitle.store,
            content: [DialogContent.confirmStoreEmployee],
            action: DialogAction.confirmCreate,
          });
        } else if (popupStatus.value.type === PopupType.update) {
          store.dispatch("getDialog", {
            isShow: true,
            type: DialogType.info,
            title: DialogTitle.store,
            content: [DialogContent.confirmStoreEmployee],
            action: DialogAction.confirmUpdate,
          });
        } else {
          store.dispatch("getPopupStatus", {
            isShowPopup: false,
            type: "",
          });
          store.dispatch("getEmployeeDetail");
        }
      } else {
        store.dispatch("getPopupStatus", {
          isShowPopup: false,
          type: "",
        });
        store.dispatch("getEmployeeDetail");
      }
    };

    /**
     *
     * @param {*} e
     * shift tab quay lại btnClose
     * Author: vdtien (28/5/2023)
     */
    const hanldeFocusLast = (e) => {
      // console.log(e.key);
      if (e.shiftKey && e.key === "Tab") {
        e.preventDefault();
        btnClose.value.focus();
      }
    };
    /**
     *
     * @param {*} e
     * tab nhảy về ô employeeCode
     * shift tab nhảy về btnCancel
     * Author: vdtien (28/5/2023)
     */
    const hanldeFocusFirst = (e) => {
      if (!e.shiftKey && e.which == 9) {
        e.preventDefault();
        errRefs.EmployeeCode.value.focus();
      }
      if (e.shiftKey && e.which == 9) {
        e.preventDefault();
        // btnCancel.value.$el.nextElementSibling.focus();
        btnCancel.value.focus();
      }
    };

    /**
     * Mô tả: bắt sự kiện keypress cho document khi open popup
     * created by : vdtien
     * created date: 30-05-2023
     */
    const handleKeyDownDocument = (event) => {
      // console.log("key pressed:", event.key);
      if (event.key === "Tab") {
        event.preventDefault();
        errRefs.EmployeeCode.value.focus();
        // tmpTablIndex.value.focus();
      } else if (event.key === "Escape") {
        onClosePopup();
      } else if (event.ctrlKey && event.keyCode === 83) {
        event.preventDefault();
        storeEmployee();
      }
    };

    /**
     * validate các field xem có lỗi hay không
     * Author:vdtien(28/5/2023)
     */
    const validatorEmployee = () => {
      // xóa lỗi trước đó
      // Xóa tất cả các trường của reactive object
      Object.keys(errValidator).forEach((key) => {
        delete errValidator[key];
      });
      store.dispatch("getErrsValidate", {});

      // Kiểm tra các trường

      // Mã không được để trống
      let isEmployeeCodeEmpty = !employeeInfo.value?.EmployeeCode?.trim();

      if (isEmployeeCodeEmpty) {
        errValidator.EmployeeCode = [
          ...(errValidator?.EmployeeCode ?? []),
          ErrValidator.employeeCodeEmppty,
        ];
      } else {
        // Mã đúng định dạng [NV-]<Mã số>
        let employeeCodeFormat = /^(NV-)(\d+)$/;
        if (!employeeInfo.value.EmployeeCode.match(employeeCodeFormat)) {
          errValidator.EmployeeCode = [
            ...(errValidator?.EmployeeCode ?? []),
            ErrValidator.employeeCodeForamt,
          ];
        }
      }

      // Tên không được để trống
      let isEmployeeNameEmpty = !employeeInfo.value?.FullName?.trim();

      if (isEmployeeNameEmpty) {
        errValidator.EmployeeName = [
          ...(errValidator?.EmployeeName ?? []),
          ErrValidator.employeeNameEmpty,
        ];
      }

      // Đơn vị không được để trống
      let isDepartmentEmpty = !employeeInfo.value?.DepartmentId?.trim();

      if (isDepartmentEmpty) {
        errValidator.DepartmentId = [
          ...(errValidator?.DepartmentId ?? []),
          ErrValidator.departmentEmpty,
        ];
      }

      // Ngày sinh không lớn hơn ngày hiện tại
      if (employeeInfo.value?.DateOfBirth) {
        let currentDate = new Date().getTime();
        let dof = Date.parse(employeeInfo.value.DateOfBirth);
        if (dof > currentDate) {
          errValidator.DateOfBirth = [
            ...(errValidator?.dateOfBirth ?? []),
            ErrValidator.dateOfBirth,
          ];
        }
      } else {
        delete employeeInfo.value.DateOfBirth;
      }

      // số cmnd chỉ chứa số và tối đa 25 ký tự
      if (employeeInfo.value?.IdentityNumber) {
        if (!containsOnlyNumbers(employeeInfo.value.IdentityNumber)) {
          errValidator.IdentityNumber = [
            ...(errValidator?.IdentityNumber ?? []),
            ErrValidator.containsOnlyNumber(EmployeeCol.IdentityNumber.text),
          ];
        }
      }

      // Ngày cấp không lớn hơn ngày hiện tại
      if (employeeInfo.value?.IdentityDateRelease) {
        let currentDate = new Date().getTime();
        let dof = Date.parse(employeeInfo.value.IdentityDateRelease);
        if (dof > currentDate) {
          errValidator.IdentityDateRelease = [
            ...(errValidator?.IdentityDateRelease ?? []),
            ErrValidator.identityDateRelease,
          ];
        }
      } else {
        delete employeeInfo.value.IdentityDateRelease;
      }

      // Email đúng định dạng
      if (employeeInfo.value?.Email) {
        let emailFormat =
          /^([a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+)@([a-zA-Z0-9-]+)((\.[a-zA-Z0-9-]{2,3})+)$/;
        if (!employeeInfo.value.Email.match(emailFormat)) {
          errValidator.Email = [
            ...(errValidator?.Email ?? []),
            ErrValidator.email,
          ];
        }
      }
      // // so dien thoai chi chua so
      // if (employeeInfo?.value?.PhoneNumber) {
      //   if (!containsOnlyNumbers(employeeInfo.value.PhoneNumber)) {
      //     errValidator.PhoneNumber = [
      //       ...(errValidator?.PhoneNumber ?? []),
      //       ErrValidator.containsOnlyNumber(EmployeeCol.PhoneNumber.text),
      //     ];
      //   }
      // }

      // // so dien thoai chi chua so
      // if (employeeInfo?.value?.MobilePhoneNumber) {
      //   if (!containsOnlyNumbers(employeeInfo.value.MobilePhoneNumber)) {
      //     errValidator.MobilePhoneNumber = [
      //       ...(errValidator?.MobilePhoneNumber ?? []),
      //       ErrValidator.containsOnlyNumber(EmployeeCol.MobilePhoneNumber.text),
      //     ];
      //   }
      // }

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
     * created date: 05-06-2023
     * @param {type} param -
     * @returns
     */
    const storeEmployee = () => {
      console.log("something");
      let isValid = validatorEmployee();

      if (isValid && popupStatus.value.type === PopupType.create) {
        // lưu thông tin nhân viên
        store.dispatch("createEmployee", {
          employee: employeeInfo.value,
          typeStore: TypeStore.store,
        });
      } else if (isValid && popupStatus.value.type === PopupType.update) {
        // sửa thông tin nhân viên
        store.dispatch("updateEmployee", {
          employee: employeeInfo.value,
          typeStore: TypeStore.store,
        });
      }
      // có lỗi thì không làm gì cả
    };
    /**
     * Mô tả: xử lý hành động bấm cất và thêm
     * created by : vdtien
     * created date: 05-06-2023
     * @param {type} param -
     * @returns
     */
    const storeAndAddEmployee = () => {
      let isValid = validatorEmployee();
      if (isValid && popupStatus.value.type === PopupType.create) {
        // lưu thông tin nhân viên
        store.dispatch("createEmployee", {
          employee: employeeInfo.value,
          typeStore: TypeStore.storeAndAdd,
        });
      } else if (isValid && popupStatus.value.type === PopupType.update) {
        // sửa thông tin nhân viên
        store.dispatch("updateEmployee", {
          employee: employeeInfo.value,
          typeStore: TypeStore.storeAndAdd,
        });
      }
      // có lỗi thì không làm gì cả
    };

    return {
      ButtonType,
      onClosePopup,
      PopupType,
      popupStatus,
      employeeInfo,
      employeeDialog,
      dataDeaprtment,
      employeeDetailRef,
      errRefs,
      btnClose,
      btnCancel,
      btnStore,
      btnStoreAndAdd,
      tmpTablIndex,
      hanldeFocusLast,
      hanldeFocusFirst,
      errValidator,
      storeEmployee,
      storeAndAddEmployee,
      dataDepartmentFilter,
      Gender,
      employeeDetail,
      errsValidate,
      MaxLength,
    };
  },
};
</script>

<template>
  <div
    ref="employeeDetailRef"
    class="popup-wrapper outline-none"
    :tabindex="0"
    @keydown.stop="">
    <div class="popup-container flex flex-col">
      <div class="popup__header flex items-center">
        <div class="popup-header__title">
          {{
            popupStatus?.type === PopupType.create
              ? "Thêm mới nhân viên"
              : "Cập nhật nhân viên"
          }}
        </div>
        <div class="popup-header__options flex items-center">
          <label
            class="checkbox-wrapper w-auto flex flex-row items-center pointer m-0">
            <input
              class="input-checkbox m-0 mr-1"
              type="checkbox"
              :checked="employeeInfo.IsCustomer === 1"
              @click="
                employeeInfo.IsCustomer === 1
                  ? (employeeInfo.IsCustomer = 0)
                  : (employeeInfo.IsCustomer = 1)
              " />
            <span> Là khách hàng </span>
          </label>
          <label
            class="checkbox-wrapper w-auto flex flex-row items-center pointer m-0">
            <input
              class="input-checkbox m-0 mr-1"
              type="checkbox"
              :checked="employeeInfo.IsSupplier === 1"
              @click="
                employeeInfo.IsSupplier === 1
                  ? (employeeInfo.IsSupplier = 0)
                  : (employeeInfo.IsSupplier = 1)
              " />
            <span> Là nhà cung cấp </span>
          </label>
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
      <form id="employeeForm" action="" class="popup__body flex-1">
        <div class="form-content flex flex-col gap-24-0">
          <div class="form-content__top flex flex-row gap-0-24">
            <div class="form-content-top__left flex flex-col gap-16-0 w-1/2">
              <div class="flex gap-0-8">
                <b-textfield
                  :ref="errRefs.EmployeeCode"
                  v-model="employeeInfo.EmployeeCode"
                  :tab-index="2"
                  required
                  label="Mã"
                  :max-length="MaxLength.code"
                  class-label="w-2/5"
                  :err-msg="errsValidate?.EmployeeCode?.join('') ?? ''"
                  @empty-err-msg="
                    () => {
                      delete errsValidate.EmployeeCode;
                    }
                  "
                  @keydown="hanldeFocusLast" />
                <b-textfield
                  :ref="errRefs.EmployeeName"
                  v-model="employeeInfo.FullName"
                  :tab-index="3"
                  required
                  label="Tên"
                  :max-length="MaxLength.name"
                  class-label="w-3/5"
                  :err-msg="errsValidate?.EmployeeName?.join('') ?? ''"
                  @empty-err-msg="
                    () => {
                      delete errsValidate.EmployeeName;
                    }
                  " />
              </div>
              <div class="w-full">
                <b-combobox
                  :ref="errRefs.DepartmentId"
                  :tab-index="4"
                  is-required
                  label-title="Đơn vị"
                  :max-length="MaxLength.default"
                  place-holder="-- Chọn đơn vị --"
                  :err-msg="errsValidate?.DepartmentId?.join('') ?? ''"
                  :data-list="dataDeaprtment"
                  :id-selected="employeeInfo.DepartmentId"
                  @on-click-id-selected="
                    (id) => (employeeInfo.DepartmentId = id)
                  "
                  @add-value-selected="
                    (value) => (employeeInfo.DepartmentName = value)
                  "
                  @empty-err-msg="
                    () => {
                      delete errsValidate.DepartmentId;
                    }
                  "
                  @keydown.tab.stop="" />
              </div>
              <div class="w-full">
                <b-textfield
                  v-model="employeeInfo.PositionName"
                  :tab-index="5"
                  label="Chức danh"
                  :max-length="MaxLength.name"
                  class-label="w-full" />
              </div>
            </div>
            <div class="form-content-top__right flex flex-col gap-16-0 w-1/2">
              <div class="flex items-center gap-0-8">
                <b-textfield
                  :ref="errRefs.DateOfBirth"
                  v-model="employeeInfo.DateOfBirth"
                  :tab-index="6"
                  input-type="date"
                  label="Ngày sinh"
                  class-label="w-2/5"
                  :err-msg="errsValidate?.DateOfBirth?.join('') ?? ''"
                  @empty-err-msg="
                    () => {
                      delete errsValidate.DateOfBirth;
                    }
                  " />

                <label class="w-3/5 m-0 block">
                  Giới tính
                  <div
                    class="flex flex-row gap-0-24 mt-2 h-8"
                    style="position: relative">
                    <label
                      class="flex flex-row items-center m-0 w-auto font-regular">
                      <input
                        v-model="employeeInfo.Gender"
                        :tabindex="7"
                        type="radio"
                        class="m-0 mr-1"
                        :value="Gender.male"
                        name="radio-gender"
                        @keydown.tab.stop="" />
                      Nam
                    </label>

                    <label
                      class="flex flex-row items-center m-0 w-auto font-regular">
                      <input
                        v-model="employeeInfo.Gender"
                        :tabindex="7"
                        type="radio"
                        class="m-0 mr-1"
                        :value="Gender.female"
                        name="radio-gender"
                        @keydown.tab.stop="" />
                      Nữ
                    </label>

                    <label
                      class="flex flex-row items-center m-0 w-auto font-regular">
                      <input
                        v-model="employeeInfo.Gender"
                        :tabindex="7"
                        type="radio"
                        class="m-0 mr-1"
                        :value="Gender.other"
                        name="radio-gender"
                        @keydown.tab.stop="" />
                      Khác
                    </label>
                  </div>
                </label>
              </div>
              <div class="flex w-full gap-0-8">
                <b-textfield
                  :ref="errRefs.IdentityNumber"
                  v-model="employeeInfo.IdentityNumber"
                  :tab-index="8"
                  label="Số CMND"
                  :max-length="MaxLength.identity"
                  title="Số căn cước công dân"
                  class-label="w-3/5"
                  :err-msg="errsValidate?.IdentityNumber?.join('') ?? ''"
                  @empty-err-msg="
                    () => {
                      delete errsValidate.IdentityNumber;
                    }
                  " />
                <b-textfield
                  :ref="errRefs.IdentityDateRelease"
                  v-model="employeeInfo.IdentityDateRelease"
                  :tab-index="9"
                  input-type="date"
                  label="Ngày cấp"
                  class-label="w-2/5"
                  :err-msg="errsValidate?.IdentityDateRelease?.join('') ?? ''"
                  @empty-err-msg="
                    () => {
                      delete errsValidate.IdentityDateRelease;
                    }
                  " />
              </div>
              <div class="w-full">
                <b-textfield
                  v-model="employeeInfo.IdentityPlaceRelease"
                  :tab-index="10"
                  label="Nơi cấp"
                  :max-length="MaxLength.default"
                  class-label="w-full" />
              </div>
            </div>
          </div>
          <div class="form-content__bottom flex flex-col gap-16-0">
            <div class="w-full">
              <b-textfield
                v-model="employeeInfo.Address"
                :tab-index="11"
                label="Địa chỉ"
                :max-length="MaxLength.default"
                class-label="w-full" />
            </div>

            <div class="w-full flex gap-0-8 justify-start">
              <b-textfield
                v-model="employeeInfo.PhoneNumber"
                :tab-index="12"
                label="ĐT di động"
                :max-length="MaxLength.phoneNumber"
                class-label="w-1/4"
                title="Điện thoại di động" />
              <b-textfield
                v-model="employeeInfo.MobilePhoneNumber"
                :tab-index="13"
                label="ĐT cố định"
                :max-length="MaxLength.phoneNumber"
                class-label="w-1/4"
                title="Điện thoại cố định" />

              <b-textfield
                :ref="errRefs.Email"
                v-model="employeeInfo.Email"
                :max-length="MaxLength.Email"
                :tab-index="14"
                label="Email"
                class-label="w-1/4"
                :err-msg="errsValidate?.Email?.join('') ?? ''"
                @empty-err-msg="
                  () => {
                    delete errsValidate.Email;
                  }
                " />
            </div>
            <div class="w-full flex gap-0-8 justify-start">
              <b-textfield
                v-model="employeeInfo.BankAccount"
                :tab-index="15"
                label="Tài khoản ngân hàng"
                :max-length="MaxLength.identity"
                class-label="w-1/4" />
              <b-textfield
                v-model="employeeInfo.BankName"
                :tab-index="16"
                label="Tên ngân hàng"
                :max-length="MaxLength.default"
                class-label="w-1/4" />
              <b-textfield
                v-model="employeeInfo.BankBranch"
                :tab-index="17"
                label="Chi nhánh"
                :max-length="MaxLength.default"
                class-label="w-1/4" />
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
            @keydown.enter="storeEmployee"
            @click="storeEmployee" />
          <b-button
            ref="btnStoreAndAdd"
            class="btn--pri"
            title="Cất và thêm"
            :tab-index="19"
            @keydown.enter="storeAndAddEmployee"
            @click="storeAndAddEmployee" />
        </div>
      </div>
    </div>
    <!-- <div ref="tmpTablIndex" :tabindex=" 1"></div> -->
  </div>
</template>
<style>
/* @import url(./EmployeeDetail.css); */
</style>
