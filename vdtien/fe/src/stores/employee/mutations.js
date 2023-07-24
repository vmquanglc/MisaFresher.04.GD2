import {
  areAllElementsInArray,
  removeSubArrayElements,
  uniqueElementsArray,
} from "@/utils/helper";

const mutations = {
  SET_EMPLOYEE_LIST(state, payload) {
    state.employeeList = [...payload];
  },
  SET_NEW_EMPLOYEE_CODE(state, payload) {
    state.employeeDetail = { ...state.employeeDetail, EmployeeCode: payload };
  },
  SET_EMPLOYEE_DETAIL(state, payload) {
    state.employeeDetail = { ...payload };
  },
  // SET_POPUP_STATUS(state, payload) {
  //   state.popupStatus = { ...payload };
  // },
  CREATE_EMPLOYEE(state, payload) {
    let newEmployee = { ...payload };
    state.employeeList.unshift(newEmployee);
  },
  UPDATE_EMPLOYEE(state, payload) {
    let updatedEmployee = { ...payload };
    const index = state.employeeList.findIndex(
      (emp) => emp.EmployeeId === updatedEmployee.EmployeeId
    );
    if (index !== -1) {
      let tmpEmployeeList = [...state.employeeList];
      tmpEmployeeList.splice(index, 1, updatedEmployee);
      state.employeeList = [...tmpEmployeeList];
    }
  },
  /**
   * Mô tả:
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  DELETE_EMPLOYEE(state, payload) {
    const index = state.employeeList.findIndex(
      (emp) => emp.EmployeeId === payload
    );
    // console.log(index);
    if (index !== -1) {
      let tmpEmployeeList = [...state.employeeList];
      tmpEmployeeList.splice(index, 1);
      // console.log(tmpEmployeeList);
      state.employeeList = [...tmpEmployeeList];
    }
  },

  // SET_FILTER_AND_PAGING(state, payload) {
  //   state.filterAndPaging = { ...payload };
  // },
  SET_LIST_EMPLOYEE_LIST_CHECKED(state, payload) {
    let tmpEmployeeIdListChecked = [...state.employeeIdListChecked]; // Giữ nguyên proxy array

    if (!payload) {
      state.employeeIdListChecked = [];
    } else {
      const proxyPayload = new Proxy(payload, {}); // Chuyển đổi payload thành proxy array

      if (areAllElementsInArray(proxyPayload, tmpEmployeeIdListChecked)) {
        // console.log("oke");
        let newArr = removeSubArrayElements(
          proxyPayload,
          tmpEmployeeIdListChecked
        );
        state.employeeIdListChecked = [...newArr];
      } else {
        const uniqueEls = uniqueElementsArray(
          proxyPayload,
          tmpEmployeeIdListChecked
        );
        tmpEmployeeIdListChecked.push(...uniqueEls);
        state.employeeIdListChecked = [...tmpEmployeeIdListChecked];
      }
    }
  },
};

// const list = [
//   {
//     AccountId: 1,
//     AccountName: "1",
//     ParentId: null,
//   },
//   {
//     AccountId: 2,
//     AccountName: "2",
//     ParentId: null,
//   },
//   {
//     AccountId: 4,
//     AccountName: "4",
//     ParentId: 1,
//   },
//   {
//     AccountId: 3,
//     AccountName: "3",
//     ParentId: 4,
//   },
//   {
//     AccountId: 5,
//     AccountName: "5",
//     ParentId: null,
//   },
//   {
//     AccountId: 6,
//     AccountName: "6",
//     ParentId: 2,
//   },
//   {
//     AccountId: 7,
//     AccountName: "7",
//     ParentId: 2,
//   },
// ];

// [
//   {
//     AccountId: 1,
//     AccountName: "1",
//     ParentId: null,
//   },
//   {
//     AccountId: 4,
//     AccountName: "4",
//     ParentId: 1,
//   },

//   {
//     AccountId: 3,
//     AccountName: "3",
//     ParentId: 4,
//   },
//   {
//     AccountId: 2,
//     AccountName: "2",
//     ParentId: null,
//   },
//   {
//     AccountId: 6,
//     AccountName: "6",
//     ParentId: 2,
//   },
//   {
//     AccountId: 7,
//     AccountName: "7",
//     ParentId: 2,
//   },

//   {
//     AccountId: 5,
//     AccountName: "5",
//     ParentId: null,
//   },
// ];

export default mutations;
