// import React, { useState, useEffect, useCallback } from "react";
// import axios from "axios";
// import DataGrid, {
//   Column,
//   Editing,
//   Popup,
//   Paging,
//   Scrolling,
//   Form,
// } from "devextreme-react/data-grid";
// import "devextreme/dist/css/dx.light.css";

// const DataGridComponent = ({ parentId }) => {
//   const [data, setData] = useState([]);
//   const [columns, setColumns] = useState([]);

//   const fetchData = useCallback(async () => {
//     try {
//       const result = await axios.get(`/api/value/${parentId}`);
//       console.log("result", result);
//       if (result.data.length > 0) {
//         const firstItem = result.data[0];
//         const editableColumns = firstItem.details
//           .filter((d) => d.isEditable)
//           .map((d) => d.columnName);
//         setColumns(editableColumns);
//         setData(
//           result.data.map((item) => {
//             const rowData = {};
//             editableColumns.forEach((col) => {
//               rowData[col] =
//                 item.values.find((val) => val.columnName === col)?.value || "";
//             });
//             return { ...item, ...rowData };
//           })
//         );
//       } else {
//         setColumns([]);
//         setData([]);
//       }
//     } catch (error) {
//       console.error("Error fetching data", error);
//       setColumns([]); // Fallback to empty array on error
//       setData([]); // Fallback to empty array on error
//     }
//   }, [parentId]);

//   useEffect(() => {
//     fetchData();
//   }, [fetchData]);

//   const handleInsert = async (e) => {
//     try {
//       await axios.post("/api/value", e.data);
//       fetchData();
//     } catch (error) {
//       console.error("Error inserting data", error);
//     }
//   };

//   const handleUpdate = async (e) => {
//     try {
//       await axios.put(`/api/value/${e.key}`, e.data);
//       fetchData();
//     } catch (error) {
//       console.error("Error updating data", error);
//     }
//   };

//   const handleDelete = async (e) => {
//     try {
//       await axios.delete(`/api/value/${e.key}`);
//       fetchData();
//     } catch (error) {
//       console.error("Error deleting data", error);
//     }
//   };

//   return (
//     <DataGrid
//       dataSource={data}
//       keyExpr="id"
//       showBorders={true}
//       onRowInserting={handleInsert}
//       onRowUpdating={handleUpdate}
//       onRowRemoving={handleDelete}
//     >
//       {columns &&
//         columns.map((col, index) => (
//           <Column key={index} dataField={col} caption={col} />
//         ))}
//       <Editing
//         mode="popup"
//         allowUpdating={true}
//         allowDeleting={true}
//         allowAdding={true}
//       >
//         <Popup title="Edit Row" showTitle={true} width={700} height={525} />
//         <Form></Form>
//       </Editing>
//       <Scrolling mode="virtual" />
//       <Paging defaultPageSize={10} />
//     </DataGrid>
//   );
// };

// export default DataGridComponent;
