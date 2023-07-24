import AccountService from "@/api/services/accountService";
import { DialogAction, DialogType, ToastType, TypeStore } from "@/enums";
import { DialogTitle, ToastContent } from "@/resources";
import { nestTreeData } from "@/utils/helper";
import { ref } from "vue";
const actions = {
  /**
   * Mô tả: lay danh sach tai khoan
   * created by : vdtien
   * created date: 18-07-2023
   * @param {type} param -
   * @returns
   */
  async getAccountsListTree({ state, rootState, commit, dispatch }) {
    try {
      dispatch("toggleLoading");

      let res = await AccountService.getListTreeAccount({
        ...rootState.global.filterAndPaging,
        isRoot: true,
        grade: 0,
      });
      if (res && res.Data && res.TotalRecord) {
        // console.log(res);
        dispatch("getTotalRecords", res.TotalRecord);
        dispatch("getTotalRoots", res.TotalRoot);
        let resNew = res.Data.map((item) => ({ ...item, showChild: false }));
        commit("SET_ACCOOUNTS_LIST", resNew);
      }
    } catch (error) {
      console.log(error);
      // hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: chen danh sach account con vao duoi account cha
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  async getAccountsListByParentId({ state, commit, dispatch }, parentId) {
    try {
      dispatch("toggleLoading");
      let res = await AccountService.getListAccountByParentId(parentId);
      if (res && res.length > 0) {
        res = res.map((item) => ({ ...item, showChild: false }));
        // res = ref([...res]);
        dispatch("getAccountsListChildrensToList", {
          parentId: parentId,
          childrens: res,
        });
      }
    } catch (error) {
      console.log(error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  async getListAccountChildrenByListParentId(
    { state, commit, dispatch },
    listParentId
  ) {
    try {
      dispatch("toggleLoading");
      await dispatch("sortTreeChildrens", listParentId);
    } catch (error) {
      console.log(error);
    } finally {
      dispatch("toggleLoading");
    }
  },
  /**
   * Mô tả: lay danh sach account theo danh sach parent id
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  async sortTreeChildrens({ state, commit, dispatch }, listParentId) {
    try {
      let res = await AccountService.getListAccountByListParentId(listParentId);
      if (res && res.length > 0) {
        // tien xu ly res them truong showChild
        listParentId = [...res.map((obj) => obj.AccountId)];

        res = res.map((obj) => ({ ...obj, showChild: false }));
        // sap xep cay
        await dispatch("getListAccountChildrenByParents", res);
        // de quy
        await dispatch("sortTreeChildrens", listParentId);
      }
    } catch (error) {
      console.log(error);
    } finally {
    }
  },

  /**
   * Mô tả: them danh sach node con vao danh sach node cha
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  getListAccountChildrenByParents({ state, commit, dispatch }, payload) {
    commit("SET_LIST_ACCOUNT_CHILDRENS_TO_PARENTS", payload);
  },

  /**
   * Mô tả: chen danh sach con vao sau cha
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  getAccountsListChildrensToList({ state, commit, dispatch }, payload) {
    commit("SET_ACCOUNT_LIST_CHILDRENS", payload);
  },

  /**
   * Mô tả: thêm mới tài khoản
   * created by : vdtien
   * created date: 22-07-2023
   * @param {object} account - thông tin tài khoản thêm mới
   * @returns
   */
  async createAccount({ state, rootState, commit, dispatch }, payload) {
    // sau khi them thanh cong load lai page ve trang dau
    const { account, typeStore } = payload;
    try {
      dispatch("toggleLoading");

      // thêm mới nếu không trùng mã
      let res = await AccountService.createRecord(account);
      // thêm thành công
      if (res) {
        // console.log(res);
        // tang ban gho len 1 va chen ban ghi moi vao danh sach
        res = { ...res, showChild: false };
        if (!res.ParentId) {
          commit("CREATE_ACCOUNT", res);
        } else {
          let parentIndex = state.accountsList.findIndex(
            (acc) => acc.AccountId === res.ParentId
          );
          // lay cac tai khoan con co parentId = res.ParentId
          if (parentIndex) {
            dispatch("toggleLoading");
            await dispatch("getAccountsListByParentId", res.ParentId);
            dispatch("toggleLoading");
          }
        }
        dispatch("getToast", {
          isShow: true,
          type: ToastType.success,
          content: ToastContent.createAccountSuccess,
        });
        dispatch("getAccountDetail");
        // commit("SET_TOTAL_RECORDS", state.totalRecords + 1);
        dispatch("getPopupStatus");
        // // laod lai du lieu
        // dispatch("getFilterAndPaging", {
        //   pageSize: rootState.global.filterAndPaging.pageSize,
        //   keySearch: "",
        //   pageNumber: 1,
        // });
        // dispatch("toggleLoading");
        // await dispatch("getAccountsListTree");
        // dispatch("toggleLoading");

        if (typeStore === TypeStore.store) {
        } else if (typeStore === TypeStore.storeAndAdd) {
          dispatch("getPopupStatus", {
            isShowPopup: true,
            type: PopupType.create,
          });
        }
      }

      // thêm thất bại
    } catch (error) {
      // console.log(error);
      // add error vào dialog
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: Cap nhat nhan vien
   * created by : vdtien
   * created date: 23-06-2023
   * @param {type} param -
   * @returns
   */
  async updateAccount({ state, commit, dispatch }, payload) {
    const { account, typeStore } = payload;
    try {
      dispatch("toggleLoading");

      // call api cập nhật lại nhân viên
      let accountId = account.AccountId;
      // delete account.AccountId;
      let res = await AccountService.updateRecord(account, accountId);

      //cập nhật thành công
      if (res) {
        res.showChild = false;
        commit("UPDATE_EMPLOYEE", res);
        dispatch("getToast", {
          isShow: true,
          type: ToastType.success,
          content: ToastContent.updateAccountSuccess(account.AccountCde),
        });
        dispatch("getAccountDetail");
        dispatch("getPopupStatus");
        if (typeStore === TypeStore.store) {
        } else if (typeStore === TypeStore.storeAndAdd) {
          dispatch("getPopupStatus", {
            isShowPopup: true,
            type: PopupType.create,
          });
        }
      }
      // lỗi
    } catch (error) {
      console.log(error);
      // add error vào dialog
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: Xoa tai khoan theo id
   * created by : vdtien
   * created date: 23-06-2023
   * @param {type} param -
   * @returns
   */
  async deleteAccount({ state, rootState, commit, dispatch }, account) {
    try {
      dispatch("toggleLoading");
      console.log(account);
      // call api xóa nhân viên theo id
      let res = await AccountService.deleteRecord(account.AccountId);

      //xóa thành công
      if (res) {
        commit("DELETE_EMPLOYEE", account);
        dispatch("getToast", {
          isShow: true,
          type: ToastType.success,
          content: ToastContent.deleteAccountSuccess(account.AccountCode),
        });
      }
      dispatch("getAccountDetail");
      dispatch("getTotalRecords", rootState.global.totalRecords - 1);
      // commit("SET_TOTAL_RECORDS", state.totalRecords - 1);

      // lỗi
    } catch (error) {
      console.log(error);
      // add error vào dialog
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   *
   * @param {*} param0
   * @param {*} payload
   * lấy dữ liệu cho accountDetail
   * Author:vdtien(19/7/2023)
   */
  getAccountDetail({ commit }, payload) {
    commit("SET_ACCOUNT_DETAIL", payload);
  },

  /**
   * Mô tả: toggle cac node con
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  toggleShowChild({ commit }, payload) {
    commit("TOGGLE_SHOW_CHILD", payload);
  },

  /**
   * Mô tả: an het tat cac cap
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  toggleShowAll({ commit }, payload) {
    commit("TOGGLE_SHOW_ALL", payload);
  },
};
function hanldeException(dispatch, ex) {
  console.log(ex);
  let errsMsg = ex?.response?.data?.UserMsg ?? [];
  if (!Array.isArray(errsMsg)) {
    errsMsg = ["Có lỗi vui lòng liên hệ nhân viên Misa để được hỗ trợ"];
  }
  // check loi validate hoac dupCode
  if (ex?.response?.data?.ErrCode === 2 || ex?.response?.data?.ErrCode === 3) {
    dispatch("getErrsValidate", ex?.response?.data?.ErrorsMore ?? {});
    dispatch("getDialog", {
      isShow: true,
      type: DialogType.error,
      title: DialogTitle.inValidInput,
      content: [...errsMsg],
      action: DialogAction.confirmValidate,
    });
  } else {
    dispatch("getDialog", {
      isShow: true,
      type: DialogType.error,
      title: DialogTitle.errorServer,
      content: [...errsMsg],
    });
  }
}
export default actions;
