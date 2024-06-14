
// // src/DatasourceGrid.js
// import React, { useEffect, useState } from "react";
// import {
//   DataGrid,
//   Column,
//   Editing,
//   ColumnChooser,
//   ColumnFixing,
//   Grouping,
//   GroupPanel,
//   Pager,
//   Paging,
//   SearchPanel,
//   Sorting,
//   Summary,
// } from "devextreme-react/data-grid";
// import { getDatasourceDetails, addValues } from "../../../apiService";
// import "./DatasourceGrid.css"; // Import custom CSS file
// const DatasourceGrid = ({ datasourceId }) => {
//   const [columns, setColumns] = useState([]);
//   const [values, setValues] = useState([]);
//   const [nextRowId, setNextRowId] = useState(1); // Initialize unique identifier
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState(null);

//   useEffect(() => {
//     const fetchDatasourceDetails = async () => {
//       try {
//         const data = await getDatasourceDetails(datasourceId);
//         console.log("Columns from API:", data.columns);
//         setColumns(data.columns);
//         setValues(data.values);
//         setLoading(false);
//       } catch (error) {
//         console.error("Error fetching data:", error);
//         setError(error.message || "An error occurred while fetching data.");
//         setLoading(false);
//       }
//     };

//     if (datasourceId) {
//       setLoading(true);
//       fetchDatasourceDetails();
//     }
//   }, [datasourceId]);

//   const onRowInserted = async (e) => {
//     const newValue = e.data;
//     const formattedValues = [newValue];

//     try {
//       await addValues(datasourceId, formattedValues);
//       const updatedValues = await getDatasourceDetails(datasourceId);
//       setValues(updatedValues.values);
//     } catch (error) {
//       console.error("Error adding value:", error);
//       setError(error.message || "An error occurred while adding value.");
//     }
//   };

//   if (loading) {
//     return <div>Loading...</div>;
//   }

//   if (error) {
//     return <div>Error: {error}</div>;
//   }

//   return (
//     <div>
//       <DataGrid
//         dataSource={values.map((row, index) => ({
//           ...row,
//           __rowId: index + 1,
//         }))}
//         onRowInserted={onRowInserted}
//         allowAdding={true}
//         showBorders={true}
//         keyExpr="__rowId" // Using custom __rowId as the keyExpr
//         allowColumnReordering={true}
//         allowColumnResizing={true}
//         columnAutoWidth={true}
//         wordWrapEnabled={true}
//       >
//         <Editing mode="row" allowAdding={true} />
//         <ColumnChooser enabled={true} />
//         <ColumnFixing enabled={true} />
//         <Grouping />
//         <GroupPanel visible={true} />
//         <Pager
//           showPageSizeSelector={true}
//           allowedPageSizes={[5, 10, 20]}
//           showInfo={true}
//         />
//         <Paging defaultPageSize={10} />
//         <SearchPanel visible={true} />
//         <Sorting mode="multiple" />

//         {columns.map((col) => (
//           <Column
//             key={col.userFriendlyName}
//             dataField={col.columnName}
//             caption={col.userFriendlyName}
//             dataType={col.dataType.toLowerCase()}
//           />
//         ))}
//       </DataGrid>
//     </div>
//   );
// };

// export default DatasourceGrid;

// import React, { useEffect, useState } from "react";
// import {
//   DataGrid,
//   Column,
//   Editing,
//   ColumnChooser,
//   ColumnFixing,
//   Grouping,
//   GroupPanel,
//   Pager,
//   Paging,
//   SearchPanel,
//   Sorting,
//   Summary,
// } from "devextreme-react/data-grid";
// import { getDatasourceDetails, updateValues } from "../../../apiService";
// import "./DatasourceGrid.css"; // Import custom CSS file
// import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
// import { faSpinner,faCheck,faTimes} from '@fortawesome/free-solid-svg-icons';




// const DatasourceGrid = ({ datasourceId }) => {
//   const [columns, setColumns] = useState([]);
//   const [values, setValues] = useState([]);
//   const [loading, setLoading] = useState(true);
//   const [error, setError] = useState(null);

//   useEffect(() => {
//     const fetchDatasourceDetails = async () => {
//       try {
//         const data = await getDatasourceDetails(datasourceId);
//         console.log("Columns from API:", data.columns);
//         console.log("Datasource",data);
//         setColumns(data.columns);
//         setValues(data.values);
//         setLoading(false);
//       } catch (error) {
//         console.error("Error fetching data:", error);
//         setError(error.message || "An error occurred while fetching data.");
//         setLoading(false);
//       }
//     };

//     if (datasourceId) {
//       setLoading(true);
//       fetchDatasourceDetails();
//     }
//   }, [datasourceId]);

//   const onRowUpdated = async (e) => {
//     const updatedValue = e.data;
//     // Ensure __rowId is present and included
//     if (!updatedValue.__rowId) {
//       console.error("Missing __rowId in updated data");
//       return;
//     }

//     const formattedValues = [{ ...updatedValue, __rowId: updatedValue.__rowId.toString() }];

//     try {
//       await updateValues(datasourceId, formattedValues);
//       const updatedValues = await getDatasourceDetails(datasourceId);
//       setValues(updatedValues.values);
//     } catch (error) {
//       console.error("Error updating value:", error);
//       setError(error.message || "An error occurred while updating value.");
//     }
//   };

//   if (loading) {
//   return (
//     <div className="loading-spinner">
//       <FontAwesomeIcon icon={faSpinner} spin />
//       <p>Loading data, please wait...</p>
//     </div>
//   );
// }

