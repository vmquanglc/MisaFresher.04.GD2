import account from "@/stores/account";

export const ErrValidator = {
  // employee
  employeeCodeEmppty: "Mã không được để trống.",
  employeeCodeForamt: "Mã nhân viên không đúng định dạng [NV-]<Mã số>.",
  maxLength: (field, length) => {
    return `${field} không được vượt quá ${length} ký tự.`;
  },
  containsOnlyNumber: (field) => {
    return `${field} chỉ chứa ký tự số.`;
  },
  employeeNameEmpty: "Tên không được để trống.",
  departmentEmpty: "Đơn vị không được để trống.",
  positionNameMaxLength: "Vị trí không quá 100 ký tự.",
  dateOfBirth: "Ngày sinh không lớn hơn ngày hiện tại.",
  identityNumberMaxLength: "Số chứng minh nhân dân không quá 25 ký tự.",
  identityNumberContainsOnlyNumber: "Số chứng minh nhân dân chỉ chứa số.",
  identityPlaceReleaseMaxLength: "Nơi cấp không quá 255 ký tự.",
  identityDateRelease: "Ngày cấp không lớn hơn ngày hiện tại.",
  addressMaxLength: "Địa chỉ không quá 255 ký tự.",
  mobilePhoneMaxLength: "Số điện thoại di động không quá 50 ký tự.",
  mobilePhoneContainsOnlyNumber: "Số điện thoại di dộng chỉ chứa số.",
  phoneNumberMaxLength: "Số điện thoại không quá 50 ký tự.",
  phoneNumberContainsOnlyNumber: "Số điện thoại chỉ chứa số.",
  email: "Email không đúng định dạng.",

  // account
  fieldEmplty: (fieldName) => `${fieldName} không được để trống.`,
  accountCodeEmpty: "Mã tài khoản không được để trống.",
  accountNameEmpty: "Tên tài khoản không được để trống.",
  accountFeatureEmpty: "Tính chất tài khoản không được để trống.",
};

export const DialogTitle = {
  inValidInput: "Dữ liệu không hợp lệ.",
  store: "Cất bản ghi?",
  delete: "Xóa bản ghi?",
  errorServer: "Lỗi máy chủ.",
};

export const DialogContent = {
  confirmDeleteEmployee: (employeeCode) => {
    return `Bạn có chắc muốn xóa nhân viên <${employeeCode}> này không?`;
  },
  confirmStoreEmployee: "Dữ liệu thay đỏi bạn có muốn lưu không?",
  confirmDeleteMultiEmployee: (numberRecords) => {
    return `Bạn có chắc chắn muốn xóa ${numberRecords} nhân viên này không?`;
  },

  // account
  confirmDeleteAccount: (accountCode) => {
    return `Bạn có chắc muốn xóa tài khoản <${accountCode}> này không?`;
  },
};

