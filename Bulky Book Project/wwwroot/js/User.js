var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAllUsers"
        },
        "columns": [
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "organization.organizationName", "width": "15%" },
            { "data": "role", "width": "15%" },

            {
                "data": { id: "id", lockoutEnd: "lockoutEnd" },

                "render": function (data) {
                    var currentDate = new Date().getTime();
                    var lockEndDate = new Date(data.lockoutEnd).getTime();
                    if (lockEndDate > currentDate) {
                        return `
                            <div class="text-center">
                                <a onclick=LockOrUnLockUser('${data.id}') class="btn btn-danger text-white" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock-open"></i>Unlock
                                </a>
                            </div>    
                                `
                    }
                    else {
                        return `
                           <div class="text-center">
                                 <a onclick=LockOrUnLockUser('${data.id}') class="btn btn-success text-white" style="cursor:pointer; width:100px">
                                    <i class="fas fa-lock"></i> Lock
                                </a>
                               
                            </div>    
                            `
                    }
                }, "width": "25%"

            },

        ]
    })
}

function LockOrUnLockUser(id) {
   
            $.ajax({
                url: 'User/LockOrUnlockUser',
                data: JSON.stringify(id),
                contentType:'application/json',
                type: "POST",
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
  