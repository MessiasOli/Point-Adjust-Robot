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
        path: "*",
        redirect: "/",
      },
      {
        path: "AdjustWorkShift",
        name: "AdjustWorkShift",
        component: () =>
          import("src/pages/WorkShift/AdjustWorkshift/AdjustWorkshift.vue"),
      },
      {
        path: "CoverWorkShift",
        name: "CoverWorkShift",
        component: () => import("pages/WorkShift/CoverWorkShift/CoverWorkShift.vue"),
      },
      {
        path: "Absence",
        name: "Absence",
        component: () => import("pages/WorkShift/Absence/Absence.vue"),
      },
      {
        path: "Settings",
        name: "Settings",
        component: () => import("pages/Settings/Settings.vue"),
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
