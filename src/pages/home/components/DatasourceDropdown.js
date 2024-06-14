// src/DatasourceDropdown.js

import React, { useEffect, useState } from "react";
import { SelectBox } from "devextreme-react/select-box";
import { getDatasources } from "../../../apiService";
import "./../home.css";
const DatasourceDropdown = ({ onDatasourceChange }) => {
  const [datasources, setDatasources] = useState([]);

  useEffect(() => {
    const fetchDatasources = async () => {
      const data = await getDatasources();
      setDatasources(data);
    };
    fetchDatasources();
  }, []);

  return (
    <SelectBox
      items={datasources}
      valueExpr="id"
      displayExpr="datasourceName"
      onValueChanged={(e) => onDatasourceChange(e.value)}
      placeholder="Select a Datasource"
    />
  );
};
export default DatasourceDropdown;
