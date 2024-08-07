import { Button, Form, Input, message, Modal } from "antd";
import { useState } from "react";
import { PlusCircleOutlined } from "@ant-design/icons";
import axios from "axios";
export default function AddSheep({setOrders}) {
  const [form] = Form.useForm();
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isLoading, setIsLoading] = useState();
  const token = localStorage.getItem("token");
  const showModal = () => {
    setIsModalOpen(true);
  };
  const handleCancel = () => {
    setIsModalOpen(false);
  };
  const onFinish = async (values) => {
    setIsLoading(true);
    try {
      const response = await axios.post(
        `http://localhost:5218/api/Order/create-order`,
        values,
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );
      handleCancel();
      setOrders(response.data);
      setIsLoading(false);

      message.success("Thêm thành công");
    } catch (error) {
      setIsLoading(false);
      console.error("There was a problem with the fetch operation:", error);
    }
  };

  return (
    <div>
      <Button style={{margin:"10px 0"}} type="primary" onClick={showModal} icon={<PlusCircleOutlined />}>
        Thêm mới
      </Button>
      <Modal
        title="Đặt đơn hàng"
        open={isModalOpen}
        footer={null}
        // width={650}
        onCancel={handleCancel}
      >
        <Form
          name="addStaff"
          onFinish={onFinish}
          layout="vertical"
          requiredMark=""
          form={form}
        >
          <Form.Item
            label="Số lượng"
            name="orderQuantity"
            rules={[
              {
                required: true,
                message: "Vui lòng nhập số lượng",
              },
            ]}
          >
            <Input placeholder="Số lượng" type="number" min="0" />
          </Form.Item>

          <Form.Item>
            <Button
              type="primary"
              htmlType="submit"
              loading={isLoading}
              icon={<PlusCircleOutlined />}
            >
              Đặt hàng
            </Button>
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
}
