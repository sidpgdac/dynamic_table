// // src/apiService.js

// import axios from "axios";

// const apiBaseUrl = "https://localhost:7253/api"; // Replace with your backend URL

// export const getDatasources = async () => {
//   const response = await axios.get(`${apiBaseUrl}/DataValues`);
//   return Array.isArray(response.data) ? response.data : [];
// };

// export const getDatasourceDetails = async (id) => {
//   const response = await axios.get(`${apiBaseUrl}/DataValues/${id}`);
//   return response.data;
// };

// export const addValues = async (parentId, values) => {
//   const response = await axios.post(
//     `${apiBaseUrl}/DataValues/${parentId}/values`,
//     values
//   );
//   return response.data;
// };


import axios from "axios";

const apiBaseUrl = "https://localhost:7253/api"; // Replace with your backend URL

export const getDatasources = async () => {
  const response = await axios.get(`${apiBaseUrl}/DataValues`);
  return Array.isArray(response.data) ? response.data : [];
};

export const getDatasourceDetails = async (id) => {
  const response = await axios.get(`${apiBaseUrl}/DataValues/${id}`);
  return response.data;
};

export const updateValues = async (parentId, values) => {
  try {
    const response = await axios.put(
      `${apiBaseUrl}/DataValues/${parentId}/values`,
      values
    );
    return response.data;
  } catch (error) {
    console.error("Update values error:", error.response?.data || error.message);
    throw error;
  }
};

