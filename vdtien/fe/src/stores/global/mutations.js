const mutations = {
  TOGGLE_LOADING(state) {
    state.isLoading = !state.isLoading;
  },
  SET_TOAST(state, payload) {
    state.toast = { ...payload };
  },
  TOGGLE_SIDEBAR_SHRINK(state) {
    state.isSidebarShrink = !state.isSidebarShrink;
  },
  SET_FILTER_AND_PAGING(state, payload) {
    state.filterAndPaging = { ...payload };
  },
  SET_TOTAL_RECORDS(state, payload) {
    state.totalRecords = payload;
  },
  SET_TOTAL_PAGES(state, payload) {
    state.totalRoots = payload;
  },
  SET_DIALOG(state, payload) {
    state.dialog = { ...payload };
  },
  SET_ERRS_VALIDATE(state, payload) {
    state.errsValidate = { ...payload };
  },
  SET_POPUP_STATUS(state, payload) {
    state.popupStatus = { ...payload };
  },
};
export default mutations;
