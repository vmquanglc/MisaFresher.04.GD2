import { AccountFeature, Gender, Status } from "@/enums";

function removeDiacritics(str) {
  return str.normalize("NFD").replace(/[\u0300-\u036f]/g, "");
}

function converGender(genderType) {
  let gender = "";
  switch (genderType) {
    case Gender.male:
      gender = "Nam";
      break;
    case Gender.female:
      gender = "Nữ";
      break;
    case Gender.other:
      gender = "Khác";
      break;
    default:
      break;
  }
  return gender;
}
/**
 * Mô tả: Kiem tra xem chuoi do co dang date khoong
 * created by : vdtien
 * created date: 18-06-2023
 * @param {type} param -
 * @returns boolean
 */
function isValidDate(dateString) {
  let date = new Date(dateString);
  return !isNaN(date);
}

/**
 * Mô tả: Convert string ve dd/mm/yyyy
 * created by : vdtien
 * created date: 18-06-2023
 * @param {type} param -
 * @returns "dd/mm/yyy" || ""
 */
function convertToDDMMYYYY(dateString) {
  if (!isValidDate(dateString)) {
    return "";
  }

  let date = new Date(dateString);
  let day = date.getDate();
  let month = date.getMonth() + 1; // Tháng trong JavaScript bắt đầu từ 0 (0 - 11), nên cộng thêm 1
  let year = date.getFullYear();

  // Đảm bảo rằng ngày và tháng luôn có 2 chữ số
  let formattedDay = ("0" + day).slice(-2);
  let formattedMonth = ("0" + month).slice(-2);

  let formattedDate = formattedDay + "/" + formattedMonth + "/" + year;

  return formattedDate;
}
function convertToYYYYMMDD(dateString) {
  if (!isValidDate(dateString)) {
    return "";
  }

  let date = new Date(dateString);
  let day = date.getDate();
  let month = date.getMonth() + 1; // Tháng trong JavaScript bắt đầu từ 0 (0 - 11), nên cộng thêm 1
  let year = date.getFullYear();

  // Đảm bảo rằng ngày và tháng luôn có 2 chữ số
  let formattedDay = ("0" + day).slice(-2);
  let formattedMonth = ("0" + month).slice(-2);

  let formattedDate = year + "-" + formattedMonth + "-" + formattedDay;

  return formattedDate;
}

function removeEmptyFields(obj) {
  for (let key in obj) {
    if (obj[key] === "" || obj[key] === null) {
      delete obj[key];
    }
  }
  return obj;
}

/**
 * Mô tả: kiểm tra 1 mảng có chứa các phần tử của 1 mảng khác không (chuyển tham chiếu thành string)
 * created by : vdtien
 * created date: 06-06-2023
 * @param {type} param -
 * @returns true or false
 */
function areAllElementsInArray(subArr, priArr) {
  const result = subArr.every((element) => priArr.includes(element));
  return result;
}

/**
 * Mô tả: Lay ra cac phan tu o subArr khong o priArr
 * created by : vdtien
 * created date: 06-06-2023
 * @param {type} param -
 * @returns []
 */
function uniqueElementsArray(subArr, priArr) {
  const uniqueElements = subArr.filter((element) => !priArr.includes(element));
  return uniqueElements;
}

/**
 * Mô tả: xoa cac phan tu trong mang con o mang goc
 * created by : vdtien
 * created date: 06-06-2023
 * @param {type} param -
 * @returns
 */
function removeSubArrayElements(subArr, priArr) {
  const newArr = priArr.filter((element) => !subArr.includes(element));
  return newArr;
}

function containsOnlyNumbers(str) {
  return !isNaN(str);
}

function nestTreeData(items, parentId = null) {
  // Chuyển đổi dữ liệu thành định dạng mong muốn
  const result = [];

  for (const item of items) {
    if (item.ParentId === parentId) {
      const children = nestTreeData(items, item.AccountId);

      if (children.length > 0) {
        result.push({
          ...item,
          showChild: false,
          children: children,
        });
      } else {
        result.push({
          ...item,
          showChild: false,
        });
      }
    }
  }

  return result;
}

function converAccountFeature(accountFeature) {
  let featureName = "";
  switch (accountFeature) {
    case AccountFeature.debt:
      featureName = "Dư nợ";
      break;
    case AccountFeature.redundant:
      featureName = "Dư có";
      break;
    case AccountFeature.combine:
      featureName = "Lưỡng tính";
      break;
    case AccountFeature.noBalance:
      featureName = "Không có số dư";
    default:
      break;
  }
  return featureName;
}

function converStatusField(status) {
  let text = "";
  switch (status) {
    case Status.stopUsing:
      text = "Ngưng sử dụng";
      break;
    case Status.using:
      text = "Sử dụng";
      break;
    default:
      text = "Ngưng sử dụng";
      break;
  }
  return text;
}

export {
  removeDiacritics,
  converGender,
  removeEmptyFields,
  areAllElementsInArray,
  uniqueElementsArray,
  removeSubArrayElements,
  convertToDDMMYYYY,
  convertToYYYYMMDD,
  containsOnlyNumbers,
  nestTreeData,
  converAccountFeature,
  converStatusField,
};
