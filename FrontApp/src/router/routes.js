const routes = [
  {
    path: "/",
    component: () => import("layouts/MainLayout.vue"),
    children: [
      {
        path: "",
        name: "home",
        component: () => import("pages/IndexPage.vue"),
      },
      {
        path: "AdjustWorkShift",
        name: "AdjustWorkShift",
        component: () =>
          import("src/pages/AdjustWorkshift/AdjustWorkshift.vue"),
      },
      {
        path: "CoverWorkShift",
        name: "CoverWorkShift",
        component: () => import("pages/CoverWorkShift/CoverWorkShift.vue"),
      },
      {
        path: "Settings",
        name: "Settings",
        component: () => import("pages/Settings.vue"),
      },
    ],
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: "/:catchAll(.*)*",
    component: () => import("pages/ErrorNotFound.vue"),
  },
];

export default routes;
