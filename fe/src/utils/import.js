import { defineAsyncComponent } from "vue";

export function registerGlobalComponents(app) {
  // app.component(
  //   "auth-layout",
  //   defineAsyncComponent(() => import("@/layouts/auth-layout"))
  // );
  app.component(
    "DefaultLayout",
    defineAsyncComponent(() =>
      import("@/layouts/DefaultLayout/DefaultLayout.vue")
    )
  );
}
