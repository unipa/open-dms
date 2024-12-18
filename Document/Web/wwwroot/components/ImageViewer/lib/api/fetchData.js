export const getImageInfo = async (URL, password, documentId, imageId) => {
  const response = await fetch(
      `${URL}?documentId=${documentId}&imageId=${imageId}&Password=${password}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
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
      return data;
    })
    .catch((error) => {
      return error;
    });

  return response;
};

export const getPage = async (URL, documentId, imageId, contentId, pageIndex, password) => {
  const response = await fetch(
      `${URL}?documentId=${documentId}&imageId=${imageId}&PageIndex=${pageIndex}&Password=${password}`,
    {
      method: "GET",
    }
  );

  if (response.ok) {
    const blob = await response.blob();
    return blob;
  } else {
    throw new Error("Something went wrong");
  }
};
