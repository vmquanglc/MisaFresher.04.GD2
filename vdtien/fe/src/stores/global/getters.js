const getters = {
  isLoading: (state) => state.isLoading,
  toast: (state) => state.toast,
  isSidebarShrink: (state) => state.isSidebarShrink,
  dialog: (state) => state.dialog,
  filterAndPaging: (state) => state.filterAndPaging,
  totalRecords: (state) => state.totalRecords,
  totalRoots: (state) => state.totalRoots,
  errsValidate: (state) => state.errsValidate,
};
export default getters;
