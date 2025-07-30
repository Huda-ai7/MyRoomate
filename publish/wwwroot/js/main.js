// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// let selectedFile = [];
// // Write your JavaScript code.
// function perviewChange(input) {
//   if (input.files && input.files.length > 0) {
//     selectedFile = Array.from(input.files).slice(0, 4); // Limit to 4 images
//     updateImagePreviews();
//   }
// }
// function updateImagePreviews() {
//   const container = document.getElementById("imagePreviewContainer");
//   const previews = document.getElementById("preview");

//   if (selectedFile.length > 0) {
//     container.style.display = "block";
//     previews.innerHTML = "";

//     selectedFile.forEach((file, index) => {
//       const reader = new FileReader();
//       reader.onload = function (e) {
//         const previewItem = document.createElement("div");
//         previewItem.className = "preview-item";
//         previewItem.innerHTML = `
//           <img src="${e.target.result}" alt="Preview" width=100>
//           <button type="button" class="remove-image" onclick="removeImage(${index})">
//               <i class="fas fa-times"></i>
//             </button>
//           `;
//         previews.appendChild(previewItem);
//       };
//       reader.readAsDataURL(file);
//     });
//   } else {
//     container.style.display = "none";
//   }
// }
// function removeImage(index) {
//   selectedFile.splice(index, 1);
//   updateFileInput();
//   updateImagePreviews();
// }
// function updateFileInput() {
//   const input = document.querySelector('input[type="file"]');
//   const dt = new DataTransfer();

//   selectedFile.forEach((file) => {
//     dt.items.add(file);
//   });

//   input.files = dt.files;
// }

// document.getElementById("postForm").addEventListener("submit", function (e) {
//   e.preventDefault();

//   var formData = new FormData();
//   formData.append("Content", document.getElementById("content").value);

//   var files = document.getElementById("images").files;
//   for (var i = 0; i < files.length; i++) {
//     formData.append("Images", files[i]); // ✅ Must match controller param
//   }

//   send();
// });

// function send() {
//   $.ajax({
//     url: "/Post/Add_Post_Ajax",
//     type: "POST",
//     data: formData,
//     success: function (response) {
//       alert("Success");
//     },
//   });
// }

// asp.net True
let selectedFile = [];
function addImgFile(input) {
  const imgPerview = document.getElementById("imgPerview");
  const files = input.files;
  filesList = Array.from(files).slice(0, 4);
  filesList.forEach((file) => {
    // Check if file already exists
    const exists = selectedFile.some(
      (f) => f.name === file.name && f.size === file.size
    );
    if (!exists) {
      selectedFile.push[file];
    } else {
      alert(`${file.name} is already selected`);
      return selectedFile;
    }
  });
  selectedFile = [...selectedFile, ...filesList];
  imgPerview.textContent = "";
  imgPerview.textContent = `${selectedFile.length} files selected`;

  // Create a FormData object manually
  const formData = new FormData();
  formData.append(
    "Content",
    document.querySelector('textarea[name="Content"]').value
  );

  // Add all files individually
  selectedFile.forEach((file, index) => {
    console.log("Adding file to FormData:", file.name);
    formData.append("Images", file);
  });

  console.log(selectedFile);
}

// function confirmList() {
//   const formData = new FormData();
//   selectedFile.forEach((file) => {
//     formData.append("imgList", file); // Match this name with your C# controller
//   });

//   $.ajax({
//     url: "/UserController/UpdateImgeList",
//     type: "GET",
//     data: formData,
//     contentType: false, // Important
//     processData: false, // Important
//     success: function (response) {
//       console.log("Success:", response);
//       alert("List updated successfully!");
//     },
//     error: function (xhr, status, error) {
//       console.error("Error:", error);
//       alert("Error updating list");
//     },
//   });
// }
// fetch("/api/UserController/Add_Post", {
//   method: "POST",
//   headers: {
//     "Content-Type": "application/json",
//   },
//   body: JSON.stringify(selectedFile),
// })
//   .then((response) => response.json())
//   .then((data) => console.log(data));
