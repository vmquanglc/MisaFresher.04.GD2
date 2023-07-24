const state = {
  isLoading: false,
  toast: {
    isShow: false,
    type: "",
    content: "",
  },
  isSidebarShrink: false,
  dialog: {
    isShow: false,
    type: "",
    title: "",
    content: [],
    action: "",
  },
  filterAndPaging: {
    keySearch: "",
    pageSize: 20,
    pageNumber: 1,
  },
  totalRecords: 0,
  totalRoots: 1,
  errsValidate: {},
  popupStatus: {
    isShowPopup: false,
    type: "",
  },
};
export default state;
