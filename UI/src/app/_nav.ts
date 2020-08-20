import { INavData } from "@coreui/angular";

export const navItems: INavData[] = [
  {
    name: "Dashboard",
    url: "/dashboard",
    icon: "icon-speedometer",
    badge: {
      variant: "info",
      text: "NEW",
    },
  },
  {
    divider: true,
  },
  {
    title: true,
    name: "Extras",
  },
  {
    name: "Pages",
    url: "/pages",
    icon: "icon-star",
    children: [
      {
        name: "Error 404",
        url: "/404",
        icon: "icon-star",
      },
      {
        name: "Error 500",
        url: "/500",
        icon: "icon-star",
      },
    ],
  },
  {
    name: "Disabled",
    url: "/dashboard",
    icon: "icon-ban",
    badge: {
      variant: "secondary",
      text: "NEW",
    },
    attributes: { disabled: true },
  },
];
