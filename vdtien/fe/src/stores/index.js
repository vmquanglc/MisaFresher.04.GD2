import { createStore } from "vuex";
import employee from "./employee";
import global from "./global";
import account from "./account";
const store = createStore({
  modules: {
    global,
    employee,
    account,
  },
});
export default store;
