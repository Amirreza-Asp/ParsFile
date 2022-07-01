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
            "url": "/Admin/Category/GetAllCategories",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "50%", "class": "text-center" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class = "text-center">
                                      <a href="/Admin/Category/Edit/${data}" class  = "btn btn-sm btn-info w-25 text-white">
                                                <i class="far fa-edit"></i></a>
                                      &nbsp;
                                      <a onclick=Delete("/Admin/Category/Remove/${data}") class = "btn btn-sm btn-danger w-25 text-white">
                                                <i class="far fa-trash-alt"></i></a>
                                </div>`;
                }, "width": "50%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "ایتم مورد نظر حذف شود ؟",
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
            });
        }
    });
}