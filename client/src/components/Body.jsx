import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons";
import { Button, Layout, Menu } from "antd";
import { useEffect, useState } from "react";
import { Link, Outlet, useLocation } from "react-router-dom";


import HeaderApp from "./HeaderApp";
import { menuItems, rootSubmenuKeys } from "./AdminMenu";
const { Header, Sider, Content } = Layout;

export default function Body() {
 
  const [collapsed, setCollapsed] = useState(false);
  const [openKeys, setOpenKeys] = useState(["home"]);
  const location = useLocation();
  const [activePath, setActivePath] = useState(location.pathname);
  useEffect(() => {
    setActivePath(location.pathname);
  }, [location.pathname]);
  // Function to find the matching menu key based on the current path
  const findActiveMenuKey = (items, path) => {
    for (const item of items) {
      if (item.path === path) {
        return item.key;
      }
      if (item.children) {
        const childKey = findActiveMenuKey(item.children, path);
        if (childKey) {
          return childKey;
        }
      }
    }
    return null;
  };

  const activeKey = findActiveMenuKey(menuItems, activePath);
  // console.log(activeKey);
  const onOpenChange = (keys) => {
    const latestOpenKey = keys.find((key) => openKeys.indexOf(key) === -1);

    // Nếu latestOpenKey không nằm trong rootSubmenuKeys, chỉ cập nhật openKeys
    if (rootSubmenuKeys.indexOf(latestOpenKey) === -1) {
      setOpenKeys(keys);
    } else {
      // Nếu latestOpenKey nằm trong rootSubmenuKeys, mở menu đó và đóng các menu khác
      setOpenKeys(latestOpenKey ? [latestOpenKey] : []);
    }
  };

  const headerStyle = {
    color: "#fff",
    height: "auto",
    lineHeight: "64px",
    background: "#fff",
    display: "flex",
    justifyContent: "space-between",
    alignItems: "center",
    boxShadow: "0 2px 8px rgba(0, 0, 0, 0.15)",
    width: "100%",
    zIndex: 1000,
  };

  return (
    <div className="body">
      <Layout>
        <Header style={headerStyle}>
         <HeaderApp/>
        </Header>
        <Layout style={{ backgroundColor: "#fff" }}>
          <Sider
            trigger={null}
            collapsible
            collapsed={collapsed}
            width={250}
            style={{
              backgroundColor: "#f5f5f5",
              overflowY: "auto",
              borderRadius: 4,
              minHeight: 800,
            }}
          >
            <Button
              type="text"
              icon={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
              onClick={() => setCollapsed(!collapsed)}
              style={{
                fontSize: "16px",
                width: 64,
                height: 64,
              }}
            />
            <div className="demo-logo-vertical" />
            <Menu
              selectedKeys={[activeKey]}
              style={{ backgroundColor: "#f5f5f5" }}
              mode="inline"
              inlineIndent={10}
              openKeys={openKeys}
              onOpenChange={onOpenChange}
            >
              {renderMenuItems(menuItems)}
            </Menu>
          </Sider>
          <Content
            style={{
              padding: 10,
            }}
          >
            <Outlet />
          </Content>
        </Layout>
      
      </Layout>
    </div>
  );
}

function renderMenuItems(items) {

  const hideForRoles = {
    WORKSHOP: ["branchname", "account", "quota"],
    CUSTOMER: [
      "home",
      "customer",
      "category",
      "sms",
      "personnel",
      "job",
      "maintenance",
      "branchname",
      "account",
      "quota",
      "car"
    ],
    ADMIN: [
      "home",
      "customer",
      "category",
      "sms",
      "personnel",
      "job",
      "maintenance",
      "car"
    ],
  };
//   const hiddenKeys = hideForRoles[] || [];
  return items
    .map((item) => {
      
      if (item.children) {
        const subMenuItems = renderMenuItems(item.children);
        if (subMenuItems.some((subItem) => subItem !== null)) {
          return (
            <Menu.SubMenu key={item.key} icon={item.icon} title={item.label}>
              {subMenuItems}
            </Menu.SubMenu>
          );
        }
      } else {
        return (
          <Menu.Item key={item.key} icon={item.icon}>
            <Link to={item.path}>{item.label}</Link>
          </Menu.Item>
        );
      }
      return null;
    })
    .filter((item) => item !== null);
}
