var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#coverTypeTblData").DataTable({
        "ajax": {
            "url": "/Admin/CoverType/GetAllCoverTypes"
        },
        "columns": [
            {
                "data": "coverTypeName", "width": "60%"
            },
            {
                "data": "coverTypeId",
                "render": function (data) {
                    return `
                             <div class="text-center">
                                 <a href="/Admin/CoverType/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Admin/CoverType/DeleteCoverType/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>    
                            `
                }
            }
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