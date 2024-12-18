const baseURL = `https://k8smaster.elmisoftware.com/api/search/DataType/Search`;
const action_codice = "value";
const action_name = "lookupValue_like";

function createNewObject(item) {
  const newObj = {
    // If values from call changes, here you can change the key name
    name: item.lookupValue,
    codice: item.value,
    id: item.id,
  };

  for (let key in item) {
    if (Array.isArray(item[key])) {
      newObj.children = createNewArray(item[key]);
    }
  }

  return newObj;
}

function createNewArray(data) {
  if (Array.isArray(data)) {
    return data.map((item) => createNewObject(item));
  }
  return [];
}

export const fetchLookupData = async ({
  searchTerm,
  token,
  instanceId,
  maxResults = 10,
}) => {
  if (!searchTerm) return;
  const response = await fetch(
    `${baseURL}/${instanceId}/${maxResults}/${searchTerm}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Methods": "GET",
        "Access-Control-Allow-Credentials": true,
        Accept: "application/json",
        Authorization: `Bearer: ${token}`,
      },
      data: {
        instanceId,
      },
      // Se post contiene un body
      //body: JSON.stringify({ searchTerm }),
    }
  )
    .then((res) => {
      if (res.ok) {
        return res.json();
      } else {
        throw new Error("Something went wrong");
      }
    })
    .then((data) => {
      return createNewArray(data);
    })
    .catch((error) => {
      return error;
    });
  return response;
};
