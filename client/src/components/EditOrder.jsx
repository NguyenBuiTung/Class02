
import { Form, Input, InputNumber } from 'antd';

export default function EditOrder({
  editing,
  dataIndex,
  title,
  children, 
  ...restProps
}) {
  const renderCellContent = (dataIndex) => {
    if (dataIndex === 'orderQuantity') {
      
      return (
        <InputNumber
          placeholder="Số lượng"    
          type="number"
          min="0"
        />
      );
    }
    return <Input />;
  };

  return (
    <td {...restProps}>
      {editing ? (
        <Form.Item
          name={dataIndex}
          style={{ margin: 0 }}
          rules={[
            {
              required: true,
              message: `Vui lòng nhập ${title}!`,
            },
          ]}
        >
          {renderCellContent(dataIndex)}
        </Form.Item>
      ) : (
        children
      )}
    </td>
  );
}
