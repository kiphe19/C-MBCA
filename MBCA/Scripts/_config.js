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
//var getVessel = function () {
//    $.post("api/getvessel")
//        .success(function (e) {
//            var vessel = Array();
//            for (var i in e.data) {
//                var qp = {
//                    label: e.data[i].name,
//                    value: e.data[i].name
//                }
//                vessel.push(qp);
//            }
//            HireEditor.field("vessel").update(vessel);
//        })
//}

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
                label: "Unit Category",
                name: "unit_cat",
                type: "select",
                options: [
                    { label: "0", value: "0" },
                    { label: "1", value: "1" }
                ]
            },
            {
                label: "Distance",
                name: "unit_distance",
                type: "select",
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
            { data: "unit_cat" },
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
                    return "$" + Number(data.cost_usd).toLocaleString();
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "Rp"+Number(data.cost_rp).toLocaleString();
                }
            }
        ],
        select: true,
        buttons: [
            {
                text: "New",
                action: function(){
                    $("#modalFuel").modal({ backdrop: false })
                    $("#modalFuel input[name='action']").val("create");
                }
            },
            {
                text: "Edit",
                action: function () {
                    var a = fuelTable.rows('.selected').indexes();
                    var b = fuelTable.row(a).data();
                    if (a.length !== 0) {
                        $("#modalFuel").modal({ backdrop: false })
                        $("#modalFuel input[name='tgl']").val(b.tgl);
                        $("#modalFuel input[name='cost']").val(b.cost_usd);
                        $("#modalFuel input[name='action']").val("update");
                        $("#modalFuel input[name='id']").val(b.id);
                    }
                }
            },
            { extend: "remove", editor: editor }
        ]
    }).on('init', function () {
        i = 1;
    });

    CharterEditor = new $.fn.dataTable.Editor({
        ajax: "api/hire",
        table: "#hireTable",
        fields: [
            { label: "Vessel", name: "vessel", type: "select" },
        ]
    })

    CharterTable = $("#hireTable").DataTable({
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
            { data: "tgl" },
            { data: "vessel" },
            {
                data: null, render: function (data) {
                    return "$"+Number(data.cost_usd).toLocaleString();
                }
            },
            {
                data: null, render: function (data) {
                    return "Rp" + Number(data.cost_rp).toLocaleString();
                }
            }
        ],
        buttons: [
            {
                text: "New",
                action: function(){
                    $("#modalCharter").modal({ backdrop: false });
                    $("#modalCharter button[type='submit']").text("Add")
                    $("#modalCharter input").val(null);
                    $("#modalCharter input[name='action']").val("create");
                }
            },
            {
                text: "Edit",
                action: function () {
                    var a = CharterTable.rows('.selected').indexes();
                    var b = CharterTable.row(a).data();
                    if (a.length > 0) {
                        $("#modalCharter").modal({ backdrop: false });
                        $("#modalCharter input[name='tgl']").val(b.tgl);
                        $("#modalCharter input[name='cost']").val(b.cost_usd);
                        $("#modalCharter input[name='action']").val("update");
                        $("#modalCharter input[name='id']").val(b.id);
                        $("#modalCharter button[type='submit']").text("Update")
                        $("#modalCharter select[id='vessel'").val(b.vessel)
                    }
                }
            },
            { extend: "remove", editor: CharterEditor }
        ]
    }).on('init', function () {
        i = 1;
    })

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

    editor = new $.fn.dataTable.Editor({
        ajax: "api/demob",
        table: "#demobTable"
    })

   DemobTable =  $("#demobTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/demob",
            type: "post"
        },
        select: true,
        columns: [
            {
                data: null, render: function (data, type, row) {
                    return i++;
                }
            },
            { data: "tgl" },
            { data: "vessel" },
            {
                data: null, render: function (data) {
                    return "$" + Number(data.cost_usd).toLocaleString();
                }
            },
            {
                data: null, render: function (data) {
                    return "Rp" + Number(data.cost_rp).toLocaleString();
                }
            }
        ],
        buttons: [
            {
                text: "New",
                action: function () {
                    $("#modalDemob").modal({ backdrop: false });
                    $("#modalDemob button[type='submit']").text("Add")
                    $("#modalDemob input").val(null);
                    $("#modalDemob input[name='action']").val("create");
                }
            },
            {
                text: "Edit",
                action: function () {
                    var a = DemobTable.rows('.selected').indexes();
                    var b = DemobTable.row(a).data();
                    if (a.length > 0) {
                        $("#modalDemob").modal({ backdrop: false });
                        $("#modalDemob input[name='tgl']").val(b.tgl);
                        $("#modalDemob input[name='cost']").val(b.cost_usd);
                        $("#modalDemob input[name='action']").val("update");
                        $("#modalDemob input[name='id']").val(b.id);
                        $("#modalDemob select[id='vessel'").val(b.vessel)
                        $("#modalDemob button[type='submit']").text("Update")
                    }
                }
            },
            {extend: "remove", editor: editor}
        ]
    }).on('init', function () {
        i = 1;
    });

    $("#modalCurrency form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/currency", data, function (res) {
            if(res == "success"){
                $("#modalCurrency").modal('hide');
                CharterTable.ajax.reload();
            }else{
                alert(res);
            }
        })
        e.preventDefault();
    })
    $("#modalCharter form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/charter", data, function (res) {
            if (res == "success") {
                $("#modalCharter").modal('hide');
                CharterTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })
    $("#modalDemob form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/demob", data, function (res) {
            if (res == "success") {
                $("#modalDemob").modal('hide');
                DemobTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })
    $("#modalFuel form input[name='tgl']").datetimepicker({
        format: "MM/DD/YYYY"
    })
    $("#modalCharter form input[name='tgl']").datetimepicker({
        format: "MM/DD/YYYY"
    })
    $("#modalDemob form input[name='tgl']").datetimepicker({
        format: "MM/DD/YYYY"
    })
    $("#modalFuel form").submit(function (e) {
        var data = $(this).serialize();
        $.post("api/cs/fuel", data, function (res) {
            if (res=="success") {
                $("#modalFuel").modal('hide');
                fuelTable.ajax.reload();
            } else {
                alert(res);
            }
        })
        e.preventDefault();
    })
});