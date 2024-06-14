import React, { useState, useEffect } from 'react';
import axios from 'axios';
import DataGrid, { Column } from 'devextreme-react/data-grid';
import 'devextreme/dist/css/dx.light.css'; // Import DevExtreme CSS

const AuditTable = () => {
  const [auditEntries, setAuditEntries] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchAuditEntries = async () => {
      try {
        const response = await axios.get('https://localhost:7253/api/DataValues/Audit');
        setAuditEntries(response.data);
        setLoading(false);
      } catch (error) {
        console.error('Error fetching audit entries:', error);
        setError('An error occurred while fetching audit entries.');
        setLoading(false);
      }
    };

    fetchAuditEntries();
  }, []);

  return (
    <div>
      <h2>Audit Table</h2>
      <DataGrid
        dataSource={auditEntries}
        columnAutoWidth={true}
        showBorders={true}
        showRowLines={true}
        loading={loading}
        error={error}
      >
        {/* <Column dataField="columnId" caption="Column Id" />
        <Column dataField="rowId" caption="Row Id" /> */}
        <Column dataField="columnName" caption="Column Name" />
        <Column dataField="datasourceName" caption="Datasource Name" />
        <Column dataField="oldValue" caption="Old Value" />
        <Column dataField="newValue" caption="New Value" />
        <Column dataField="modifiedDate" caption="Modified Date" dataType="datetime" />
        <Column dataField="modifiedBy" caption="Modified By" />
      </DataGrid>
    </div>
  );
};

export default AuditTable;
