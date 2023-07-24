import axios from "../axios";
import BaseService from "./baseService";

class AccountService extends BaseService {
  endpoint = "Accounts";

  /**
   * Mô tả: lay danh sach account theo parent id
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  async getListAccountByParentId(parentId) {
    const res = await axios.get(
      `${this.getEndpoint()}/parent/${parentId}/childrens`
    );
    return res.data;
  }

  /**
   * Mô tả: lay danh sach account theo danh sach parent id
   * created by : vdtien
   * created date: 20-07-2023
   * @param {type} param -
   * @returns
   */
  async getListAccountByListParentId(listParentId) {
    const res = await axios.post(
      `${this.getEndpoint()}/parents/childrens`,
      listParentId
    );
    return res.data;
  }

  /**
   * Mô tả: lay danh dach tai khaon theo phan cap cay
   * created by : vdtien
   * created date: 24-07-2023
   * @param {type} param -
   * @returns
   */
  async getListTreeAccount(params) {
    const res = await axios.get(`${this.getEndpoint()}/filter-tree`, {
      params: {
        ...params,
      },
    });
    return res.data;
  }
}

export default new AccountService();
