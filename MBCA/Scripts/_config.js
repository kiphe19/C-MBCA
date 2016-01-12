var editor,
    unitEditor,
    unitTable,
    HireEditor,
    i = 1;
var getNewDistance = function () {
    $.post("api/distancevalue")
        .success(function (e) {
            var distance = Array();
            for (var i in e.data) {
                var qp = {
                    label: e.data[i].distance_name,
                    value: e.data[i].distance
                }
                distance.push(qp);
            }
            unitEditor.field("unit_distance").update(distance);
        })
}
var getVessel = function () {
    $.post("api/getvessel")
        .success(function (e) {
            var vessel = Array();
            for (var i in e.data) {
                var qp = {
                    label: e.data[i].name,
                    value: e.data[i].name
                }
                vessel.push(qp);
            }
            HireEditor.field("vessel").update(vessel);
        })
}

$(document).ready(function () {
    editor = new $.fn.dataTable.Editor({
        ajax: "api/vessel",
        table: "#vesselTable",
        fields: [
            {
                label: "Vessel Name",
                name: "name",
            },
            {
                label: "Description",
                name: "vs_desc"
            }
        ]
    }).on('initCreate', function () {
        i = vesselTable.data().length + 1;
    }).on('remove', function () {
        i = 1;
        vesselTable.ajax.reload()
    }).on('edit', function () {
        i = 1;
        vesselTable.ajax.reload()
    })

    vesselTable = $('#vesselTable').DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/vessel",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "name" },
            { data: "vs_desc" }
        ],
        select: true,
        buttons: [
            { extend: 'create', editor: editor, text: "Add New Vessel" },
            { extend: 'edit', editor: editor },
            { extend: 'remove', editor: editor },
        ]
    }).on('init', function () {
        i = 1;
    })

    editor = new $.fn.dataTable.Editor({
        ajax: "api/activity",
        table: "#activityTable",
        fields: [
            { label: "Activity Name", name: "activity_name" },
            { label: "Description", name: "activity_ket" }
        ]
    });

    $('#activityTable').DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/activity",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "activity_name" },
            { data: "activity_ket" }
        ],
        select: true,
        buttons: [
            {
                extend: 'create',
                editor: editor,
                text: "Add New Activity",
            },
            {
                extend: 'edit',
                editor: editor
            },
            {
                extend: 'remove',
                editor: editor,
            },
        ]
    }).on('init', function () {
        i = 1;
    });


    unitEditor = new $.fn.dataTable.Editor({
        ajax: "api/unit",
        table: "#userTable",
        fields: [
            { name: "unit_name", label: "Unit Name" },
            {
                label: "Distance",
                name: "unit_distance",
                type: "select",
                className: ""
            },
            { name: "unit_ket", label: "Description" }
        ]
    });
    unitEditor.one('preOpen', function () {
        getNewDistance();
    })

    unitTable = $("#userTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/unit",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "unit_name" },
            { data: "unit_distance" },
            { data: "unit_ket" }
        ],
        select: true,
        buttons: [
            { extend: 'create', editor: unitEditor },
            { extend: 'edit', editor: unitEditor },
            { extend: 'remove', editor: unitEditor }
        ]
    }).on('init', function () {
        i = 1;
    });

    editor = new $.fn.dataTable.Editor({
        ajax: "api/distance",
        table: "#distanceTable",
        fields: [
            { label: "Area Name", name: "distance_name" },
            { label: "Distance", name: "distance" }
        ]
    })
    editor.on('edit', function () {
        getNewDistance();
    })

    $("#distanceTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/distance",
            type: 'post'
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "distance_name" },
            {
                data: null, render: function (data, type, row) {
                    return data.distance + " NMi";
                }
            }
        ],
        select: true,
        buttons: [
            { extend: "create", editor: editor, text: "Create new Area" },
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor }
        ]
    }).on('init', function () {
        i = 1;
    });

    editor = new $.fn.dataTable.Editor({
        ajax: "api/fuel",
        table: "#fuelTable",
        fields: [
            { label: "Date", name: "tgl", type: "datetime", format: "DD/MM/YYYY" },
            { label: "Cost", name: "cost" }
        ]
    })

    fuelTable = $("#fuelTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/fuel",
            type: "post"
        },
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "tgl" },
            {
                data: null, render: function (data, type, row) {
                    return "$ " + Number(data.cost_usd).toLocaleString();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "Rp. "+Number(data.cost_rp).toLocaleString();
                }
            }
        ],
        select: true,
        buttons: [
            //{ extend: "create", editor: editor, text: "Add new Daily Fuel" },
            {
                text: "New",
                action: function(){
                    $("#modalFuel").modal({backdrop:false})
                }
            },
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor }
        ]
    }).on('init', function () {
        i = 1;
    });

    HireEditor = new $.fn.dataTable.Editor({
        ajax: "api/hire",
        table: "#hireTable",
        fields: [
            { label: "Vessel", name: "vessel", type: "select" },
            { label: "Early Period", name: "s_period", type: "datetime", format: "DD/MM/YYYY" },
            { label: "End of Period", name: "e_period", type: "datetime", format: "DD/MM/YYYY" }
        ]
    })

    HireEditor.one('preOpen', function () {
        getVessel();
    })

    $("#hireTable").dataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/hire",
            type: 'post'
        },
        select: true,
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "vessel" },
            { data: "s_period" },
            { data: "e_period" }
        ],
        buttons: [
            { extend: "create", editor: HireEditor, text: "Add new Hire" },
            { extend: "edit", editor: HireEditor },
            { extend: "remove", editor: HireEditor }
        ]
    }).on('init', function () {
        i = 1;
    }).on('create', function () {
        i = 1;
    });

    editor = new $.fn.dataTable.Editor({
        ajax: "api/currency",
        table: "#currencyTable",
        fields: [
            { label: "Name", name: "currency_name" },
            { label: "Value", name: "currency_value" },
        ]
    })

    currencyTable = $("#currencyTable").DataTable({
        dom: '<B<"floatright"f>>rtip',
        ajax: {
            url: "api/currency",
            type: "post"
        },
        serverSide: true,
        columns: [
            { data: "currency_name" },
            { data: "currency_value" },
            { data: "last_up" }
        ],
        select: true,
        buttons: [
            //{ extend: "create", editor: editor },
            {
                text: "Edit", action: function (e, dt, node, config) {
                    var q = currencyTable.rows('.selected').indexes()
                    if (q.length !== 0) {
                        var a = currencyTable.row(q).data();
                        $("#modalCurrency").modal({
                            backdrop: false
                        })
                        $("#modalCurrency form input[name='currency_name']").val(a.currency_name);
                        $("#modalCurrency form input[name='currency_value']").val(a.currency_value);
                        $("#modalCurrency form input[name='currency_id']").val(a.id);
                    }
                }
            },
            { extend: "remove", editor: editor }
        ]
    })

    $("#modalCurrency form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/currency", data, function (res) {
            if(res == "success"){
                $("#modalCurrency").modal('hide');
                currencyTable.ajax.reload();
            }else{
                alert(res);
            }
        })
        e.preventDefault();
    })
    $("#modalFuel form input[name='tgl']").datetimepicker({
        format: "DD/MM/YYYY"
    })
    $("#modalFuel form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/fuel", data, function (res) {
            if (res=="true") {
                $("#modalFuel").modal('hide');
                fuelTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })
});