//   if (error) {
//     return <div>Error: {error}</div>;
//   }

//   return (
//     <div>
//       <DataGrid
//         dataSource={values.map((row, index) => ({
//           ...row,
//           __rowId: index + 1,
//         }))}
//         onRowUpdated={onRowUpdated}
//         allowUpdating={true}
//         showBorders={true}
//         keyExpr="__rowId" // Using custom __rowId as the keyExpr
//         allowColumnReordering={true}
//         allowColumnResizing={true}
//         columnAutoWidth={true}
//         wordWrapEnabled={true}
//       >
//         <Editing mode="row" allowUpdating={true} />
//         <ColumnChooser enabled={true} />
//         <ColumnFixing enabled={true} />
//         <Grouping />
//         <GroupPanel visible={true} />
//         <Pager
//           showPageSizeSelector={true}
//           allowedPageSizes={[5, 10, 20]}
//           showInfo={true}
//         />
//         <Paging defaultPageSize={10} />
//         <SearchPanel visible={true} />
//         <Sorting mode="multiple" />

//         {columns.map((col) => (
//           <Column
//             key={col.userFriendlyName}
//             dataField={col.columnName}
//             caption={col.userFriendlyName}
//             dataType={col.dataType.toLowerCase()}
//             allowEditing={col.isEditable}
//             cellRender={(cellInfo) => {
//               const value = cellInfo.value;
//               if (value instanceof Date) {
//                 return value.toLocaleDateString();
//               } else if (col.dataType === 'BIT') {
//                 return value? <FontAwesomeIcon icon={faCheck} /> : <FontAwesomeIcon icon={faTimes} />;
//               }
//               return value;
//             }}
//           />
//         ))}
//       </DataGrid>
//     </div>
//   );
// };

// export default DatasourceGrid;


import React, { useEffect, useState } from "react";
import {
  DataGrid,
  Column,
  Editing,
  ColumnChooser,
  ColumnFixing,
  Grouping,
  GroupPanel,
  Pager,
  Paging,
  SearchPanel,
  Sorting,
  Summary,
} from "devextreme-react/data-grid";
import { getDatasourceDetails, updateValues } from "../../../apiService";
import "./DatasourceGrid.css"; // Import custom CSS file
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faSpinner, faCheck, faTimes } from '@fortawesome/free-solid-svg-icons';
import { toast, ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';

const DatasourceGrid = ({ datasourceId }) => {
  const [columns, setColumns] = useState([]);
  const [values, setValues] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchDatasourceDetails = async () => {
      try {
        const data = await getDatasourceDetails(datasourceId);
        console.log("Columns from API:", data.columns);
        console.log("Datasource", data);
        setColumns(data.columns);
        setValues(data.values);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching data:", error);
        setError(error.message || "An error occurred while fetching data.");
        setLoading(false);
        toast.error(error.message || "An error occurred while fetching data.");
      }
    };

    if (datasourceId) {
      setLoading(true);
      fetchDatasourceDetails();
    }
  }, [datasourceId]);

  const onRowUpdated = async (e) => {
    const updatedValue = e.data;
    // Ensure __rowId is present and included
    if (!updatedValue.__rowId) {
      console.error("Missing __rowId in updated data");
      toast.error("Missing __rowId in updated data");
      return;
    }

    const formattedValues = [{ ...updatedValue, __rowId: updatedValue.__rowId.toString() }];

    try {
      await updateValues(datasourceId, formattedValues);
      const updatedValues = await getDatasourceDetails(datasourceId);
      setValues(updatedValues.values);
      toast.success("Row updated successfully");
    } catch (error) {
      console.error("Error updating value:", error);
      setError(error.message || "An error occurred while updating value.");
      toast.error(error.message || "An error occurred while updating value.");
    }
  };

  if (loading) {
    return (
      <div className="loading-spinner">
        <FontAwesomeIcon icon={faSpinner} spin />
        <p>Loading data, please wait...</p>
      </div>
    );
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <div>
      <ToastContainer />
      <DataGrid
        dataSource={values.map((row, index) => ({
          ...row,
          __rowId: index + 1,
        }))}
        onRowUpdated={onRowUpdated}
        allowUpdating={true}
        showBorders={true}
        keyExpr="__rowId" // Using custom __rowId as the keyExpr
        allowColumnReordering={true}
        allowColumnResizing={true}
        columnAutoWidth={true}
        wordWrapEnabled={true}
      >
        <Editing mode="row" allowUpdating={true} />
        <ColumnChooser enabled={true} />
        <ColumnFixing enabled={true} />
        <Grouping />
        <GroupPanel visible={true} />
        <Pager
          showPageSizeSelector={true}
          allowedPageSizes={[5, 10, 20]}
          showInfo={true}
        />
        <Paging defaultPageSize={10} />
        <SearchPanel visible={true} />
        <Sorting mode="multiple" />

        {columns.map((col) => (
          <Column
            key={col.userFriendlyName}
            dataField={col.columnName}
            caption={col.userFriendlyName}
            dataType={col.dataType.toLowerCase()}
            allowEditing={col.isEditable}
            cellRender={(cellInfo) => {
              const value = cellInfo.value;
              if (value instanceof Date) {
                return value.toLocaleDateString();
              } else if (col.dataType === 'BIT') {
                return value ? <FontAwesomeIcon icon={faCheck} /> : <FontAwesomeIcon icon={faTimes} />;
              }
              return value;
            }}
          />
        ))}
      </DataGrid>
    </div>
  );
};

export default DatasourceGrid;
