import axios from "../axios";

class BaseService {
  constructor() {}
  endpoint = "";

  /**
   * Mô tả: trả về endpoint
   * created by : vdtien
   * created date: 02-06-2023
   */
  getEndpoint() {
    return this.endpoint;
  }

  /**
   * Mô tả: láy danh sách tất cả bản ghi
   * created by : vdtien
   * created date: 02-06-2023
   */
  async getAll() {
    const res = await axios.get(this.getEndpoint());
    return res.data;
  }

  /**
   * Mô tả: lấy danh sách các bản ghi theo paging và filter
   * created by : vdtien
   * created date: 02-06-2023
   */
  async getList(params) {
    const res = await axios.get(`${this.getEndpoint()}/filter`, {
      params: {
        ...params,
      },
    });
    return res.data;
  }

  /**
   * Mô tả: lấy bản ghi theo phân trang và filter
   * created by : vdtien
   * created date: 05-06-2023
   * @param {type} param -
   * @returns
   */
  async getById(id) {
    const res = await axios.get(`${this.getEndpoint()}/${id}`);
    return res.data;
  }

  /**
   * Mô tả: thêm mới bản ghi
   * created by : vdtien
   * created date: 02-06-2023
   * @param {object} createBody - thông tin bản ghi
   * @returns
   */
  async createRecord(createBody) {
    const res = await axios.post(`${this.getEndpoint()}`, {
      ...createBody,
    });
    return res.data;
  }

  /**
   * Mô tả: cập nhật record theo id
   * created by : vdtien
   * created date: 02-06-2023
   * @param {object} updateBody -
   * @param {string} id -
   * @returns bản ghi được cập nhật
   */
  async updateRecord(updateBody, id) {
    const res = await axios.put(`${this.getEndpoint()}/${id}`, {
      ...updateBody,
    });
    return res.data;
  }

  /**
   * Mô tả: xóa bản ghi theo id
   * created by : vdtien
   * created date: 02-06-2023
   * @param {string} id -
   * @returns
   */
  async deleteRecord(id) {
    const res = await axios.delete(`${this.getEndpoint()}/${id}`);
    return res.data;
  }

  async deleleRecordMulti(listId) {
    const res = await axios.post(`${this.getEndpoint()}/delete-multi`, listId);
    return res.data;
  }
}
export default BaseService;