export const ToastContent = {
  createEmployeeSuccess: "Thêm nhân viên mới thành công.",
  updateEmployeeSuccess: (employeeCode) => {
    return `Cập nhật nhân viên <${employeeCode}> thành công.`;
  },
  deleteEmployeeSuccess: (employeeCode) => {
    return `Xóa nhân viên <${employeeCode}> thành công.`;
  },
  deleteMultiEmployeeSuccess: "Xóa nhân viên thành công.",

  // account
  createAccountSuccess: "Thêm tài khoản mới thành công.",
  deleteAccountSuccess: (accountCode) => {
    return `Xóa tài khoản <${accountCode}> thành công.`;
  },
  updateAccountSuccess: (accountCode) => {
    return `Cập nhật tài khoản <${accountCode}> thành công.`;
  },
};
export const AccountCol = {
  AccountCode: {
    text: "Mã tài khoản",
  },
  AccountName: {
    text: "Tên tài khoản",
  },
  AccountFeature: {
    text: "Tính chất",
  },
  AccountNameEnglish: {
    text: "Tên tiếng anh",
  },
  Explain: {
    text: "Diễn giải",
  },
  Status: {
    text: "Trạng thái sử dụng",
  },
  action: {
    text: "Chức năng",
  },
};
// employee
export const EmployeeCol = {
  checkbox: {
    text: "",
  },
  EmployeeCode: {
    text: "Mã nhân viên",
  },
  FullName: {
    text: "Tên nhân viên",
  },
  Gender: {
    text: "Giới tính",
  },
  DateOfBirth: {
    text: "Ngày sinh",
  },
  PositionName: {
    text: "Chức danh",
  },
  DepartmentName: {
    text: "Bộ phận",
  },
  IdentityNumber: {
    text: "Số CMNN",
    title: "Số chứng minh nhân dân",
  },
  IdentityDateRelease: {
    text: "Ngày cấp",
  },
  IdentityPlaceRelease: {
    text: "Nơi cấp",
  },
  Address: {
    text: "Địa chỉ",
  },
  PhoneNumber: {
    text: "ĐT cố định",
    title: "Điện thoại cố định",
  },
  MobilePhoneNumber: {
    text: "ĐT di động",
    title: "Điện thoại di động",
  },
  Email: {
    text: "Email",
  },
  BankAccount: {
    text: "STK ngân hàng",
    title: "Số tài khoản ngân hàng",
  },
  BankName: {
    text: "Tên ngân hàng",
  },
  BankBranch: {
    text: "Tên chi nhánh",
  },
  action: {
    text: "Chức năng",
  },
};
export const SidebarList = {
  top: {
    general: {
      name: "Employee",
      icon: "icon--general",
      title: "Tổng quan",
      text: "Tổng quan",
    },
    cash: {
      name: "Fake",
      icon: "icon--cash",
      title: "Tiền mặt",
      text: "Tiền mặt",
    },
    deposits: {
      name: "Components",
      icon: "icon--deposits",
      title: "Tiền gửi",
      text: "Tiền gửi",
    },
    purchase: {
      name: "",
      icon: "icon--purchase",
      title: "Mua hàng",
      text: "Mua hàng",
    },
    sell: {
      name: "",
      icon: "icon--sell",
      title: "Bán hàng",
      text: "Bán hàng",
    },
    billManagement: {
      name: "",
      icon: "icon--billManagement",
      title: "Quản lý hóa đơn",
      text: "Quản lý hóa đơn",
    },
    warehouse: {
      name: "",
      icon: "icon--warehouse",
      title: "Kho",
      text: "Kho",
    },
    tools: {
      name: "",
      icon: "icon--tools",
      title: "Công cụ dụng cụ",
      text: "Công cụ dụng cụ",
    },
    fixedAssets: {
      name: "",
      icon: "icon--fixedAssets",
      title: "Tài sản cố định",
      text: "Tài sản cố định",
    },
    tax: {
      name: "",
      icon: "icon--tax",
      title: "Thuế",
      text: "Thuế",
    },
    price: {
      name: "",
      icon: "icon--price",
      title: "Giá thành",
      text: "Giá thành",
    },
    synthetic: {
      name: "",
      icon: "icon--synthetic",
      title: "Tổng hợp",
      text: "Tổng hợp",
    },
    budget: {
      name: "",
      icon: "icon--budget",
      title: "Ngân sách",
      text: "Ngân sách",
    },
    report: {
      name: "",
      icon: "icon--report",
      title: "Báo cáo",
      text: "Báo cáo",
    },
    financialAnalysis: {
      name: "",
      icon: "icon--financialAnalysis",
      title: "Phân tích tài chính",
      text: "Phân tích tài chính",
    },
  },
  bottom: {
    directory: {
      name: "Directory",
      icon: "icon--tools",
      title: "Danh mục",
      text: "Danh mục",
    },
    initialBalance: {
      name: "",
      icon: "icon--fixedAssets",
      title: "Số dư ban đầu",
      text: "Số dư ban đầu",
    },
    knowledge: {
      name: "",
      icon: "icon--tax",
      title: "Kiến thức",
      text: "Kiến thức",
    },
  },
};
export const ButtonTitle = {
  addEmployee: "Thêm mới nhân viên",
  store: "Cất",
  storeAndAdd: "Cất và thêm",
  delete: "Xóa",
  edit: "Sửa",
  duplicate: "Nhân bản",
  cancel: "Hủy",
  multiAction: "Thực hiện hàng loạt",
  prev: "Trước",
  last: "Sau",
};
export const FreeText = {
  employee: "Nhân viên",
  notFoundRecord: "Không tìm thấy bản ghi nào!",
  totalRecords: "Tổng số",
  accountSystem: "Hệ thống tài khoản",
  directory: "Danh mục",
};
export const InputPlaceholder = {
  findEmployee: "Tìm kiếm nhân viên...",
  findAccount: "Tìm kiếm theo số, tên tài khoản",
};
export const StatusText = {
  using: "Đang sử dụng",
  stopUsing: "Ngừng sử dụng",
};
