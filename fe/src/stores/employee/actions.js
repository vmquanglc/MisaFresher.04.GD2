import EmployeesService from "@/api/services/employeeService";
import {
  DialogAction,
  DialogType,
  PopupType,
  StatusCode,
  ToastType,
  TypeStore,
} from "@/enums";
import { DialogTitle, ToastContent } from "@/resources";
import { convertToYYYYMMDD } from "@/utils/helper";
const actions = {
  /**
   *
   * @param {} param0
   * lấy danh sách nhân viên từ api
   *  Author:vdtien(25/5/2023)
   */
  async getAllEmployeeList({ state, commit, dispatch }) {
    try {
      dispatch("toggleLoading");
      const data = await EmployeesService.getAll();
      commit("SET_EMPLOYEE_LIST", data);
    } catch (error) {
      console.log(error);
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: lấy danh sách nhân viên theo phân trang và filter
   * created by : vdtien
   * created date: 05-06-2023
   * @param {type} param -
   * @returns
   */
  async getEmployeeList({ state, rootState, commit, dispatch }) {
    try {
      dispatch("toggleLoading");

      let res = await EmployeesService.getList(
        rootState.global.filterAndPaging
      );
      if (res && res.Data) {
        commit("SET_EMPLOYEE_LIST", res.Data);
        dispatch("getTotalRecords", res.TotalRecord);
        // commit("SET_TOTAL_RECORDS", res.TotalRecord);
      }
    } catch (error) {
      console.log(error);
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: export file excel danh sach nhan vien
   * created by : vdtien
   * created date: 30-06-2023
   * @param {type} param -
   * @returns
   */
  async exportExcelEmployeeList({ state, commit, dispatch }) {
    try {
      dispatch("toggleLoading");

      let res = await EmployeesService.exportExcelEmployeeList(
        state?.filterAndPaging?.keySearch ?? ""
      );
      if (res) {
        //https://stackoverflow.com/questions/41938718/how-to-download-files-using-axios
        // create file link in browser's memory
        const href = URL.createObjectURL(res);

        // create "a" HTML element with href to file & click
        const link = document.createElement("a");
        link.href = href;
        link.setAttribute("download", `Danh_sach_nhan_vien_${Date.now()}.xlsx`); //or any other extension
        document.body.appendChild(link);
        link.click();

        // clean up "a" element & remove ObjectURL
        document.body.removeChild(link);
        URL.revokeObjectURL(href);
      }
    } catch (error) {
      // console.log(error);
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: trả về thôngg tin nhân viên theo id
   * created by : vdtien
   * created date: 04-06-2023
   * @param {string} employeeId - id của nhân viên
   * @returns
   */
  async getEmployeeById({ state, commit, dispatch }, employeeId) {
    try {
      dispatch("toggleLoading");
      const data = await EmployeesService.getById(employeeId);
      if (data) {
        dispatch("getEmployeeDetail", { ...data });
      }
    } catch (error) {
      console.log(error);
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: lấy mã nhân viên mới nhất
   * created by : vdtien
   * created date: 04-06-2023
   * @param {type} param -
   * @returns
   */
  async getNewEmployeecode({ state, commit, dispatch }) {
    try {
      dispatch("toggleLoading");
      const data = await EmployeesService.getNewEmployeecode();
      if (data) commit("SET_NEW_EMPLOYEE_CODE", data);
      // chuyển employeeCode sang dạng NV-<mã số>
      // if (data) {
      //   let newEmployeeCode = `NV-${data}`;
      //   commit("SET_NEW_EMPLOYEE_CODE", newEmployeeCode);
      // }
    } catch (error) {
      console.log(error);
      hanldeException(dispatch, error);
    } finally {
      dispatch("toggleLoading");
    }
  },

  /**
   * Mô tả: thêm mới nhân viên
   * created by : vdtien
   * created date: 29-05-2023
   * @param {object} employee - thông tin nhân viên thêm mới
   * @returns
   */
  async createEmployee({ state, rootState, commit, dispatch }, payload) {
    const { employee, typeStore } = payload;
    try {
      dispatch("toggleLoading");

      // thêm mới nếu không trùng mã
      let res = await EmployeesService.createRecord(employee);
      // thêm thành công
      if (res) {
        // console.log(res);
        // add nhân viên mới vào đầu danh sách
        // res đang trả về chỉ là 1 nếu thành công thì không ổn, cần trả về bản ghi mới để lấy id bản ghi
        commit("CREATE_EMPLOYEE", res);
        dispatch("getToast", {
          isShow: true,
          type: ToastType.success,
          content: ToastContent.createEmployeeSuccess,
        });
        dispatch("getEmployeeDetail");
        dispatch("getTotalRecords", rootState.global.totalRecords + 1);
        // commit("SET_TOTAL_RECORDS", state.totalRecords + 1);
        dispatch("getPopupStatus");
        if (typeStore === TypeStore.store) {
        } else if (typeStore === TypeStore.storeAndAdd) {
          const data = await EmployeesService.getNewEmployeecode();
          if (data) commit("SET_NEW_EMPLOYEE_CODE", data);
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
  async updateEmployee({ state, commit, dispatch }, payload) {
    const { employee, typeStore } = payload;
    try {
      dispatch("toggleLoading");
      // call api cập nhật lại nhân viên
      let employeeId = employee.EmployeeId;
      // delete employee.EmployeeId;
      let res = await EmployeesService.updateRecord(employee, employeeId);

      //cập nhật thành công
      if (res) {
        commit("UPDATE_EMPLOYEE", res);
        dispatch("getToast", {
          isShow: true,
          type: ToastType.success,
          content: ToastContent.updateEmployeeSuccess(employee.EmployeeCode),
        });
        dispatch("getEmployeeDetail");
        dispatch("getPopupStatus");
        if (typeStore === TypeStore.store) {
        } else if (typeStore === TypeStore.storeAndAdd) {
          const data = await EmployeesService.getNewEmployeecode();
          if (data) commit("SET_NEW_EMPLOYEE_CODE", data);
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
   * Mô tả: Xoa nhan vien theo id
   * created by : vdtien
   * created date: 23-06-2023
   * @param {type} param -
   * @returns
   */
  async deleteEmployee({ state, rootState, commit, dispatch }, employee) {
    try {
      dispatch("toggleLoading");
      // console.log(employee);
      // call api xóa nhân viên theo id
      let res = await EmployeesService.deleteRecord(employee.EmployeeId);

      //xóa thành công
      if (res) {
        commit("DELETE_EMPLOYEE", employee.EmployeeId);
        dispatch("getToast", {
          isShow: true,
          type: ToastType.success,
          content: ToastContent.deleteEmployeeSuccess(employee.EmployeeCode),
        });
      }
      dispatch("getEmployeeDetail");
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

  async deleteMultiEmployee({ state, rootState, commit, dispatch }) {
    try {
      // console.log(state.employeeIdListChecked);
      dispatch("toggleLoading");
      let res = await EmployeesService.deleleRecordMulti(
        state.employeeIdListChecked
      );
      if (res) {
        dispatch("toggleLoading");
        dispatch("getToast", {
          isShow: true,
          type: ToastType.success,
          content: ToastContent.deleteMultiEmployeeSuccess,
        });
        dispatch("getFilterAndPaging", {
          ...rootState.global.filterAndPaging,
          pageNumber: 1,
        });
        dispatch("getEmployeeIdListCkecked");
        dispatch("getEmployeeList");
      }
    } catch (error) {
      dispatch("toggleLoading");
      console.log(error);
      // add error vào dialog
      hanldeException(dispatch, error);
    }
  },

  /**
   *
   * @param {*} param0
   * @param {*} payload
   * lấy dữ liệu cho employeeDetail
   * Author:vdtien(25/5/2023)
   */
  getEmployeeDetail({ commit }, payload) {
    if (payload?.DateOfBirth) {
      payload.DateOfBirth = convertToYYYYMMDD(payload.DateOfBirth);
    }
    if (payload?.IdentityDateRelease) {
      payload.IdentityDateRelease = convertToYYYYMMDD(
        payload.IdentityDateRelease
      );
    }
    commit("SET_EMPLOYEE_DETAIL", payload);
  },

  /**
   * Mô tả: set tham số filter and paging
   * created by : vdtien
   * created date: 30-05-2023
   */
  getFilterAndPaging({ state, commit, dispatch }, payload) {
    commit("SET_FILTER_AND_PAGING", payload);

    // call api employeeList
  },

  getEmployeeIdListCkecked({ state, commit, dispatch }, payload) {
    commit("SET_LIST_EMPLOYEE_LIST_CHECKED", payload);
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
