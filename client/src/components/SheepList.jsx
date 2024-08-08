import axios from "axios";
import { useState } from "react";
import { useEffect } from "react";
import {
  Col,
  Flex,
  Form,
  message,
  Popconfirm,
  Row,
  Table,
  Tooltip,
} from "antd";
import AddSheep from "./AddSheep";
import { DeleteOutlined, SyncOutlined } from "@ant-design/icons";
import EditOrder from "./EditOrder";
import SheepRun from "./SheepRun";
export default function SheepList() {
  const [form] = Form.useForm();
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState([]);
  // const [editingKey, setEditingKey] = useState("");
  // const isEditing = (record) => record.orderId === editingKey;
  // const edit = (record) => {
  //   form.setFieldsValue({
  //     ...record,
  //   });
  //   console.log(record)
  //   setEditingKey(record.orderId);
  // };
  // const cancel = () => {
  //   setEditingKey("");
  // };
  const token = localStorage.getItem("token");
  useEffect(() => {
    const fetchOrders = async () => {
      try {
        // Lấy token từ localStorage

        // Gửi yêu cầu GET với Bearer token trong header
        const response = await axios.get(
          "http://localhost:5218/api/Order/get-orders",
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );
        console.log(response);
        // Cập nhật dữ liệu nhận được
        setOrders(response.data);
      } catch (error) {
        if (error.response && error.response.status === 401) {
          // Xử lý khi token không hợp lệ (có thể redirect đến trang đăng nhập)
          console.error("Unauthorized: Please log in again.");
        } else {
          console.error("Error fetching data:", error);
        }
      } finally {
        setLoading(false); // Đặt loading thành false khi yêu cầu hoàn tất
      }
    };

    fetchOrders();
  }, []);
  useEffect(() => {
    const dataNew = orders?.map((items, index) => {
      return {
        key: index,
        orderId: items.orderId,
        orderQuantity: items?.orderQuantity,
      };
    });
    setData(dataNew);
  }, [orders]);
  const handleDelete = async (record) => {
    try {
      const response = await axios.delete(
        `http://localhost:5218/api/Order/delete-order/${record.orderId}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      setOrders(response.data);
      message.success("Xóa đơn hàng thành công");
    } catch (error) {
      // Xử lý lỗi trong quá trình xóa
      message.error("Có lỗi xảy ra");
    }
  };
  // const save = async (id) => {
  //   try {
  //     const row = await form.validateFields();
  //     const newData = [...orders];
  //     const index = newData.findIndex((item) => id === item.orderId);
  //     if (index > -1) {
  //       const item = newData[index];
  //       newData.splice(index, 1, {
  //         ...item,
  //         ...row,
  //       });
  //       // console.log(newData[index]);
  //       const response = await axios.put(
  //         `http://localhost:5218/api/Order/update-order/${id}`,
  //         newData[index],
  //         {
  //           headers: {
  //             "Content-Type": "application/json",
  //           },
  //         }
  //       );
  //       setData(response.data);
  //       message.success("Cập nhật đơn hàng thành công");
  //       setEditingKey("");
  //     } else {
  //       setEditingKey("");
  //     }
  //   } catch (error) {
  //     message.error("Có lỗi xảy ra ");
  //   }
  // };
  // const [sheepList, setSheepList] = useState([]);
  const [currentSheep, setCurrentSheep] = useState(null);
  const [loadRun, setLoadRun] = useState(false);  
  const [loadingTran, setLoadingTran] = useState({});
  const [progress, setProgress] = useState(0); // For the progress bar
  const [intervalId, setIntervalId] = useState(null); // To manage the interval
  const transition = async (id) => {
    setLoadRun(true);
    setLoadingTran((prevLoadingTran) => ({
      ...prevLoadingTran,
      [id]: true,
    }));
    setProgress(0); // Reset progress bar
    setCurrentSheep(null); // Clear previous sheep info
    try {
      const response = await axios.post(
        `http://localhost:5218/api/Order/process-transaction/${id}`,
        {
          orderId: id,
        },
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );
      const data = response.data;

      // Update progress bar with an interval
      let currentIndex = 0;
      const interval = setInterval(() => {
        if (currentIndex < data.length) {
          setCurrentSheep(data[currentIndex]);
          setProgress(((currentIndex + 1) / data.length) * 100);
          currentIndex++;
        } else {
          clearInterval(interval);
          setLoadRun(false);
          setLoadingTran((prevLoadingTran) => ({
            ...prevLoadingTran,
            [id]: false,
          }));
        }
      }, 1000); // Update every second

      setIntervalId(interval);
    } catch (error) {
      console.error("Error processing transaction:", error);
      alert("Failed to process transaction");
      setLoadRun(false);
      setLoadingTran((prevLoadingTran) => ({
        ...prevLoadingTran,
        [id]: false,
      }));
      clearInterval(intervalId);
    }
  };
  useEffect(() => {
    // Clean up interval on unmount
    return () => clearInterval(intervalId);
  }, [intervalId]);
  // console.log(currentSheep)
  const columns = [
    {
      title: "Id Đơn hàng",
      dataIndex: "orderId",
    },
    {
      title: "Số lượng",
      dataIndex: "orderQuantity",
      editable: true,
    },
    {
      title: "Thao tác",
      dataIndex: "operation",
      align: "center",
      width: "12.5%",
      render: (_, record) => {
        // const editable = isEditing(record);
        return (
          <Flex align="center" gap={10} justify="space-evenly">
            <Tooltip placement="top" title="Xóa đơn hàng">
              <Popconfirm
                placement="leftTop"
                title="Bạn có chắc chắn?"
                onConfirm={() => handleDelete(record)}
                okText="Xóa"
                cancelText="Hủy"
              >
                <DeleteOutlined style={{ color: "#f5222d", fontSize: 20 }} />
              </Popconfirm>
            </Tooltip>
            <Tooltip placement="top" title="Giao dịch">
              <SyncOutlined
                onClick={() => {
                  transition(record.orderId);
                }}
                spin={loadingTran[record.orderId]}
                style={{ color: "blue", fontSize: 20 }}
              />
            </Tooltip>
          </Flex>
        );
      },
    },
  ];
  const mergedColumns = columns.map((col) => {
    if (!col.editable) {
      return col;
    }
    return {
      ...col,
      onCell: (record) => ({
        record,
        dataIndex: col.dataIndex,
        title: col.title,
        // editing: isEditing(record),
      }),
    };
  });
  return (
    <div>
      <h2>Danh sách đơn đặt hàng</h2>
      <AddSheep setOrders={setOrders} />
      <Form form={form} component={false}>
        <Row gutter={[16, 16]}>
          <Col span={12}>
            <Table
              components={{
                body: {
                  cell: (cellProps) => <EditOrder {...cellProps} />,
                },
              }}
              loading={loading}
              columns={mergedColumns}
              dataSource={data}
              pagination={{
                position: ["bottomCenter"], // Đặt phân trang ở vị trí giữa dưới cùng
              }}
            />
          </Col>
          <Col span={12}>
            <SheepRun
              loadRun={loadRun}
              currentSheep={currentSheep}
              progress={progress}
            />
          </Col>
        </Row>
      </Form>
    </div>
  );
}
