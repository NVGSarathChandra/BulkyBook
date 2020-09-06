var dataTable;

$(document).ready(
    function() {
        loadProductTable();
    }
)

function loadProductTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAllProducts"
        },
        "columns": [
            { "data": "product.productTitle", "width": "15%" },
            { "data": "product.productISBN", "width": "15%" },
            { "data": "product.productAuthor", "width": "15%" },
            { "data": "product.price", "width": "15%" },
            { "data": "product.category.categoryName", "width": "15%" },

            {
                "data": "product.productID",
                "render": function (data) {
                    return `
                           <div class="text-center">
                                 <a href="/Admin/Product/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Admin/Product/DeleteProduct/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>    
                            `
                }, "width": "40%"

            },
        ]
    })
}

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete?",
        text: "You will not be able to restore the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }

            })
        }
    })
}