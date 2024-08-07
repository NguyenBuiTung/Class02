const formatDateTime = (createdAt) => {
    const date = new Date(createdAt);
    const formattedDateTime = `${date.getSeconds()}`;
  
    return formattedDateTime;
  };
  
  export default formatDateTime;
  