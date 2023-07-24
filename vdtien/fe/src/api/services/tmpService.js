import axios from "@/api/axios";

class EmployeesService {
  constructor() {
    this.endpoint = "Employees";
  }

  async getAllEmployees() {
    try {
      const res = await axios.get(this.endpoint);
      return res.data;
    } catch (error) {
      console.log(error);
      throw error;
    }
  }

  async createEmployee(employee) {
    try {
      const res = await axios.post(this.endpoint, employee);
      return res.data;
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
  async updateEmployee(employee) {
    try {
      const res = await axios.put(`${this.endpoint}/${employee.id}`, employee);
      return res.data;
    } catch (error) {
      throw error;
    }
  }
  async deleteEmployee(id) {
    try {
      const res = await axios.delete(`${this.endpoint}/${id}`);
      return res.data;
    } catch (error) {
      throw error;
    }
  }
  async findEmployeeByEmployeeCode(employeeCode) {
    try {
      const res = await axios.get(
        `${this.endpoint}/employeeCode/${employeeCode}`
      );
      return res.data;
    } catch (error) {
      throw error;
    }
  }
}
export default new EmployeesService();
