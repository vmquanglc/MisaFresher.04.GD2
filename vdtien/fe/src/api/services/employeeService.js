import axios from "../axios";
import BaseService from "./baseService";

class EmployeeService extends BaseService {
  endpoint = "Employees";

  /**
   * Mô tả: lấy mã nhân viên mới nhất
   * created by : vdtien
   * created date: 30-06-2023
   * @param {type} param -
   * @returns (string) mã nhân viên
   */
  async getNewEmployeecode() {
    const res = await axios.get(`${this.getEndpoint()}/newEmployeeCode`);
    return res.data;
  }

  /**
   * Mô tả: export file excel danh sach nhan vien
   * created by : vdtien
   * created date: 30-06-2023
   * @param {type} param -
   * @returns
   */

  async exportExcelEmployeeList(keySearch) {
    const res = await axios.get(`${this.getEndpoint()}/export-excel`, {
      params: {
        keySearch,
      },
      responseType: "blob",
    });
    return res.data;
  }
}

export default new EmployeeService();
