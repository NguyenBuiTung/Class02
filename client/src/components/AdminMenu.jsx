import {
  HomeOutlined,
  ShoppingCartOutlined,
} from "@ant-design/icons";

export const menuItems = [
  {
    key: "home",
    icon: <HomeOutlined />,
    label: "Báo cáo",
    path: "/",
    children: [
        {
          key: "report-time",
          label: "Thông tin báo cáo",
          path: "/",
        },
        
      ],
  },
  {
    key: "order",
    icon: <ShoppingCartOutlined />,
    label: "Đơn hàng",
    path: "/",
    children: [
      {
        key: "list-order",
        label: "Danh sách đơn hàng",
        path: "/order",
      },
      
    ],
  },
];
export const rootSubmenuKeys = ["home", "customer", "category"];
