import { createRouter, createWebHistory } from "vue-router";

// 2. Define some routes
// Each route should map to a component.
// We'll talk about nested routes later.
const routes = [
  {
    path: "/DI/employee",
    name: "Employee",
    meta: {
      layout: "default",
    },
    component: () =>
      import(
        /* webpackChunkName: "home" */ "@/views/employee/EmployeeList/EmployeeList.vue"
      ),
  },
  {
    path: "/DI",
    name: "Directory",
    meta: {
      layout: "default",
    },
    component: () =>
      import(/* webpackChunkName: "home" */ "@/views/DirectoryPage.vue"),
  },
  {
    path: "/DI/account-system",
    name: "DIAccountSystem",
    meta: {
      layout: "default",
    },
    component: () =>
      import(
        /* webpackChunkName: "home" */ "@/views/account/AccountList/AccountList.vue"
      ),
  },
  {
    path: "/fake",
    name: "Fake",
    meta: {
      layout: "default",
    },
    component: () => import("@/views/FakePage.vue"),
  },
  {
    path: "/components",
    name: "Components",
    component: () =>
      import(/* webpackChunkName: "home" */ "@/views/ComponentPage.vue"),
  },
  {
    path: "/:pathMatch(.*)*",
    name: "NotFound",
    meta: {
      layout: "notfound",
    },
    redirect: "/DI",
    // component: () => <div>NotFound</div>,
  },
];

// 3. Create the router instance and pass the `routes` option
// You can pass in additional options here, but let's
// keep it simple for now.
const router = createRouter({
  // 4. Provide the history implementation to use. We are using the hash history for simplicity here.
  history: createWebHistory(),
  routes, // short for `routes: routes`
});

export default router;
