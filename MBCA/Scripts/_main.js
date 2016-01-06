var editor,
    unitEditor,
    unitTable;
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
    });

    $('#vesselTable').DataTable({
        dom: "Bfrtip",
        serverSide: true,
        ajax: {
            url: "api/vessel",
            type: 'post'
        },
        columns: [
            { data: "name" },
            { data: "vs_desc" }
        ],
        select: true,
        buttons: [
            { extend: 'create', editor: editor, text: "Add New Vessel" },
            { extend: 'edit', editor: editor },
            { extend: 'remove', editor: editor },
        ]
    });

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
        serverSide: true,
        ajax: {
            url: "api/activity",
            type: 'post'
        },
        columns: [
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
            { name: "unit_ket", label: "Description"}
        ]
    });
    unitEditor.one('preOpen', function () {
        getNewDistance();
    })

    unitTable = $("#userTable").DataTable({
        dom: "Bfrtip",
        serverSide: true,
        ajax: {
            url: "api/unit",
            type: 'post'
        },
        columns: [
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
    })

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
        serverSide: true,
        columns: [
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
    });

    editor = new $.fn.dataTable.Editor({
        ajax: "api/fuel",
        table: "#fuelTable",
        fields: [
            { label: "Date", name: "tgl", type: "datetime", format: "MM/DD/YYYY" },
            { label: "Cost", name: "cost" }
        ]
    })

    $("#fuelTable").DataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/fuel",
            type: "post"
        },
        columns: [
            { data: "tgl" },
            { data: "cost" }
        ],
        select: true,
        buttons: [
            { extend: "create", editor: editor, text: "Add new Daily Fuel" },
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor }
        ]
    })
        .order(0, 'desc');

    editor = new $.fn.dataTable.Editor({
        ajax: "api/hire",
        table: "#hireTable",
        fields: [
            { label: "Vessel", name: "vessel_name", type: "select" },
            { label: "Early Period", name: "s_period", type: "datetime", format: "M/D/YYY"},
            { label: "End of Period", name: "f_period", type: "datetime", format: "M/D/YYY" }
        ]
    })

    $("#hireTable").dataTable({
        dom: "Bfrtip",
        ajax: {
            url: "api/hire",
            type: 'post'
        },
        serverSide: true,
        select: true,
        columns: [
            //{ data: "name" },
            { data: "s_period" },
            { data: "f_period" }
        ],
        buttons: [
            { extend: "create", editor: editor, text: "Add new Hire" },
            { extend: "edit", editor: editor },
            { extend: "remove", editor: editor }
        ]
    });
});