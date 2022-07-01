var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $("#tblData").dataTable({
        "language": {
            "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Persian.json"
        },
        "ajax": {
            "url": "/Admin/File/GetAllFiles",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
        
            { "data": "name", "width": "15%", "class": "text-center" },
            { "data" : "username", "width" : "15%", "class" : "text-center" },
            { "data": "createTime", "width": "10%", "class": "text-center" },
            { "data": "price", "width": "10%", "class": "text-center" },
            { "data": "downloads", "width": "10%", "class": "text-center" },
            {
                "data": "accepted",
                "render": function (accepted) {
                    if (accepted) {
                        return `<i class="fa fa-check text-success"></i>`;
                    }
                    return `<i class="fa fa-close text-danger"></i>`;
                }, "width": "10%", "class": "text-center" },
            {
                "data": "deleted",
                "render": function (deleted) {
                    if (deleted) {
                        return `<i class="fa fa-check text-danger"></i>`;
                    }
                    return `<i class="fa fa-close text-success"></i>`;
                }, "width": "13%", "class": "text-center"
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class = "text-center">
                                        <a href="/Admin/File/Show/${data}" class="btn btn-info">
                                            <i class="fa fa-eye"></i>
                                        </a>
                                        <a href="/Admin/File/Accept/${data}" class="btn btn-success">
                                            <i class="fa fa-check"></i>  
                                        </a>
                                        <button onClick=Delete('/Admin/File/AdminRemove/${data}') class="btn btn-danger">
                                            <i class="fa fa-trash-alt text-white"></i>
                                        </button>
                                </div>`;
                }, "width": "17%"
            },
        ]
    });
}

function Delete(url) {
    swal({
        title: "فایل مورد نظر حذف شود؟",
        text: "امکان بازیابی اطلاعات وجود ندارد",
        icon: "error",
        buttons: true,
        dangerMode: true
    }).then(willDelete => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        $("#tblData").DataTable().ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}