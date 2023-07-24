const mutations = {
  SET_ACCOOUNTS_LIST(state, payload) {
    state.accountsList = [...payload];
  },
  SET_ACCOUNT_LIST_CHILDRENS(state, payload) {
    // tim vi tri cha,
    let parent = state.accountsList.find(
      (acc) => acc.AccountId === payload.parentId
    );
    let index = state.accountsList.findIndex(
      (acc) => acc.AccountId === payload.parentId
    );
    // console.log(index);

    // insert danh sach con vao duoi cha
    if (index !== -1) {
      let tmpAccoutsList = [...state.accountsList];
      parent.IsParent = 1;
      parent.showChild = true;
      parent.NumberChilds = payload.childrens.length;
      tmpAccoutsList.splice(index, 1, { ...parent });
      tmpAccoutsList.splice(index + 1, 0, ...payload.childrens);
      state.accountsList = [...tmpAccoutsList];
    }
  },
  SET_LIST_ACCOUNT_CHILDRENS_TO_PARENTS(state, payload) {
    let tmpAccoutsList = [...state.accountsList];

    for (const account of payload) {
      const parentIndex = tmpAccoutsList.findIndex(
        (acc) => acc.AccountId === account.ParentId
      );
      if (parentIndex !== -1) {
        tmpAccoutsList.splice(parentIndex + 1, 0, account);
      }
    }

    state.accountsList = [...tmpAccoutsList];
  },
  SET_ACCOUNT_DETAIL(state, payload) {
    state.accountDetail = { ...payload };
  },
  TOGGLE_SHOW_CHILD(state, payload) {
    // console.log(state);
    let index = state.accountsList.findIndex(
      (acc) => acc.AccountId === payload
    );
    let account = state.accountsList.find((acc) => acc.AccountId === payload);
    if (index !== -1) {
      let tmpAccoutsList = [...state.accountsList];
      account.showChild = !account.showChild;
      tmpAccoutsList.splice(index, 1, account);
      state.accountsList = [...tmpAccoutsList];
    }
  },
  TOGGLE_SHOW_ALL(state, payload) {
    let tmpAccoutsList = [...state.accountsList];
    tmpAccoutsList = tmpAccoutsList.map((obj) => ({
      ...obj,
      showChild: payload,
    }));
    state.accountsList = [...tmpAccoutsList];
  },
  /**
   * Mô tả: them moi 1 ban ghi
   * created by : vdtien
   * created date: 23-07-2023
   * @param {type} param -
   * @returns
   */
  CREATE_ACCOUNT(state, payload) {
    let newAccount = { ...payload };
    state.accountsList.unshift(newAccount);
  },

  /**
   * Mô tả: Cap nhat tai khoan
   * created by : vdtien
   * created date: 23-07-2023
   * @param {type} param -
   * @returns
   */
  UPDATE_EMPLOYEE(state, payload) {
    let account = { ...payload };
    let tmpAccountsList = [...state.accountsList];
    // tim tai khoan cu
    let accountPrev = state.accountsList.find(
      (acc) => acc.AccountId === account.AccountId
    );
    let index = state.accountsList.findIndex(
      (acc) => acc.AccountId === account.AccountId
    );
    // thay đổi nhánh
    if (accountPrev && accountPrev.ParentId != account.ParentId) {
      // parent old khac null
      if (accountPrev.ParentId) {
        let parentOld = tmpAccountsList.find(
          (acc) => acc.AccountId === accountPrev.ParentId
        );
        let indexParentOld = tmpAccountsList.find(
          (acc) => acc.AccountId === accountPrev.ParentId
        );

        if (indexParentOld !== -1) {
          parentOld.NumberChilds -= 1;
          parentOld.IsParent = parentOld.NumberChilds > 0 ? 1 : 0;
          tmpAccountsList.splice(indexParentOld, 1, { ...parentOld });
        }
      }
    }
    // xoa node cu di
    if (index != -1) {
      tmpAccountsList.splice(index, 1);
    }
    let parenNode = tmpAccountsList.find(
      (acc) => acc.AccountId === account.ParentId
    );
    let indexParent = tmpAccountsList.findIndex(
      (acc) => acc.AccountId === account.ParentId
    );

    if (indexParent !== -1) {
      parenNode.NumberChilds += 1;
      parenNode.IsParent = 1;
      tmpAccountsList.splice(indexParent, 1, { ...parenNode });
      if (parenNode.showChild) {
        tmpAccountsList.splice(indexParent + 1, 0, account);
      }
    }

    state.accountsList = [...tmpAccountsList];
  },

  /**
   * Mô tả:xoa tai khoan theo id
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  DELETE_EMPLOYEE(state, payload) {
    const index = state.accountsList.findIndex(
      (acc) => acc.AccountId === payload.AccountId
    );
    let tmpAccountsList = [...state.accountsList];
    if (payload.ParentId) {
      const indexParent = state.accountsList.findIndex(
        (acc) => acc.AccountId === payload.ParentId
      );
      const parentNode = state.accountsList.find(
        (acc) => acc.AccountId === payload.ParentId
      );
      if (indexParent != -1) {
        parentNode.NumberChilds -= 1;
        if (parentNode.NumberChilds === 0) {
          parentNode.IsParent = 0;
        }
        tmpAccountsList.splice(indexParent, 1, { ...parentNode });
      }
    }
    // console.log(index);
    if (index !== -1) {
      tmpAccountsList.splice(index, 1);
      // console.log(tmpAccountList);
    }
    state.accountsList = [...tmpAccountsList];
  },
};
export default mutations;
